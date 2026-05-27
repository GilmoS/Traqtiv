using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Traqtiv.API.DAL;
using Traqtiv.API.DAL.Interfaces;
using Traqtiv.API.Data;
using Traqtiv.API.Services;
using Traqtiv.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy =>
    {
        policy.WithOrigins("http://localhost:5173","http://10.0.2.2:5203","http://localhost:5203").AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container
// Configure JSON serialization to use string representation for enums,
// improving readability and compatibility with client applications that consume the API
builder.Services.AddControllers().AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to include JWT authentication in the API documentation, allowing developers to test authenticated endpoints directly from the Swagger UI
// This configuration adds a security definition for Bearer tokens and a security requirement to ensure that the Swagger UI prompts for a JWT token when testing protected endpoints
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add DbContext
builder.Services.AddDbContext<SmartFitnessDb>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add DAL
builder.Services.AddScoped<ISmartFitnessDal, SmartFitnessDal>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IDailyActivityService, DailyActivityService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddHttpClient<WeatherService>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

// Configure JWT authentication with the specified parameters, including issuer, audience, and signing key
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowWeb"); // Enable CORS with the defined policy to allow requests from the specified origin (http://localhost:5173)




// Enforce HTTPS, enable authentication and authorization middleware, and map controller routes to handle incoming requests
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// The Program class is defined as a partial class to allow for separation of concerns and better organization of code and for easier testing
//,especially when using integration tests that require a reference to the Program class for setting up the test server and configuring the application environment
public partial class Program { }
