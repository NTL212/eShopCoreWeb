
using eShopCoreWeb.Application.Catalog.Products;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using eShopCoreWeb.Data.EF;
using Microsoft.EntityFrameworkCore;
using eShopCoreWeb.Application.Common;
using eShopCoreWeb.ApiIntegration;
using Microsoft.AspNetCore.Authentication.Cookies;
using eShopCoreWeb.WebApp.LocalizationResources;
using System.Globalization;
using FluentValidation.AspNetCore;
using eShopCoreWeb.ViewModels.System.Users;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
var cultures = new[]
          {
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => {
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        
    })
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
    {
        // When using all the culture providers, the localization process will
        // check all available culture providers in order to detect the request culture.
        // If the request culture is found it will stop checking and do localization accordingly.
        // If the request culture is not found it will check the next provider by order.
        // If no culture is detected the default culture will be used.

        // Checking order for request culture:
        // 1) RouteSegmentCultureProvider
        //      e.g. http://localhost:1234/tr
        // 2) QueryStringCultureProvider
        //      e.g. http://localhost:1234/?culture=tr
        // 3) CookieCultureProvider
        //      Determines the culture information for a request via the value of a cookie.
        // 4) AcceptedLanguageHeaderRequestCultureProvider
        //      Determines the culture information for a request via the value of the Accept-Language header.
        //      See the browsers language settings

        // Uncomment and set to true to use only route culture provider
        ops.UseAllCultureProviders = false;
        ops.ResourcesPath = "LocalizationResources";
        ops.RequestLocalizationOptions = o =>
        {
            o.SupportedCultures = cultures;
            o.SupportedUICultures = cultures;
            o.DefaultRequestCulture = new RequestCulture("vi");
        };
    });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(options =>
   {
       options.Cookie.Name = "WebAppCookie";
       options.LoginPath = "/Account/Index/";
       options.AccessDeniedPath = "/Users/Forbidden/";
   })
   .AddGoogle(options =>
   {
       options.ClientId = "103741414803-n3t39vqim5c198iep7flgi7miqd7kaea.apps.googleusercontent.com";
       options.ClientSecret = "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";
   })
   .AddFacebook(options =>
   {
       options.ClientId = "1327390431201676";
       options.ClientSecret = "c7cde5faa4470a395e7720eb74ddd8b3";
       options.CallbackPath = "/dang-nhap-tu-facebook";
   });

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "WebAppSession";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IManageProductService, ManageProductService>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();
IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

var connectionString = configuration.GetConnectionString("eShopCoreWebDb");

builder.Services.AddDbContext<EShopDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.UseRequestLocalization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
});
app.Run();
