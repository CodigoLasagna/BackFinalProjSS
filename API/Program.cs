using System.Text.Json.Serialization;
using Business.Contract;
using Business.Implementation;
using Data.Contract;
using Data.Implementation;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

/*remove when prod and fix to prod*/
services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder => corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .SetIsOriginAllowed((host) => true)
        .AllowAnyMethod());
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

/*load services*/
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserRepository, UserRepository>();

services.AddScoped<IPhishUserService, PhishUserService>();
services.AddScoped<IPhishUserRepository, PhishUserRepository>();

services.AddScoped<IRsaKeyService, RsaKeyService>();
services.AddScoped<IRsaKeyRepository, RsaKeyRepository>();


var app = builder.Build();
//maybe put cors here
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
