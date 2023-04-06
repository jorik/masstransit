using concurrency;
using concurrency.Components;
using concurrency.Components.Database;
using MassTransit;
using Microsoft.EntityFrameworkCore;

const string connectionString = "Server=localhost;Port=3306;Database=test;Uid=test;Pwd=test";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestContext>(optionsBuilder =>
{
    optionsBuilder.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        options => { options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(1), Array.Empty<int>()); });
});

builder.Services.AddMassTransit(mt =>
{
    mt.SetEntityFrameworkSagaRepositoryProvider(r =>
    {
        r.UseMySql();
        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
        r.ExistingDbContext<TestContext>();
    });

    mt.AddSagaStateMachines(typeof(TestStateMachine).Assembly);
    mt.AddConsumers(typeof(TestStateMachine).Assembly);

    mt.UsingRabbitMq((context, configurator) =>
    {
        configurator.UseInMemoryOutbox();
        configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using var dbContext = app.Services
    .GetRequiredService<IServiceScopeFactory>()
    .CreateScope()
    .ServiceProvider
    .GetRequiredService<TestContext>();

dbContext.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
