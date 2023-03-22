using AspNetCoreVaultIntegration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Convey.Secrets.Vault;
using Microsoft.Extensions.Options;
using Convey;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddVault(options =>
{
    var vaultOptions = builder.Configuration.GetSection("Vault");
    options.Address = vaultOptions["Address"];
    options.Token = vaultOptions["Token"];
    options.MountPath = vaultOptions["MountPath"];
    options.SecretPath = vaultOptions["SecretPath"];
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var section = builder.Configuration.GetSection("SomeSettings");
builder.Services.AddOptions<SomeSettings>().Bind(section);
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
