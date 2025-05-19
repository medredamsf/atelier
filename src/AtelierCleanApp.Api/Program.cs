using AtelierCleanApp.Api.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Main: Application starting ---");
        var builder = WebApplication.CreateBuilder(args);

        Console.WriteLine("--- Main: Setting AddUserSecrets ---");
        builder.Configuration.AddUserSecrets<Program>();
        Console.WriteLine("--- Main: Returned from AddUserSecrets ---");

        Console.WriteLine("--- Main: Calling ConfigureApplicationServices ---");
        builder.Services.ConfigureApplicationServices(builder.Configuration);
        Console.WriteLine("--- Main: Returned from ConfigureApplicationServices ---");

        var app = builder.Build();

        Console.WriteLine("--- Main: Calling ConfigureRequestPipeline ---");
        app.ConfigureRequestPipeline();
        Console.WriteLine("--- Main: Returned from ConfigureRequestPipeline ---");

        Console.WriteLine("--- Main: Calling app.Run() ---");
        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--- Main: app.Run() EXITED WITH EXCEPTION: {ex.ToString()} ---");
        }
        
        Console.WriteLine("--- Main: app.Run() has exited (this means the web server stopped) ---");
    }
}