using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer()
  .AddInMemoryClients(Config.Clients)
  .AddInMemoryApiScopes(Config.ApiScopes)
  .AddInMemoryIdentityResources(Config.IdentityResources)
  .AddInMemoryApiResources(Config.ApiResources)
  .AddTestUsers(Config.TestUsers)
  .AddDeveloperSigningCredential();

var app = builder.Build();


app.UseStaticFiles();
app.UseRouting();

// app.UseHttpsRedirection();

// app.MapControllers();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.UseIdentityServer();

app.Run();
