using System.Net;
using System.Text;
using RiwiTalent.Services.Repository;
using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RiwiTalent.Domain.Services.Interface.Coders;
using RiwiTalent.Infrastructure.Persistence.Repository;
using RiwiTalent.Domain.Services.Groups;
using RiwiTalent.Domain.Services.Interface.Login;
using RiwiTalent.Domain.Services.Interface.Emails;
using RiwiTalent.Infrastructure.Persistence.Emails;
using RiwiTalent.Domain.Services.Interface.Technologies;
using RiwiTalent.Domain.Services.Interface.Terms;
using RiwiTalent.Infrastructure.ExternalServices;
using RiwiTalent.Application.DTOs;
using RiwiTalent.Domain.Validators;
using RiwiTalent.Domain.ExternalKey;
using RiwiTalent.Infrastructure.ExternalServices.MailKit;
using RiwiTalent.Application.AutoMapper;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using RiwiTalent.Domain.Services.Interface.Users;
using RiwiTalent.Infrastructure.Persistence.Repository.Users;

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupCoderRepository, GroupCoderRepository>();
builder.Services.AddScoped<ICoderStatusHistoryRepository, CoderStatusHistoryRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddTransient<IEmailSelectedRepository, EmailSelectedRepository>();
builder.Services.AddScoped<ITechnologyRepository, TechnologyRepository>();
builder.Services.AddScoped<ITermAndConditionRepository, TermAndConditionRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();


//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(GroupCoderProfile));



//Validator
builder.Services.AddTransient<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddTransient<IValidator<GroupDto>, GroupCoderValidator>();
builder.Services.AddTransient<IValidator<CoderDto>, CoderValidator>();

//Utils
builder.Services.AddTransient<ExternalKeyUtils>();
builder.Services.AddTransient<SendFile>();

//Services Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton(c =>
{
    var cloudinarySettings = c.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(
        cloudinarySettings.CloudName,
        cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret
    );

    return new Cloudinary(account);
});

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

