using Microsoft.OpenApi.Models;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using TheDailyRoutine.Infrastructure.Data;
using TheDailyRoutine.Infrastructure.Data.Models;
using TheDailyRoutine.Infrastructure.Extensions;
using TheDailyRoutine.Core.Extensions;
using TheDailyRoutine.Core.Services;
using Microsoft.EntityFrameworkCore;
using TheDailyRoutine.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddApplicationDbContext(builder.Configuration);

// Configure Identity with ApplicationUser
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add RazorPages for Identity UI
builder.Services.AddRazorPages();

// Add infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add Hangfire
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// Add application services
builder.Services.AddApplicationServices();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TheDailyRoutine API",
        Version = "v1",
        Description = "API for TheDailyRoutine habit tracking application"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173") // Default Vite port
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheDailyRoutine API V1");
        c.RoutePrefix = "api-docs";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

/*app.UseHttpsRedirection();
*/app.UseStaticFiles();

app.UseRouting();

app.UseCors();
// Configure Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Daily Routine Jobs",
    IsReadOnlyFunc = context => app.Environment.IsProduction()
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure recurring jobs
RecurringJob.AddOrUpdate<NotificationJobService>(
    "daily-reminders",
    service => service.SendDailyReminders(),
    "0 20 * * *");

RecurringJob.AddOrUpdate<NotificationJobService>(
    "weekly-summaries",
    service => service.SendWeeklySummaries(),
    "0 18 * * 0");

RecurringJob.AddOrUpdate<NotificationJobService>(
    "process-notifications",
    service => service.ProcessPendingNotifications(),
    "*/5 * * * *");

// Map endpoints

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();