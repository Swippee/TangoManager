using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Ports.Routers;
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

    queryRouter.AddQueryHandler<GetAllPaquetsQuery>(handler);   

    return queryRouter;
});

builder.Services.AddSingleton<ICommandRouter>(p => {
    var commandRouter = p.GetRequiredService<CommandRouter>();

    var handler = p.GetRequiredService<CommandHandler>();

    commandRouter.AddCommandHandler<CreateNewPaquetCommand>(handler);

    return commandRouter;
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
