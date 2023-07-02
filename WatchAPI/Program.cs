using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol.Core.Types;
using WatchAPI.DAL;
using WatchAPI.Data;
using WatchAPI.Model;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WatchApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WatchApiContext") ?? throw new InvalidOperationException("Connection string 'WatchAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWatchRepository, WatchRepository>();
builder.Services.AddCors();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
