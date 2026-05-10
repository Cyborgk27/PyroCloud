using PyroCloud.Core.Application.Extensions;
using PyroCloud.Shared.Infrastructure.Extensions;
using PyroCloud.Shared.Infrastructure.Filters;
using PyroCloud.Shared.Infrastructure.Setup;
using PyroCloud.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddInjectionInfrastructure(configuration);
builder.Services.AddInjectionApplication(configuration);
builder.Services.AddModules(configuration);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResponseWrapperFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var seeder = serviceProvider.GetRequiredService<SeedDataService>();
        await seeder.SeedAsync();
        Console.WriteLine("Seed Data ejecutado correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al ejecutar Seed Data: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructureMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseLocalStorage();

app.MapControllers();

app.Run();
