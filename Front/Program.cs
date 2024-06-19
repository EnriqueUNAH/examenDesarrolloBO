using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duration of the session
    options.Cookie.HttpOnly = true; // Session cookie should be accessible only by server-side code
    options.Cookie.IsEssential = true; // Mark the cookie as essential
});

// Add authentication services with cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Redirect to login page if unauthenticated
        options.LogoutPath = "/Home/Salir"; // Redirect to logout action when signing out
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration
        options.SlidingExpiration = true; // Renew the cookie each request if less than half the time remaining
    });

// Register IHttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Use session middleware
app.UseAuthentication(); // Use authentication middleware
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated && !context.Request.Path.StartsWithSegments("/Login"))
    {
        context.Response.Redirect("/Login");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); //home

app.Run();
