using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol.Core.Types;
using WatchAPI.DAL;
using WatchAPI.Data;
using WatchAPI.Model;
using Azure.Identity;
using System.Security.Policy;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration.GetSection("KeyVaultURL");


builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value!.ToString(), new DefaultKeyVaultSecretManager());
var client = new SecretClient(new Uri(keyVaultUrl.Value!.ToString()), new DefaultAzureCredential());

builder.Services.AddDbContext<WatchApiContext>(options =>
{
    options.UseSqlServer(client.GetSecret("secretConnection").Value.Value.ToString());
});


builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(client.GetSecret("secretStorageKey").Value.Value);
});


builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["secretStorageKey:blob"]);
    clientBuilder.AddQueueServiceClient(builder.Configuration["secretStorageKey:queue"]);
});




// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWatchRepository, WatchRepository>();
builder.Services.AddScoped<ICarouselRepository, CarouselRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
