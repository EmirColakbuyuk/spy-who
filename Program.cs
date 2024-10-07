using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Data;
using SpyFallBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register the controllers with dependency injection.
builder.Services.AddControllers();

// Register Swagger services for API documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the GameService and any other services you plan to use.
builder.Services.AddScoped<GameService>(); 
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<WordListService>();


// Register the ApplicationDbContext and configure the SQL Server connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
        // Enable retry on failure for transient errors.
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Number of retries before failing
            maxRetryDelay: TimeSpan.FromSeconds(10), // Delay between retries
            errorNumbersToAdd: null); // Optional: Specify additional error numbers to retry on
    }));

var app = builder.Build();

// Enable Swagger only in development mode for security reasons.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware to use HTTPS redirection for security.
app.UseHttpsRedirection();

// Middleware to manage request authorization.
app.UseAuthorization();

// Map controller routes.
app.MapControllers();

// Run the application.
app.Run();
