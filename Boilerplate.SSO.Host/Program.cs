using Boilerplate.SSO.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddRazorPages();
builder.Services.AddApplicationIdentityServer(configuration);
//builder.Services.AddApplicationAuthentication(configuration);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
    // app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.MapRazorPages();



app.Run();
