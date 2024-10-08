using Check_List_API.Data;
using Checklist_API.Extensions;
using Checklist_API.Features.ExceptionHandling;
using Checklist_API.Features.JWT.Features;
using Checklist_API.Features.Users.Repository;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service;
using Checklist_API.Features.Users.Service.Interfaces;
using Checklist_API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterMappers();
builder.Services.AddServices(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }