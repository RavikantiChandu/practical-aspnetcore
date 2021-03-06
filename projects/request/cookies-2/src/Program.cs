using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Hosting;

namespace PracticalAspNetCore 
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.Headers.Add("content-type","text/html");
                var deleteCookie = context.Request.Query["delete"];

                if(!string.IsNullOrWhiteSpace(deleteCookie))
                {
                    context.Response.Cookies.Delete("MyCookie");
                    await context.Response.WriteAsync($@"Delete cookie. Click <a href=""\"">here</a> to go back to home page.");
                    return;
                }

                var cookie = context.Request.Cookies["MyCookie"];

                if (string.IsNullOrWhiteSpace(cookie) && context.Request.Path == "/")  //read https://github.com/aspnet/HttpAbstractions/issues/743
                {
                    context.Response.Cookies.Append
                    (
                        "MyCookie",
                        "Hello World",
                        new CookieOptions{
                            Path = "/",
                            Expires = DateTimeOffset.Now.AddDays(1)
                        }
                    );

                    await context.Response.WriteAsync($"Writing a new cookie <br/>Refresh page to see cookie value.<br/>");
                }
                else
                {
                    await context.Response.WriteAsync($@"Click <a href=""\?delete=1"">here</a> to delete cookie.<br/>");
                }

                await context.Response.WriteAsync($"Content of Cookie: {cookie}.");
            });
        }
    }
    
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}