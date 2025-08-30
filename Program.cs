
using Microsoft.EntityFrameworkCore;
using StudentPerformanceAPI.Data;

namespace StudentPerformanceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>

                options.UseSqlServer(builder.Configuration.GetConnectionString("cs1")));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontEnd",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200","https://localhost:4200") // or your Angular URL
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();

                app.UseSwaggerUI();

            }

            app.UseCors("AllowFrontEnd");
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
 