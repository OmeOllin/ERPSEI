using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Email;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

namespace ERPSEI
{
    public static class ServicesConfiguration
    {
        public static void ConfigureEmail(WebApplicationBuilder _builder)
        {
            //Obtiene la configuración del enviador de correos.
            IConfigurationSection emailSection = _builder.Configuration.GetSection("Email");
            string address = (string)(emailSection.GetValue(typeof(string), "address") ?? throw new InvalidOperationException("Email 'address' not found."));
            string password = (string)(emailSection.GetValue(typeof(string), "password") ?? throw new InvalidOperationException("Email 'password' not found."));
            string smtp = (string)(emailSection.GetValue(typeof(string), "smtp") ?? throw new InvalidOperationException("Email 'smtp' not found."));
            int port = (int)(emailSection.GetValue(typeof(int), "port") ?? throw new InvalidOperationException("Email 'port' not found."));

            _builder.Services.AddTransient<IEmailSender, EmailSender>(x =>
                new EmailSender(address, password, smtp, port)
            );
        }

        public static void ConfigureDatabase(WebApplicationBuilder _builder)
        {
            var connectionString = _builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            _builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            _builder.Services.AddScoped<IUserFileManager, UserFileManager>();

        }

        public static void ConfigureIdentity(WebApplicationBuilder _builder)
        {
            _builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddUserManager<AppUserManager>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public static void ConfigurePagesAndLocalization(WebApplicationBuilder _builder)
        {
            _builder.Services.AddRazorPages()
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(ValidationsLocalization).GetTypeInfo().Assembly.FullName ?? "");
                    return factory.Create(nameof(ValidationsLocalization), assemblyName.Name ?? "");
                };
            });
            _builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            _builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("es-MX")
                };

                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("es-MX");
                options.SupportedUICultures = supportedCultures;
                options.SupportedCultures = supportedCultures;
            });
        }
    }
}
