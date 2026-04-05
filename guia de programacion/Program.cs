using Microsoft.EntityFrameworkCore;
using spotify.Data;
using spotify.Models;
using spotify.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<AzureBlobService>();
// Add services to the container.
builder.Services.AddRazorPages();

// 1. AGREGAR EL SERVICIO DE SESIÓN (OBLIGATORIO)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configuración de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 2. ACTIVAR EL MIDDLEWARE DE SESIÓN (OBLIGATORIO)
// Debe ir después de UseRouting y ANTES de UseAuthorization
app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();