using Microsoft.AspNetCore.Mvc;
using OrderService.Api.ProgramExtensions;
using OrderService.Application.ProgramExtensions;
using OrderService.OrderManager.ProgramExtensions;
using OrderService.Persistence.ProgramExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add(new ProducesAttribute("application/json")); });

builder.Services.AddServices()
    .AddDbContext(builder.Configuration)
    .AddRepositories()
    .AddEndpointsApiExplorer()
    .ConfigureSwagger()
    .AddValidators()
    .AddEventPublisher(builder.Configuration.GetSection("RabbitMq").Bind);

builder.Services.AddOrderManagerServices();

var app = builder.Build();

app.InitializeDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
        options.DisplayOperationId();
        options.DisplayRequestDuration();
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(o => { o.MapControllers(); });
app.Run();

/// <summary>
/// Required for Integration tests
/// </summary>
public partial class Program
{
}
