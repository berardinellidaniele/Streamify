using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streamify.Models;
using Streamify.Data;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<Utente, IdentityRole>(opzioni =>
{
    opzioni.Password.RequireNonAlphanumeric = false;
    opzioni.Password.RequiredLength = 8;
    opzioni.Password.RequireUppercase = false;
    opzioni.Password.RequireLowercase = false;
    opzioni.User.RequireUniqueEmail = true;
    opzioni.SignIn.RequireConfirmedAccount = false;
    opzioni.SignIn.RequireConfirmedEmail = false;
    opzioni.SignIn.RequireConfirmedPhoneNumber = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    return new SqlConnection(connectionString);
});

builder.Services.AddSingleton<Database>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
