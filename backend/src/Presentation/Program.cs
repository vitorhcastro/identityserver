using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Models;
using Presentation.Persistence;

const string allowLocalhostOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
{
    services.AddCors(
        options =>
        {
            options.AddPolicy(
                allowLocalhostOrigins,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                    corsPolicyBuilder.AllowAnyMethod();
                    corsPolicyBuilder.AllowAnyHeader();
                });
        });
    
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

    services.AddIdentityServer(options =>
        {
            options.UserInteraction.LoginUrl = "/Identity/Account/Login";
            options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";
            options.KeyManagement.Enabled = false; // Disable automatic key management
        })
        .AddDeveloperSigningCredential()
        .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            );
        })
        .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = optionsBuilder => optionsBuilder.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            );
        })
        .AddAspNetIdentity<ApplicationUser>();

    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:7196"; // IdentityServer URL
            options.ClientId = "mvc";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:7196";
            options.Audience = "identity-server-api";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiScope", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "identity-server-api");
        });
    });

    services.AddControllersWithViews();
    services.AddRazorPages();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();
{
    // Seed the database with initial data
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var env = serviceProvider.GetRequiredService<IHostEnvironment>();
        await IdentityServerSeeder.SeedAsync(serviceProvider, env);
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseRouting();
    
    app.UseCors(allowLocalhostOrigins);

    app.UseIdentityServer();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapRazorPages(); // Map Razor Pages for Identity UI
    app.MapControllers();
}

app.Run();
