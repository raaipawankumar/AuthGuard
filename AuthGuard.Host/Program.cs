using AuthGuard.Host;
using AuthGuard.Host.Extensions;
using AuthGuard.Host.Pages.Account;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services
.AddDatabaseContext(builder.Configuration)
.AddIdentityDependencies()
.AddIdentityServerDependencies(builder.Configuration)
.AddCookieAuthentication()
.AddHostedService<AppBackgroundService>();

builder.Configuration.GetSection("IdentityServer").Bind(new ExternalProvider());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
}

app
.UseStaticFiles()
.UseRouting()
.UseAuthentication()
.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
