using Microsoft.AspNetCore.Localization;
using System.Globalization;
using EJPro.Web.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql( //esse modulo informa que usamos postgres
        builder.Configuration.GetConnectionString("DefaultConnection"))); //E este busca a conexão guardada nos Segredos do Usuário
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var culturasSuportadas = new[]
    {
        new CultureInfo("pt-BR")
    };

    options.DefaultRequestCulture = new RequestCulture("pt-BR");
    options.SupportedCultures = culturasSuportadas;
    options.SupportedUICultures = culturasSuportadas;
});

var app = builder.Build();
app.UseRequestLocalization();

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
