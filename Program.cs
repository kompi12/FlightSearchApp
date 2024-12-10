var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow your frontend's origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // If credentials (cookies, authorization) are needed
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowSpecificOrigin");

// Enable HTTPS redirection (make sure you use HTTPS)
app.UseHttpsRedirection();

// Enable authorization (if needed for your app)
app.UseAuthorization();
app.MapControllers();

app.Run();
