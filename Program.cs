using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using PPE.Data;
using PPE.Data.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration
           .GetConnectionString("DevelopmentConnection"))
            //.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
// AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<UserResolverService>();
//builder.Services.AddTransient<SharePointService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestLineSize = 20000; // Set as needed
    options.Limits.MaxRequestBufferSize = 20000; // Set as needed
    options.Limits.MaxRequestHeadersTotalSize = 20000; // Set as needed
});

// Create the database and apply migrations
/*using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    
    // Seed data during application startup
    DataSeeder.SeedData(dbContext);
}*/

builder.Services.AddScoped<IStokeService, StokeService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

RotativaConfiguration.Setup(app.Environment.WebRootPath, @"lib/rotativa-aspnetcore");

app.Run();