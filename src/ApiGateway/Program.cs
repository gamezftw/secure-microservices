using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

var authenticationProviderKey = "IdentityApiKey";

builder.Services.AddAuthentication()
  .AddJwtBearer(authenticationProviderKey, x =>
  {
      x.Authority = "http://localhost:5005";
      x.RequireHttpsMetadata = false;
      x.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateAudience = false
      };
  });

builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

await app.UseOcelot();

app.Run();
