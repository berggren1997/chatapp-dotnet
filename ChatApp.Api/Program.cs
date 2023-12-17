using ChatApp.Api.Data;
using ChatApp.Api.Extensions;
using ChatApp.Api.Hubs;
using ChatApp.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment}.json")
    .AddEnvironmentVariables()
    .Build();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.ConfigureMySqlConnection(builder.Configuration);
builder.Services.ConfigureMSSQLConnection(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServices();
builder.Services.ConfigureCorsPolicy();
builder.Services.ConfigureCookieOptions();
builder.Services.AddSignalR();

var app = builder.Build();

using var scope = app.Services.CreateScope();
using var dbContext = scope.ServiceProvider.GetRequiredService<ChatAppContext>();
dbContext.Database.Migrate();

app.ConfigureExceptionMiddlewareHandler();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


//app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");
//app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MessageHub>("/messageHub");
app.MapHub<ConversationHub>("/conversationHub");
app.MapHub<CallsHub>("/callsHub");

app.MapControllers();

app.Run();
