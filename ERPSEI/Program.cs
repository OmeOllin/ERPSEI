using ERPSEI;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Usuarios;
using Microsoft.AspNetCore.Identity;
using ERPSEI.Email;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Email configuration
ServicesConfiguration.ConfigureEmail(builder);

//Database configuration
ServicesConfiguration.ConfigureDatabase(builder);

//Identity configuration
ServicesConfiguration.ConfigureIdentity(builder);

//Pages and localization configuration
ServicesConfiguration.ConfigurePagesAndLocalization(builder);

//Authorization configuration
ServicesConfiguration.ConfigureAuthorization(builder);

//Dependency injection configuration
ServicesConfiguration.ConfigureDependencyInjection(builder);

//Build and run application
WebApplication app = builder.Build();
using(IServiceScope scope = app.Services.CreateScope())
{
	//Authorization initialization
	//Se crea instancia del administrador de roles
	RoleManager<AppRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

	//Se inicializan los roles principales del sistema
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolMaster)) { await roleManager.CreateAsync(new AppRole(ServicesConfiguration.RolMaster)); }
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolAdministrador)) { await roleManager.CreateAsync(new AppRole(ServicesConfiguration.RolAdministrador)); }
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolUsuario)) { await roleManager.CreateAsync(new AppRole(ServicesConfiguration.RolUsuario)); }
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolCandidato)) { await roleManager.CreateAsync(new AppRole(ServicesConfiguration.RolCandidato)); }

	//Se crea instancia del administrador de accesos a los m�dulos
	IAccesoModuloManager accesoModuloManager = scope.ServiceProvider.GetRequiredService<IAccesoModuloManager>();
	List<AccesoModulo> accesos;
	List<AppRole> roles = roleManager.Roles.ToList();

	foreach (AppRole r in roles)
	{
		//Se obtienen los accesos del rol.
		accesos = await accesoModuloManager.GetByRolIdAsync(r.Id);

		//Si el rol no tiene ning�n acceso establecido, se crean sus accesos default.
		if (accesos.Count == 0)
		{
			IModuloManager moduloManager = scope.ServiceProvider.GetRequiredService<IModuloManager>();
			foreach (Modulo m in await moduloManager.GetAllAsync())
			{
				switch (r.Name)
				{
					case ServicesConfiguration.RolMaster:
						//Master tiene acceso completo a todos los m�dulos
						await accesoModuloManager.CreateAsync(new AccesoModulo() { RolId = r.Id, ModuloId = m.Id, PuedeTodo = 1, PuedeConsultar = 1, PuedeEditar = 1, PuedeEliminar = 1, PuedeAutorizar = 1 });
						break;
					case ServicesConfiguration.RolAdministrador:
						//Administrador solo puede consultar y editar en todos los m�dulos
						await accesoModuloManager.CreateAsync(new AccesoModulo() { RolId = r.Id, ModuloId = m.Id, PuedeTodo = 0, PuedeConsultar = 1, PuedeEditar = 1, PuedeEliminar = 0, PuedeAutorizar = 0 });
						break;
					case ServicesConfiguration.RolUsuario:
						//Usuario solo puede consultar en ciertos m�dulos
						switch (m.NombreNormalizado)
						{
							case "vacaciones":
							case "incapacidades":
							case "permisos":
							case "organigrama":
								await accesoModuloManager.CreateAsync(new AccesoModulo() { RolId = r.Id, ModuloId = m.Id, PuedeTodo = 0, PuedeConsultar = 1, PuedeEditar = 0, PuedeEliminar = 0, PuedeAutorizar = 0 });
								break;
							default:
								//Cualquier otro m�dulo est� bloqueado para el rol de usuario.
								break;
						}
						break;
					default:
						//Candidato y cualquier otro rol no considerado de inicio, no tienen acceso a ning�n m�dulo, por lo que no se definen accesos.
						break;
				}
			}
		}
	}

	//Se crea instancia del administrador de usuarios
	AppUserManager userManager = scope.ServiceProvider.GetRequiredService<AppUserManager>();

	//Si no existe un usuario master, entonces lo crea
	if (await userManager.Users.Where(u => u.IsMaster).FirstOrDefaultAsync() == null)
	{
		//Genera password para usuario master
		ServicesConfiguration.MasterPassword = userManager.GenerateRandomPassword(10);
		//Crea al usuario master.
		var result = await userManager.CreateAsync(ServicesConfiguration.MasterUser, ServicesConfiguration.MasterPassword);

		if (result.Succeeded)
		{
			//Asigna el rol de Master al usuario master.
			await userManager.AddToRoleAsync(ServicesConfiguration.MasterUser, ServicesConfiguration.RolMaster);

			//Env�a password por correo para notificarlo.
			IEmailSender emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
			emailSender.SendEmailAsync(ServicesConfiguration.MasterUser.Email ?? "", "Login Password", $"Use this password to login: {ServicesConfiguration.MasterPassword}");
		}
	}
}

app.UseSession();

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
