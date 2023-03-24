using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyAspNetCoreApp.Web.Filters;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));

});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x=>x.LoginPath="/Login/Index");
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
//builder.Services.AddSingleton<IHelper, Helper>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSession();
//builder.Services.AddScoped<Helper>(sp=> new Helper(""));// constructoruna parametre vermek için farklý bir kullaným þekli
builder.Services.AddScoped<NotFoundFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");//uygulamada bir hata fýrlatýldýðýnda home/error action metoduna yönlendirme yapýyor
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//blog/abc=> blog controller=> article action method çalýþsýn
//app.MapControllerRoute(
//    name: "pages",
//    pattern: "blog/{*article}",
//    defaults: new { controller = "Blog", Action = "Article" });

//app.MapControllerRoute(
//    name: "article",
//    pattern: "{controller=Blog}/{action=Article}/{name}/{id}");


//app.MapControllerRoute(
//    name: "pages",
//    pattern: "{controller}/{action}/{page}/{pagesize}");

//app.MapControllerRoute(
//    name: "GetById",
//    pattern: "{controller}/{action}/{productid}");
app.MapControllers();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//baseUrl/home/index
//baseur/home/privacy
app.Run();
