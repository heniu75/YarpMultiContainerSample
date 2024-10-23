
namespace YarpProxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configSectionName = GetYarpConfigSectionName();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection(configSectionName));

            var app = builder.Build();
            app.MapGet("/sayHello", () =>
            {
                var x = 10;
                return $"YarpProxy says: \"Hello, the time is {DateTime.Now}\"";
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapReverseProxy();

            app.MapControllers();

            app.Run();
        }

        private static string GetYarpConfigSectionName()
        {
            var docker_env = Environment.GetEnvironmentVariable("DOCKER_ENV");
            var yarpConfig = docker_env == "true"
                ? "DockerReverseProxy"
                : "ReverseProxy";
            return yarpConfig;
        }
    }

}
