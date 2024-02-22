using Cinema.Web.Models;
using Cinema.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    IConfigurationRoot configuration = builder.Configuration;

    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
    options.UseLazyLoadingProxies();
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IShowService, ShowService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
