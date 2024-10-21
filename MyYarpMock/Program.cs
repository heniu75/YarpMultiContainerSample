using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyYarpMock
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configSectionName = GetYarpConfigSectionName();

            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection(configSectionName));

            var app = builder.Build();

            app.MapGet("/sayHello", () =>
            {
                var x = 10;
                return $"Yarp says: \"Hello, the time is {DateTime.Now}\"";
            });

            app.MapReverseProxy();

            app.Run();
        }

        private static string GetYarpConfigSectionName()
        {
            var docker_env = Environment.GetEnvironmentVariable("DOCKER_ENV");
            var yarpConfig = docker_env == "true"
                ? "ReverseProxyDocker"
                : "DockerReverseProxy";
            return yarpConfig;
        }
    }
}
