using AuthGuard.Host;
using AuthGuard.Host.Extensions;
using AuthGuard.Host.Pages.Account;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddIdentityDependencies();
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddIdentityServerDependencies(builder.Configuration);
builder.Services.AddCookieAuthentication();
builder.Services.AddHostedService<AppBackgroundService>();

builder.Configuration.GetSection("IdentityServer").Bind(new ExternalProvider());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseRouting();

app.MapControllers();
app.MapRazorPages();


app.Run();
