
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.Models;

namespace MoviesWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");

            builder.Services.AddDbContext<MyMoviesContext>(options => options.UseSqlServer(connectionString));

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

            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
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

            app.UseAuthorization();

            app.MapControllers();

            app.UseCors("PermettiFrontend");

            app.Run();
        }
    }
}
