using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMovieApiService, MovieApiService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
      {
          options.Authority = "http://localhost:5005";
          options.RequireHttpsMetadata = false;

          options.ClientId = "movies_mvc_client";
          options.ClientSecret = "secret";
          options.ResponseType = "code";

          options.Scope.Add("openid");
          options.Scope.Add("profile");

          options.SaveTokens = true;

          options.GetClaimsFromUserInfoEndpoint = true;
      });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<AuthenticationDelegatingHandler>();
builder.Services.AddHttpClient("MovieAPIClient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5001/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    }).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("IDPClient", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5005/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    });

builder.Services.AddSingleton(new ClientCredentialsTokenRequest
{
    Address = "http://localhost:5005/connect/token",
    ClientId = "movieClient",
    ClientSecret = "secret",
    Scope = "movieAPI"
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
