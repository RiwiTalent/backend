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
using RiwiTalent.Utils.MailKit;

var builder = WebApplication.CreateBuilder(args);

const string MyCors = "PolicyCors";


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

//DotNetEnv
Env.Load();

//Service to MongoDb
builder.Services.AddSingleton<MongoDbContext>();

//Services to Interface and Repository
builder.Services.AddHttpClient();
builder.Services.AddScoped<ICoderRepository, CoderRepository>();
builder.Services.AddScoped<IGroupCoderRepository, GroupCoderRepository>();
builder.Services.AddScoped<ICoderStatusHistoryRepository, CoderStatusHistoryRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddScoped<ITermAndConditionRepository, TermAndConditionRepository>();
builder.Services.AddScoped<ILogginRepository, LogginRepository>();


//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Validator
builder.Services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddTransient<IValidator<GroupDto>, GroupCoderValidator>();
builder.Services.AddTransient<IValidator<CoderDto>, CoderValidator>();

//Utils
builder.Services.AddTransient<ExternalKeyUtils>();
builder.Services.AddTransient<SendFile>();

//CORS
builder.Services.AddCors(options => {
    options.AddPolicy(MyCors, builder => 
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

/* app.UseHttpsRedirection(); */

//middleware cors
app.UseCors("PolicyCors");

app.UseAuthentication();



//Controllers
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
