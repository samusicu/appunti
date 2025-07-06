
using AutoMapper;
using Autovelox.Application.Services;
using Autovelox.Data.Models;
using Autovelox.WebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace Autovelox.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<AutoveloxContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<MappeService>();
            builder.Services.AddScoped<ComuniService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermettiFrontend", builder =>
                {
                    builder
                        .WithOrigins("http://127.0.0.1:5500")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.WriteIndented = true; // Opzionale, per debug
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var tempProvider = builder.Services.BuildServiceProvider();
            var mapperConfig = tempProvider.GetRequiredService<IMapper>().ConfigurationProvider;
            mapperConfig.AssertConfigurationIsValid();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseCors("PermettiFrontend");

            app.Run();
        }
    }
}
