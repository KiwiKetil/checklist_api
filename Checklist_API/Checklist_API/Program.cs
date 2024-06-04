using Check_List_API.Data;
<<<<<<< HEAD
using Check_List_API.Middleware;
=======
>>>>>>> 408ee054196819f8f9a9a1d3937ddd8eb909228e
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<< HEAD
builder.Services.AddScoped<GlobalException>();

=======
>>>>>>> 408ee054196819f8f9a9a1d3937ddd8eb909228e
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContext<CheckListDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0)))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
<<<<<<< HEAD
=======

>>>>>>> 408ee054196819f8f9a9a1d3937ddd8eb909228e
app.UseHttpsRedirection();

app.UseMiddleware<GlobalException>();

app.UseAuthorization();

app.MapControllers();

app.Run();
