using Interface.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Service.Context;
using Service.Helpers;
using Service.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// ? ADDED (network par API expose karne ke liye)
//builder.WebHost.UseUrls("https://0.0.0.0:7228", "http://0.0.0.0:5201");


// ---------------- SERILOG ----------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "log", "log-.txt"),
    rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("IdentityService - Starting Application");


// ---------------- CONFIGURATION ----------------
var configuration = builder.Configuration;
string secretKey = configuration["SecretKey:JwtSecretKey"];

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


// ---------------- SERVICES ----------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();


// ---------------- DATABASE ----------------
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// ---------------- DEPENDENCY INJECTION ----------------
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IClaimService, ClaimServices>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientClaimService, ClientClaimServices>();
builder.Services.AddScoped<IUserService, UserService>();

// ---------------- SWAGGER ----------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT Bearer token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Authorization Configuration
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(PermissionRole.ADMIN, policy =>
//        policy.RequireClaim("Role", PermissionRole.ADMIN));
//});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PermissionRole.ADMIN, policy =>
        policy.RequireClaim(CustomClaimTypes.Role, PermissionRole.ADMIN));
    options.AddPolicy(PermissionRole.ADMINORCLIENT,
        policy =>
        {
            policy.RequireAssertion(context =>
            context.User.HasClaim(CustomClaimTypes.Role, PermissionRole.ADMIN) ||
            context.User.HasClaim(CustomClaimTypes.Role, PermissionRole.CLIENT));
        });
});
// ---------------- CORS ----------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// ---------------- AUTHENTICATION ----------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Log.Error($"Authentication Failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },

        OnTokenValidated = context =>
        {
            Log.Information("Token validated successfully");
            return Task.CompletedTask;
        }
    };
});


var app = builder.Build();


// ---------------- MIDDLEWARE ----------------

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


// ---------------- SWAGGER ----------------

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "Identity/swagger";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API V1");
});


app.MapControllers();

app.MapGet("/Identity", async context =>
{
    await context.Response.WriteAsync("Listening Identity Service....");
});

Log.Information("IdentityService - Application Ready");

app.Run();