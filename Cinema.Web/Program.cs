using Cinema.Data.Models;
using Cinema.Data.Models.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaDbContext>(options =>
{
    IConfigurationRoot configuration = builder.Configuration;

    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
    options.UseLazyLoadingProxies();
});

builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<CinemaDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Movies}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())
using (var context = serviceScope.ServiceProvider.GetRequiredService<CinemaDbContext>())
{
    String? imageSource = app.Configuration.GetValue<String>("ImageSource");
    if (imageSource == null)
        throw new ArgumentNullException(nameof(imageSource));
    await DbInitializer.InitializeAsync(context, imageSource);
}

app.Run();
