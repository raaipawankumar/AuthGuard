using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
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
GetHash();
string GetHash(){
using (var sha256 = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes("secret");
            var hash = sha256.ComputeHash(bytes);
            var c = Convert.ToBase64String(hash);
            Debug.WriteLine(c);
            return c;
        }
}


app.Run();
