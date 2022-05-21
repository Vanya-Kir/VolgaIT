using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolgaIT.Data;
using Microsoft.Extensions.Configuration;
using VolgaIT.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:80");
// Add services to the container.

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var server = builder.Configuration["DatabaseServer"] ?? "";
var port = builder.Configuration["DatabasePort"] ?? "";
var user = builder.Configuration["DatabaseUser"] ?? "";
var password = builder.Configuration["DatabasePassword"] ?? "";
var database = builder.Configuration["DatabaseName"] ?? "";

var connectionString = $"Server={server}; Port={port}; Database={database}; User Id={user}; Password={password}";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

var app = builder.Build();
// DatabaseManagmentService.MigrationInitialization(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
