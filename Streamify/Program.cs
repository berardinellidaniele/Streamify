using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Streamify.Data;
using Streamify.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(opzioni => opzioni.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

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
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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