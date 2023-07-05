using ChatApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.ConfigureMySqlConnection(builder.Configuration);
builder.Services.ConfigureMSSQLConnection(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureServices();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
});

//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(opt =>
//{
//    opt.IdleTimeout = TimeSpan.FromMinutes(15);
//    opt.Cookie.Name = "sid";
//    opt.Cookie.HttpOnly = true;
//    opt.Cookie.IsEssential = true;
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
