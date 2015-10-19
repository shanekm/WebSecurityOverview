namespace KatanaIntro
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Microsoft.Owin.Hosting;
    using Owin;

    internal class Program
    {
        private static void Main(string[] args)
        {
            string uri = "http://localhost:8010";

            // Start Katana using Startup class configuration
            // WebApp - katana specific - to explicitly start web server and have it listen at specific location 
            // and tell it how to configure itself
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started!");
                Console.ReadKey();
                Console.WriteLine("Stopping!");
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Use katana specific welcome page
            //app.UseWelcomePage();

            // Takes one param of type IOwinContext and returns a Task - Component
            //app.Run(ctx => { return ctx.Response.WriteAsync("Hello World!"); });

            // Writing anynomous Middleware
            // without creating class and extension method
            app.Use(
                async (env, next) =>
                    {
                        foreach (var pair in env.Environment)
                        {
                            Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
                        }

                        await next();
                    });

            // Another component
            app.Use(
                async (env, next) =>
                    {
                        // All this is request
                        foreach (var pair in env.Environment)
                        {
                            Console.WriteLine("Requesting: " + env.Request.Path);
                        }

                        await next();

                        // All this is response
                        Console.WriteLine("Response: " + env.Response.StatusCode);
                    });

            // Web API stuff
            // If this route is matched no other components are executed since the request matched with the response
            ConfigureWebApi(app);

            // Register new component
            //app.Use<HelloWorldComponent>();

            // Using extension method to call new HelloWorldComponent
            app.UseHelloWorld();
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            // controls routing, serializers, formatting etc
            // configure so that it knows how to response to incoming requests
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                "DefaultApi", 
                "api/{controller}/{id}", 
                new { id = RouteParameter.Optional });

            // Use web api component - OWIN has this by default
            app.UseWebApi(config);
        }
    }

    // Will write Hello World! to every request
    public class HelloWorldComponent
    {
        private Func<IDictionary<string, object>, Task> _next;

        // Requred - AppFunc - constructor that takes a func
        // using AppFunc = Func<IDictionary<string, object>, Task> 
        public HelloWorldComponent(Func<IDictionary<string, object>, Task> next)
        {
            _next = next;
        }

        // public async Task Invoke(IDictionary<string, object> environment)
        public Task Invoke(IDictionary<string, object> environment)
        {
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello!!!"); // will output Hello
            }

            //await _next(environment);
        }
    }

    public static class AppBuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder appBuilder)
        {
            appBuilder.Use<HelloWorldComponent>();
        }
    }
}