using SkyRouteTravel.Api.Extensions;
using SkyRouteTravel.Api.Middleware;
using SkyRouteTravel.Application;
using SkyRouteTravel.Application.Providers;
using SkyRouteTravel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAutoMapper((serviceProvider, cfg) =>
{
    var strategyFactory = serviceProvider.GetRequiredService<IFlightProviderStrategyFactory>();
    cfg.AddProfile(new SkyRouteTravel.Api.Mapping.ApiMappingProfile(strategyFactory));
    cfg.AddMaps(typeof(SkyRouteDbContext).Assembly);
}, Array.Empty<System.Reflection.Assembly>());

var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

// Seed Database
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SkyRouteDbContext>();
    await SkyRouteDbContext.SeedDataAsync(context);
}

app.MapFlightEndpoints();
app.MapProviderEndpoints();
app.MapBookingEndpoints();

app.Run();
