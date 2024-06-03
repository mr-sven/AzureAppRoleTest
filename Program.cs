using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// This is required to be instantiated before the OpenIdConnectOptions starts getting configured.
// By default, the claims mapping will map claim names in the old format to accommodate older SAML applications.
// 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role' instead of 'roles'
// This flag ensures that the ClaimsIdentity claims collection will be built from the claims in the token
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

// add Azure AD Authentication with Distributed cache
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
        .EnableTokenAcquisitionToCallDownstreamApi(["user.read"])
        .AddInMemoryTokenCaches();

// configure Open ID claims
builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters.RoleClaimType = "roles";
});

// Add services to the container.
builder.Services.AddRazorPages();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/StatusCode", "?statusCode={0}");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
