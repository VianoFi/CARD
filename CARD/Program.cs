using CARD.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurazione CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:7058", "https://localhost:7059") // Aggiungi anche HTTPS
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Se usi autenticazione/cookies
    });
});

// Add services to the container
builder.Services.AddControllersWithViews();

// Configurazione DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configura la pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// IMPORTANTE: L'ordine dei middleware è cruciale
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// CORS deve essere prima di Authorization
app.UseCors("MyCorsPolicy");
app.UseAuthorization();

// Mapping dei controller
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Database migration al startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Errore durante la migrazione del database");
    }
}

app.Run();