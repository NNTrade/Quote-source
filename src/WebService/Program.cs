using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebService;
using WebService.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDatabase();
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Quote source service API",
        Description = "Service for getting and caching quote data",
        Contact = new OpenApiContact
        {
            Name = "InsonusK",
            Url = new Uri("https://github.com/InsonusK")
        }
    });
});
builder.ConfigLogging();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Services.CheckDbInit();
app.AddCallLog();
app.MapControllers();

app.AddRedirectToSwagger();
app.Run();

public partial class Program { }
