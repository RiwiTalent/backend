using System.Net;
using System.Text;
using RiwiTalent.Services.Interface;
using RiwiTalent.Services.Repository;
using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RiwiTalent.Infrastructure.Data;
using RiwiTalent.Models;
using RiwiTalent.Models.DTOs;
using RiwiTalent.Validators;
using RiwiTalent.Utils.ExternalKey;

var builder = WebApplication.CreateBuilder(args);

const string MyCors = "PolicyCors";


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//DotNetEnv
Env.Load();

//Service to MongoDb
builder.Services.AddSingleton<MongoDbContext>();

//Services to Interface and Repository
builder.Services.AddScoped<ICoderRepository, CoderRepository>();
builder.Services.AddScoped<IGroupCoderRepository, GroupCoderRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ICoderStatusHistoryRepository, CoderStatusHistoryRepository>();
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();


//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Validator
builder.Services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddTransient<IValidator<GroupDto>, GroupCoderValidator>();
builder.Services.AddTransient<IValidator<CoderDto>, CoderValidator>();


builder.Services.AddTransient<ExternalKeyUtils>();

//CORS
builder.Services.AddCors(options => {
    options.AddPolicy(MyCors, builder => 
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

//Configuration JWT with environment variables
builder.Services.AddAuthentication(option => {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(configure => {
            configure.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("Issuer"),
                ValidAudience = Environment.GetEnvironmentVariable("Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key")))
            };
            //Error controls of token
            configure.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    if(context.Exception is SecurityTokenExpiredException)
                    {
                        Console.WriteLine("Token expirado, porfavor genere uno nuevo.");
                    }
                    else
                    {
                        Console.WriteLine("Usuario no autorizado.");
                    }
                    return Task.CompletedTask;
                }
            };
        });


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//middleware cors
app.UseCors("PolicyCors");

app.UseAuthentication();
app.UseAuthorization();


//Controllers
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
