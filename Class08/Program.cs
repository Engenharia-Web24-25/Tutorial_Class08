using Class08.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    //passsword default settings, we can change this to our needs
    options.Password.RequireDigit= true;
    options.Password.RequireLowercase= true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase= true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars= 1;

    // lockout settings
    options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(5);    
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // user settings
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvxzwyABCDEFGHIJKLMNOPQRSTUVXZWY0123456789-_@#";
    options.User.RequireUniqueEmail= true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<RoleManager<IdentityRole>>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    SeedRoles.Seed(roleManager);
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
