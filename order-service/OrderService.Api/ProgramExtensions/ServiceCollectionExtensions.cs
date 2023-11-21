using Microsoft.OpenApi.Models;

namespace OrderService.Api.ProgramExtensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\" (REQUIRES THE TOKEN TO START WITH 'Bearer ')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            // {
            //     {
            //         new OpenApiSecurityScheme
            //         {
            //             Reference = new OpenApiReference
            //             {
            //                 Type = ReferenceType.SecurityScheme,
            //                 Id = "Bearer"
            //             },
            //             Scheme = "oauth2",
            //             Name = "Bearer",
            //             In = ParameterLocation.Header,
            //         },
            //         new List<string>()
            //     }
            // });

            var filePath = Path.Combine(AppContext.BaseDirectory, "OrderService.Api.xml");
            options.IncludeXmlComments(filePath);
        });

        return services;
    }
}
