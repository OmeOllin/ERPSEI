using ERPSEI.Data;
using ERPSEI.Email;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Obtiene la configuración del enviador de correos.
IConfigurationSection emailSection = builder.Configuration.GetSection("Email");
string address = (string)(emailSection.GetValue(typeof(string), "address") ?? throw new InvalidOperationException("Email 'address' not found."));
string password = (string)(emailSection.GetValue(typeof(string), "password") ?? throw new InvalidOperationException("Email 'password' not found."));
string smtp = (string)(emailSection.GetValue(typeof(string), "smtp") ?? throw new InvalidOperationException("Email 'smtp' not found."));
int port = (int)(emailSection.GetValue(typeof(int), "port") ?? throw new InvalidOperationException("Email 'port' not found."));

builder.Services.AddTransient<IEmailSender, EmailSender>( x =>
    new EmailSender(address, password, smtp, port)
);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(ValidationsLocalization).GetTypeInfo().Assembly.FullName ?? "");
            return factory.Create(nameof(ValidationsLocalization), assemblyName.Name ?? "");
        };
    });
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("es-MX")
    };

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-MX");
    options.SupportedUICultures = supportedCultures;
    options.SupportedCultures = supportedCultures;
});

var app = builder.Build();

app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
