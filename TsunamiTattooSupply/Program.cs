using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using TsunamiTattooSupply.Data; 

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for file logging
Log.Logger = new LoggerConfiguration()
	.WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
	.WriteTo.Console()
	.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container. 
builder.Services.AddControllersWithViews()
	.AddRazorRuntimeCompilation()
	.AddRazorOptions(options =>
	{
		options.ViewLocationFormats.Add("/Views/BackEnd/{1}/{0}.cshtml");
		options.ViewLocationFormats.Add("/Views/BackEnd/Shared/{0}.cshtml");
	});

// ===============================
// 🔐 AUTHENTICATION (THIS IS REQUIRED)
// ===============================
builder.Services.AddAuthentication(
	CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
	options.LoginPath = "/BackEnd/Index";     // 👈 LOGIN PAGE
	options.LogoutPath = "/BackEnd/LogOut";
	options.AccessDeniedPath = "/BackEnd/Index";
	options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

builder.Services.Configure<FormOptions>(options =>
{
	options.ValueCountLimit = 10000;     // increase max form keys
	options.ValueLengthLimit = int.MaxValue;
	options.MultipartBodyLengthLimit = 104857600; // optional (100MB)
});

// ✅ Add Session BEFORE builder.Build()
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // session lifetime
	options.Cookie.HttpOnly = true; // prevent JavaScript access
	options.Cookie.IsEssential = true; // required for GDPR compliance
});

builder.Services.AddDbContext<TsunamiDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TsunamiConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot (css, js, js, admin assets)

var imagesRoot = builder.Configuration["StaticFiles:ImagesRoot"]
	?? throw new InvalidOperationException(
		"StaticFiles:ImagesRoot is not configured.");

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(imagesRoot),
	RequestPath = ""
});
 
app.UseRouting();

// ✅ Use session AFTER UseRouting and BEFORE UseAuthorization
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BackEnd}/{action=Index}/{id?}");

app.Run();
