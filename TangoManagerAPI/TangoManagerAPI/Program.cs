using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Events.QuizAggregateEvents;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Handlers;
using TangoManagerAPI.Infrastructures.Repositories;
using TangoManagerAPI.Infrastructures.Routers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<IPaquetRepository,PaquetRepository>();
builder.Services.AddSingleton<IQuizRepository,QuizRepository>();
//builder.Services.AddSingleton<ICartesRepository>();


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

    return queryRouter;
});

builder.Services.AddSingleton<ICommandRouter>(p => {
    var commandRouter = p.GetRequiredService<CommandRouter>();

    var handler = p.GetRequiredService<CommandHandler>();

    commandRouter.AddCommandHandler<CreatePaquetCommand>(handler);
    commandRouter.AddCommandHandler<CreateQuizCommand>(handler);
    commandRouter.AddCommandHandler<AnswerQuizCommand>(handler);

    return commandRouter;
});

builder.Services.AddSingleton<IEventRouter>(p => {
    var eventRouter = p.GetRequiredService<EventRouter>();

    var handler = p.GetRequiredService<EventsHandler>();

    eventRouter.AddEventHandler<QuizAnsweredEvent>(handler);
    eventRouter.AddEventHandler<QuizCardEntityAddedEvent>(handler);
    eventRouter.AddEventHandler<PacketUpdatedEvent>(handler);
    eventRouter.AddEventHandler<CardUpdatedEvent>(handler);

    return eventRouter;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
//app.UseAuthorization();

app.MapControllers();

app.Run();
