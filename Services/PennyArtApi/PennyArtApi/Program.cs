
using PennyArtApi.ExternalServices;
using PennyArtApi.Models;

namespace PennyArtApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<CrossmintOptions>(builder.Configuration.GetSection("CrossmintOptions"));
            builder.Services.Configure<PinataOptions>(builder.Configuration.GetSection("PinataOptions"));

            builder.Services.AddHttpClient<PinataClient>();
            builder.Services.AddHttpClient<CrossmintClient>();

            builder.Services.AddScoped<IPinataClient, PinataClient>();
            builder.Services.AddScoped<ICrossmintClient, CrossmintClient>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}