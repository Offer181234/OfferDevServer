using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

public class Program
{

    private static async Task Main(string[] args)
    {
        // Configure Serilog with the context and logging settings
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "log", "log-.txt"),
            rollingInterval: RollingInterval.Day).CreateLogger();

        //Log the Application Starttup
        Log.Information("OfferService -starting up application...");

        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        string secretKey = configuration["SecretKey:JwtSecretKey"];

        //string secretkey = configuration["SecretKey:JwtSecrretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        builder.Configuration.AddEnvironmentVariables();

        Log.Information("OfferService - Configuring Service...");

        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMvc();
        //builder.Services.AddScoped<>()
        //builder.Services.AddScoped<>()

        //builder.Services.AddScoped<IdataService, DataService>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Offer API",
                Version = "v1",
                Description = "API for managing Offer-related features."
            });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description="JWT Authorization header using the Bearer schema",
                Type= SecuritySchemeType.Http,
                Scheme="bearer"
            });
            //c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference= new OpenApiReference
            //            {
            //                Type=ReferenceType.SecurityScheme, Id="Bearer"
            //            }
            //        },
            //        new string[]{}
            //    }
            //});

        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();

                });
        });

        //log authentication configuration
        Log.Information("OfferService - Configuraing authentication...");

        //Authentication Configration
        // JWT Authentication
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
            OnChallenge = HandleOnChallengeAsync,

            OnForbidden = HandleOnForbiddenAsync,

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
        });
      
        Log.Information("OfferService - Configuration authorization...");

        //Authorization Configuration
        builder.Services.AddAuthentication(options =>
        {
            

        });

        var app =builder.Build();

        //Log Application SetUp Completion
        Log.Information("OfferService - Offer Service SetUp Complted Application building...");

        //Swagger SetUp For Dev envirment
        Log.Information("OfferService - Setting up Swagger UI ...");
        //app.Use(async (context, next) =>
        //{
        //    if (context.Request.Path.Value == "/offer/swagger/offer/swagger.json")
        //    {
        //        context.Request.Path = "/swagger/offer/swagger.json";
        //    }
        //    await next();
        //});
        //app.UseSwagger();
        //app.UseSwaggerUI(opation =>
        //{
        //    opation.SwaggerEndpoint("/offer/swagger/offer/swagger.json", "Offer APIs V1");
        //    opation.RoutePrefix = "offer/swagger";
        //});
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "offer/swagger";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Offer API V1");
        });
        //log ,iddleware confing
        Log.Information("OfferService - Configuring middleware..");
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/offer", async context =>
        {
            await context.Response.WriteAsync("Listening offer Service....");
        });
        //log when the application ready
        Log.Information("OfferService - application is ready to handle request");
        app.Run();
    }

    private static Task HandleOnChallengeAsync(JwtBearerChallengeContext context)
    {
        context.HandleResponse();
        context.Response.StatusCode=StatusCodes.Status401Unauthorized; 
        return Task.CompletedTask;
    }


    private static Task HandleOnForbiddenAsync(ForbiddenContext context)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden; 
        return Task.CompletedTask;
    }

}