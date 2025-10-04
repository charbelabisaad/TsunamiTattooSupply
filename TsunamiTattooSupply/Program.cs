﻿using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // <-- Add this;

builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/Views/BackEnd/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/BackEnd/Shared/{0}.cshtml");
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
app.UseStaticFiles();

app.UseRouting();

// ✅ Use session AFTER UseRouting and BEFORE UseAuthorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=BackEnd}/{action=Index}/{id?}");

app.Run();
