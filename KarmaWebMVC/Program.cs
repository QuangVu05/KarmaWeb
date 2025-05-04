using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KarmaWebMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using KarmaWebMVC.Models;
using KarmaWebMVC.Repository;



var builder = WebApplication.CreateBuilder(args);
var cnn = builder.Configuration.GetConnectionString("KarmaWebMVCContext") ?? throw new InvalidOperationException("Connection string 'KarmaWebMVCContext' not found.");
//var dbHost = Environment.GetEnvironmentVariable("DatabaseServer") ?? "";
//var dbName = Environment.GetEnvironmentVariable("DatabaseName") ?? "";
//var dbPassword = Environment.GetEnvironmentVariable("DatabasePassword");

//var cnn = $"Server={dbHost},1433;Initial Catalog={dbName};User ID=SA;Password={dbPassword};Persist Security Info=False;Encrypt=False";


//builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<KarmaWebMVCContext>(options =>
    options.UseSqlServer(cnn));


//Add Email Sender
builder.Services.AddTransient<IEmailSender, EmailSender>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<AppUserModel,IdentityRole>()
    .AddEntityFrameworkStores<KarmaWebMVCContext>().AddDefaultTokenProviders();
//builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
   // options.Password.RequiredUniqueChars = 1;
   options.User.RequireUniqueEmail = true;
  
});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    })
    .AddGoogle(googleOptions =>
    {
        var googleClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
        var googleClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value;
        if (googleClientId != null && googleClientSecret != null)
        {
            googleOptions.ClientId = googleClientId;
            googleOptions.ClientSecret = googleClientSecret;
            googleOptions.Scope.Add("email"); // Ensure email is requested
            googleOptions.SaveTokens = true;
        }
    });



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
app.UseSession();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    );
});

// Route mặc định cho tất cả các controller không thuộc area "Admin"
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    


app.Run();
