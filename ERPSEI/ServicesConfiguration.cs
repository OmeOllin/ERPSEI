﻿using ERPSEI.Authorization;
using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Data.Managers.Reportes;
using ERPSEI.Data.Managers.SAT;
using ERPSEI.Data.Managers.Usuarios;
using ERPSEI.Email;
using ERPSEI.Resources;
using ERPSEI.TokenProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

namespace ERPSEI
{
	public static class ServicesConfiguration
    {
        public const string RolMaster = "Master";
        public const string RolAdministrador = "Administrador";
        public const string RolUsuario = "Usuario";
        public const string RolCandidato = "Candidato";

        public static List<AppRole> Roles = new List<AppRole>();

        public static string MasterPassword { get; set; } = string.Empty;
        public static AppUser MasterUser { get; } = new AppUser() { EmailConfirmed = true, IsPreregisterAuthorized = true, PasswordResetNeeded = false, IsMaster = true };

        public static void ConfigureEmail(WebApplicationBuilder _builder)
        {
            //Obtiene la configuración del enviador de correos.
            IConfigurationSection emailSection = _builder.Configuration.GetSection("Email");
            string address = (string)(emailSection.GetValue(typeof(string), "address") ?? throw new InvalidOperationException("Email 'address' not found."));
            string password = (string)(emailSection.GetValue(typeof(string), "password") ?? throw new InvalidOperationException("Email 'password' not found."));
            string smtp = (string)(emailSection.GetValue(typeof(string), "smtp") ?? throw new InvalidOperationException("Email 'smtp' not found."));
            int port = (int)(emailSection.GetValue(typeof(int), "port") ?? throw new InvalidOperationException("Email 'port' not found."));

            MasterUser.Email = address;
            MasterUser.UserName = address;

            _builder.Services.AddTransient<IEmailSender, EmailSender>(x =>
                new EmailSender(address, password, smtp, port)
            );
        }

        public static void ConfigureDatabase(WebApplicationBuilder _builder)
        {
            var connectionString = _builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            _builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			_builder.Services.AddScoped<IAccesoModuloManager, AccesoModuloManager>();
			_builder.Services.AddScoped<AppRoleManager, AppRoleManager>();
            _builder.Services.AddScoped<IModuloManager, ModuloManager>();

			_builder.Services.AddScoped<IAsistenciaManager, AsistenciaManager>();

			_builder.Services.AddScoped<IConceptoManager, ConceptoManager>();
            _builder.Services.AddScoped<IPrefacturaManager, PrefacturaManager>();
            _builder.Services.AddScoped<IExportacionManager, ExportacionManager>();
			_builder.Services.AddScoped<IFormaPagoManager, FormaPagoManager>();
			_builder.Services.AddScoped<IImpuestoManager, ImpuestoManager>();
			_builder.Services.AddScoped<IMesManager, MesManager>();
			_builder.Services.AddScoped<IMetodoPagoManager, MetodoPagoManager>();
            _builder.Services.AddScoped<IMonedaManager, MonedaManager>();
			_builder.Services.AddScoped<IObjetoImpuestoManager, ObjetoImpuestoManager>();
			_builder.Services.AddScoped<IPeriodicidadManager, PeriodicidadManager>();
			_builder.Services.AddScoped<IRegimenFiscalManager, RegimenFiscalManager>();
			_builder.Services.AddScoped<ITasaOCuotaManager, TasaOCuotaManager>();
			_builder.Services.AddScoped<ITipoComprobanteManager, TipoComprobanteManager>();
			_builder.Services.AddScoped<ITipoFactorManager, TipoFactorManager>();
			_builder.Services.AddScoped<ITipoRelacionManager, TipoRelacionManager>();
			_builder.Services.AddScoped<IUnidadMedidaManager, UnidadMedidaManager>();
			_builder.Services.AddScoped<IUsoCFDIManager, UsoCFDIManager>();
			_builder.Services.AddScoped<IProductoServicioManager, ProductoServicioManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<ActividadEconomica>, ActividadEconomicaManager>();

			_builder.Services.AddScoped<IBancoEmpresaManager, BancoEmpresaManager>();
			_builder.Services.AddScoped<IArchivoEmpresaManager, ArchivoEmpresaManager>();
			_builder.Services.AddScoped<IEmpresaManager, EmpresaManager>();
            _builder.Services.AddScoped<IProductoServicioPerfilManager, ProductoServicioPerfilManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Perfil>, PerfilManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Origen>, OrigenManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Nivel>, NivelManager>();
			_builder.Services.AddScoped<IActividadEconomicaEmpresaManager, ActividadEconomicaEmpresaManager>();

			_builder.Services.AddScoped<IArchivoEmpleadoManager, ArchivoEmpleadoManager>();
			_builder.Services.AddScoped<IContactoEmergenciaManager, ContactoEmergenciaManager>();

			_builder.Services.AddScoped<IEmpleadoManager, EmpleadoManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Puesto>, PuestoManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Area>, AreaManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Oficina>, OficinaManager>();
			_builder.Services.AddScoped<IRWCatalogoManager<Subarea>, SubareaManager>();

			_builder.Services.AddScoped<IRCatalogoManager<Genero>, GeneroManager>();
			_builder.Services.AddScoped<IRCatalogoManager<EstadoCivil>, EstadoCivilManager>();

			//_builder.Services.AddScoped<IRWCatalogoManager<Asistencias>, AsistenciasManager>();
		}

		public static void ConfigureIdentity(WebApplicationBuilder _builder)
        {
            _builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<AppRole>()
            .AddRoleManager<AppRoleManager>()
            .AddUserManager<AppUserManager>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<UserAuthorizationTokenProvider<AppUser>>("UserAuthorization");
        }

        public static void ConfigureAuthorization(WebApplicationBuilder _builder) {
			_builder.Services.AddAuthorization(options =>
	            options.AddPolicy("AccessPolicy", policy => policy.Requirements.Add(new AccessRequirement()))
            );

			_builder.Services.AddScoped<IAuthorizationHandler, AccessHandler>();
		}

        public static void ConfigurePagesAndLocalization(WebApplicationBuilder _builder)
        {
            _builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
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

            _builder.Services.AddSession();
            _builder.Services.AddMemoryCache();
            _builder.Services.AddMvc(options =>
            {
                var assemblyName = new AssemblyName(typeof(ModelBindingMessages).GetTypeInfo().Assembly.FullName ?? "");
                var F = _builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var L = F.Create(nameof(ModelBindingMessages), assemblyName.Name ?? "");

                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) => L["MissingBindRequiredValueAccessor", x]);
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => L["MissingKeyOrValueAccessor"]);
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => L["MissingRequestBodyRequiredValueAccessor"]);
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => L["ValueMustNotBeNullAccessor", x]);

                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => L["UnknownValueIsInvalidAccessor", x]);
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => L["NonPropertyUnknownValueIsInvalidAccessor"]);
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => L["ValueIsInvalidAccessor", x]);

                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => L["ValueMustBeANumberAccessor", x]);
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => L["NonPropertyValueMustBeANumberAccessor"]);

                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => L["AttemptedValueIsInvalidAccessor", x, y]);
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => L["NonPropertyAttemptedValueIsInvalidAccessor", x]);
            });

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
