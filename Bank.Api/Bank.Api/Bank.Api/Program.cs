using Application.Repository;
using Application.Validation;
using Bank.Api;
using Domain;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options=>options.AddPolicy("bankploicy",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173/")
            .AllowAnyMethod()
            .AllowAnyHeader();
    }));
var connectionString = builder.Configuration["ConnectionStrings:Sql"];
builder.Services.AddDbContext<RepositoryDbContext>(o => o.UseSqlServer(connectionString,sqlOptions =>
{
    sqlOptions.MigrationsAssembly("Domain");
}));
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddAutoMapper(typeof(BankMapper));
builder.Services.AddValidatorsFromAssemblyContaining<AccountUserValidation>();
builder.Services.AddSwaggerGen();
var jwtKey = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("bankploicy");
app.Run();
