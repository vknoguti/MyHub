using MyHub.Data;
using MyHub.Extensions;
using MyHub.Services;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using MyHub.Services.Token;
using MyHub.Services.Authentication;
using MyHub.Services.Profile;
using MyHub.Services.FileStorage;
using MyHub.Services.Document;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ProfileManagerService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IFileStorage, LocalFileStorage>();
}
builder.Services.AddScoped<DocumentService>();


builder.Services
    .ConfigureSqlContext(builder.Configuration, builder.Environment)
    //.ConfigureIdentity()
    .ConfigureJWT(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await DatabaseSeeder.SeedAsync(app.Services);

app.Run();
