using ChatApp.Api.Data;
using ChatApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.ConfigureMySqlConnection(builder.Configuration);
builder.Services.AddDbContext<ChatAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureEfCoreIdentity();
builder.Services.ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
