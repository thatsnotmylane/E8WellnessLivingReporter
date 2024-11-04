using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

var clientId = builder.Configuration.GetValue<string>("web:client_id");
var clientSecret = builder.Configuration.GetValue<string>("web:client_secret");

// Add services to the container.
builder.Services.AddRazorPages();

var services = builder.Services;

services.AddRazorPages();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = clientId ?? throw new Exception("ClientId not indicated");
    options.ClientSecret = clientSecret ?? throw new Exception("Client Secret not indicated");
    options.Scope.Add("https://www.googleapis.com/auth/gmail.send"); // Request the Gmail send scope
    options.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
