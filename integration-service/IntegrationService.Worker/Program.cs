using IntegrationService.AgvProviderFactoryUs.ProgramExtensions;
using IntegrationService.Worker;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers(options => { options.Filters.Add(new ProducesAttribute("application/json")); });
builder.Services.AddSwaggerGen();
builder.Services.AddAgvProviderFactoryUsIntegration(builder.Configuration.GetSection("RabbitMq").Bind);
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

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
