using BlogApp.Application.Services.AuthenticationService;
using BlogApp.Application.Services.TokenService;
using BlogApp.Application.Services;
using BlogApp.Core.Domain.Interfaces;
using BlogApp.Core.Domain.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogApp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using BlogApp.Infrastructure.Persistence.Data;
using BlogApp.Core.Domain.Service;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);
try
{

    var connectionString = builder.Configuration.GetConnectionString("MyDb");

builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(connectionString));
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring DbContext: {ex.Message}");
}

builder.Services.AddControllers();



builder.Services.AddSingleton(x =>
{
    var blobConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
    return new BlobServiceClient(blobConnectionString);
});

builder.Services.AddSingleton<BcryptPassword>();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddScoped<Authenticator>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IBlogEntryRepository, BlogEntryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddPolicy(name: "blogapp", policy =>
    {
        // Especifica los orígenes permitidos
        policy.WithOrigins("https://localhost:4200", "http://localhost:4200") // Reemplaza con los orígenes que necesites
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Permite el envío de credenciales
    })
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Manejar errores aquí, por ejemplo, registrar el error
        Console.WriteLine($"Error: {ex.Message}");
        context.Response.StatusCode = 500; // Puedes ajustar esto según el tipo de error
        await context.Response.WriteAsync("An error occurred.");
    }
});

app.UseCors("blogapp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
