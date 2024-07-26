using Actioneer;
using Actioneer.Api;
using Actioneer.Api.Actions;
using Actioneer.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IActioneerDispatcher, ActioneerDispatcher>();
builder.Services.AddTransient<EchoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
        "/weatherforecast",
        async (
            IActioneerDispatcher actioneer,
            ILogger<GetWeatherForecast> logger,
            CancellationToken cancellationToken
        ) => await actioneer.DispatchAsync(new GetWeatherForecast(logger), cancellationToken)
    )
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
