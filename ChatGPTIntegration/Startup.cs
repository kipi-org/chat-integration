using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ChatGPTIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChatGPTIntegration.Auth;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatGPTIntegration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Позволяет использовать имена свойств как в JSON
                options.JsonSerializerOptions.DictionaryKeyPolicy = null; // Позволяет использовать ключи словарей как в JSON
                options.JsonSerializerOptions.IgnoreNullValues = true; // Игнорировать null-значения
            });

            services.AddScoped<IAuthorizationFilter, ApiKeyAuthorizationFilter>();
            services.AddHttpClient<ChatGPTService>();
            services.AddSingleton<IApiKeyService, ApiKeyService>();

            // Добавление вашего сервиса
            services.AddScoped<ChatGPTService>();

            services.AddDbContext<MessagesContext>(options =>
                options.UseNpgsql(Configuration.GetSection("DefaultConnection").Value.ToString())
            );

            // Добавление Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Добавление Swagger UI в режиме разработки
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MessagesContext>();
                dbContext.Database.Migrate();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
