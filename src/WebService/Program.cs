using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDatabase();
builder.Services.AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.ConfigureLogging(logging =>
{
    logging.ConfigureLogging(builder.Configuration);
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Services.CheckDbInit();
app.AddCallLog();
app.MapControllers();

void RedirectToSwaggerEe(HttpContext context)
{
    context.Response.Redirect("/swagger/index.html");
}
app.Map("/", RedirectToSwaggerEe);
app.Run();

public partial class Program { }
