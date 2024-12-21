global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using TheDailyRoutine.Infrastructure.Data;
global using TheDailyRoutine.Infrastructure.Data.Interfaces;
global using TheDailyRoutine.Infrastructure.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServices();


// Configure the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true; // Require email confirmation for login
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Configure email sender service (for email confirmation)
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add controllers with views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enables authentication
app.UseAuthorization();  // Enables authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (if you're using Identity default pages, such as `/Account/Manage`)
app.MapRazorPages();

app.Run();


