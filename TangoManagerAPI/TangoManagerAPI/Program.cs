using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;
using TangoManagerAPI.Application.Commands.CommandsAuth;
using TangoManagerAPI.Application.Queries.CommandsAuth;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Events.PacketLockEntityEvents;
using TangoManagerAPI.Entities.Events.QuizAggregateEvents;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Handlers;
using TangoManagerAPI.Infrastructures.Repositories;
using TangoManagerAPI.Infrastructures.Routers;
using TangoManagerAPI.Middleware;
using TangoManagerAPI.Swagger;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
}));
builder.Services.AddSingleton<IPaquetRepository,PaquetRepository>();
builder.Services.AddSingleton<IQuizRepository,QuizRepository>();
builder.Services.AddSingleton<IPacketLockRepository,PacketLockRepository>();


builder.Services.AddSingleton<QueryHandler>();
builder.Services.AddSingleton<CommandHandler>();

builder.Services.AddSingleton<QueryRouter>();
builder.Services.AddSingleton<CommandRouter>();

builder.Services.AddSingleton<EventRouter>();
builder.Services.AddSingleton<EventsHandler>();

builder.Services.AddSingleton<IQueryRouter>(p => {
    var queryRouter = p.GetRequiredService<QueryRouter>();

    var handler = p.GetRequiredService<QueryHandler>();

    queryRouter.AddQueryHandler<GetAllPaquetsQuery>(handler);
    queryRouter.AddQueryHandler<GetPacketLockQuery>(handler);

    return queryRouter;
});

builder.Services.AddSingleton<ICommandRouter>(p => {
    var commandRouter = p.GetRequiredService<CommandRouter>();

    var handler = p.GetRequiredService<CommandHandler>();

    commandRouter.AddCommandHandler<CreatePaquetCommand>(handler);
    commandRouter.AddCommandHandler<DeletePaquetCommand>(handler);
    commandRouter.AddCommandHandler<CreateQuizCommand>(handler);
    commandRouter.AddCommandHandler<AnswerQuizCommand>(handler);
    commandRouter.AddCommandHandler<AddCardToPacketCommand>(handler);
    commandRouter.AddCommandHandler<LockPacketCommand>(handler);
    commandRouter.AddCommandHandler<UnlockPacketCommand>(handler);

    return commandRouter;
});

builder.Services.AddSingleton<IEventRouter>(p => {
    var eventRouter = p.GetRequiredService<EventRouter>();

    var handler = p.GetRequiredService<EventsHandler>();

    eventRouter.AddEventHandler<QuizAnsweredEvent>(handler);
    eventRouter.AddEventHandler<QuizCardEntityAddedEvent>(handler);
    eventRouter.AddEventHandler<PacketUpdatedEvent>(handler);
    eventRouter.AddEventHandler<CardUpdatedEvent>(handler);
    eventRouter.AddEventHandler<PacketLockAccessedEvent>(handler);

    return eventRouter;
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerPacketTokenParameter>();
});

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddMemoryCache(options =>
{
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
});

var app = builder.Build();
var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerGenOptions>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        foreach (var (name, _) in swaggerOptions.Value.SwaggerGeneratorOptions.SwaggerDocs)
        {
            c.SwaggerEndpoint($"/swagger/{name}/swagger.json", name);
        }
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
//app.UseAuthorization();

app.MapControllers();
app.MapSwagger("swagger/{documentName}/swagger.json");
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
