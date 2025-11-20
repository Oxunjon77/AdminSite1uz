using BusinessLayer;
using BusinessLayer.Maneger;
using DataAsseccLayer.Concreat;
using DataAsseccLayer.EntityFramework;
using DataAsseccLayer.Repostory;
using DataAsseccLayer.Repostory.Interfase;
using DataAsseccLayer.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataAsseccLayer.Concreat.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryDl, CategoryRepostory>();
builder.Services.AddScoped<IAboutDl, EfAboutDl>();
builder.Services.AddScoped<AboutManager>(); 

builder.Services.AddScoped<ICategoryDl, EfCategoryDl>();
builder.Services.AddScoped<CategoryManager>();


Startup(builder.Services, builder.Configuration);
builder.Services.AddHostedService<NewsSchedulerService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1600); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery();

var app = builder.Build();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();



app.UseHttpsRedirection();


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();

static void Startup(IServiceCollection services, ConfigurationManager manager)
{
    services.AddControllersWithViews();
    services.AddRazorPages();

    services.AddAuthentication(

        CookieAuthenticationDefaults.AuthenticationScheme).
        AddCookie(options =>
        {
            options.LogoutPath = "/Login/Index";
        });


    services.AddHttpContextAccessor();


}