using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Handlers;
using TangoManagerAPI.Infrastructures.Repositories;
using TangoManagerAPI.Infrastructures.Routers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSingleton<IPaquetRepository,PaquetRepository>();
builder.Services.AddSingleton<QueryHandler>();
builder.Services.AddSingleton<CommandHandler>();

builder.Services.AddSingleton<QueryRouter>();
builder.Services.AddSingleton<CommandRouter>();

builder.Services.AddSingleton<IQueryRouter>(p => {
    var queryRouter = p.GetRequiredService<QueryRouter>();

    var handler = p.GetRequiredService<QueryHandler>();

    queryRouter.AddQueryHandler<GetPaquetsQuery>(handler);
    queryRouter.AddQueryHandler<GetPaquetByNameQuery>(handler);
    queryRouter.AddQueryHandler<DeletePaquetByNameQuery>(handler);
    return queryRouter;
});
builder.Services.AddSingleton<ICommandRouter>(p =>
{
    var router = p.GetRequiredService<CommandRouter>();

    var handler = p.GetRequiredService<CommandHandler>();

    router.AddCommandHandler<CreatePaquetAsyncCommand>(handler);
    router.AddCommandHandler<UpdatePaquetAsyncCommand>(handler);

    return router;
});

builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy",
    policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
    }));

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
