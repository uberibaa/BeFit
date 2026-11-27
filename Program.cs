using System.Globalization;
using BeFit.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount =
        !builder.Environment.IsDevelopment();
})
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<PolishIdentityErrors>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (value, field) => $"Wartość „{value}” jest nieprawidłowa dla pola „{field}”.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
        field => $"Pole „{field}” musi być liczbą.");
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
});

var app = builder.Build();

// HTML number inputs submit decimals with a dot. Preserve that format on all PCs.
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US", "pl-PL"),
    SupportedCultures = new[] { CultureInfo.GetCultureInfo("en-US") },
    SupportedUICultures = new[] { CultureInfo.GetCultureInfo("pl-PL") },
    RequestCultureProviders = new List<Microsoft.AspNetCore.Localization.IRequestCultureProvider>()
});

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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

if (app.Environment.IsDevelopment())
{
    await IdentitySeeder.SeedAsync(
        app.Services, app.Configuration);
}

app.Run();
