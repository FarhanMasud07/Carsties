using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  /*  
The line builder.Services.AddControllers(); 

is used in ASP.NET Core applications to register the necessary services for using controllers in your web application.

Here's why it's important:

Dependency Injection: It enables the built-in dependency injection (DI) system to provide instances of your controllers when needed.

MVC Pattern: It sets up the Model-View-Controller (MVC) architecture, allowing you to handle HTTP requests with controller actions.

Routing: It configures routing for your API endpoints, mapping incoming requests to the appropriate controller actions based on the URL and HTTP method.

JSON Formatting: It adds support for JSON serialization and deserialization, which is crucial for web APIs.

Filters and Middleware: It enables the use of filters (like authorization, logging, etc.) and integrates with middleware that can process requests and responses.

Overall, calling AddControllers() is essential for setting up the foundation of a web API in an ASP.NET Core application. */

builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
