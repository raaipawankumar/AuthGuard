using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(options =>
{
    options.NonceCookie.SameSite = SameSiteMode.Strict;
    options.CorrelationCookie.SameSite = SameSiteMode.Strict;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // Set the main OpenID Connect settings
    options.Authority = builder.Configuration.GetValue<string>("OpenIdConnect:Issuer");
   
    options.ClientId = builder.Configuration.GetValue<string>("OpenIdConnect:ClientId");
    options.ClientSecret = builder.Configuration.GetValue<string>("OpenIdConnect:ClientSecret");
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.ResponseMode = OpenIdConnectResponseMode.Query;
    string scopeString = builder.Configuration.GetValue<string>("OpenIDConnect:Scope") ?? string.Empty;
    // options.Scope.Clear();
    // scopeString.Split(" ", StringSplitOptions.TrimEntries).ToList().ForEach(scope =>
    // {
    //     options.Scope.Add(scope);
    // });
    // options.TokenValidationParameters = new TokenValidationParameters
    // {
    //     ValidIssuer = options.Authority,
    //     ValidAudience = options.ClientId
    // };

    // This example gets user information for display from the user info endpoint
   // options.GetClaimsFromUserInfoEndpoint = true;

    // Handle the post logout redirect URI
    options.Events.OnRedirectToIdentityProviderForSignOut = (context) =>
    {
        context.ProtocolMessage.PostLogoutRedirectUri = builder.Configuration.GetValue<string>("OpenIdConnect:PostLogoutRedirectUri");
        return Task.CompletedTask;
    };

    // Save tokens issued to encrypted cookies
    options.SaveTokens = true;

    // Set this in developer setups if the OpenID Provider uses plain HTTP
    options.RequireHttpsMetadata = false;

    /* Uncomment to debug HTTP requests from the web backend to the Identity Server
       Run a tool such as MITM proxy to view the request and response messages
    /*options.BackchannelHttpHandler = new HttpClientHandler()
    {
        Proxy = new WebProxy("http://127.0.0.1:8888"),
        UseProxy = true,
    };*/
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
IdentityModelEventSource.ShowPII = true;
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
