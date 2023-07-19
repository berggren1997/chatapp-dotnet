using ChatApp.Api.Extensions;
using ChatApp.Api.Hubs;
using ChatApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

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

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(opt =>
//{
//    opt.IdleTimeout = TimeSpan.FromMinutes(15);
//    opt.Cookie.Name = "sid";
//    opt.Cookie.HttpOnly = true;
//    opt.Cookie.IsEssential = true;
//});

var app = builder.Build();

app.ConfigureExceptionMiddlewareHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");
//app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MessageHub>("/messageHub");
app.MapHub<ConversationHub>("/conversationHub");
app.MapHub<CallsHub>("/callsHub");

app.MapControllers();

app.Run();
