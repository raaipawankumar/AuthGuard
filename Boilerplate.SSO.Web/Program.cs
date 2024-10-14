using Boilerplate.SSO.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddRazorPages();
builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(configuration.GetSection("IdentityServer.IdentityResources"))
    .AddInMemoryClients(configuration.GetSection("IdentityServer.Clients"))
    .AddInMemoryApiScopes(configuration.GetSection("IdentityServer.ApiScopes"))
    .AddInMemoryApiResources(configuration.GetSection("IdentityServer.ApiResources"))
    .AddTestUsers(IdentityServerSettings.TestUsers)
    .AddDeveloperSigningCredential();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
    // app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseIdentityServer();

app.Run();
