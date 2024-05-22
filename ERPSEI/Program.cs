using ERPSEI;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Usuarios;
using ERPSEI.Email;

var builder = WebApplication.CreateBuilder(args);

//Email configuration
ServicesConfiguration.ConfigureEmail(builder);

//Database configuration
ServicesConfiguration.ConfigureDatabase(builder);

//Identity configuration
ServicesConfiguration.ConfigureIdentity(builder);

//Pages and localization configuration
ServicesConfiguration.ConfigurePagesAndLocalization(builder);

//Build and run application
WebApplication app = builder.Build();

using(IServiceScope scope = app.Services.CreateScope())
{
    //Se inicializan los roles
    AppRoleManager roleManager = scope.ServiceProvider.GetRequiredService<AppRoleManager>();

    //Este rol debe existir siempre para poder tener acceso completo al sistema.
    if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolMaster)) { await roleManager.CreateAsync(new AppRole() { Name = ServicesConfiguration.RolMaster }); }
	//Este rol debe existir siempre para poder ascender a los candidatos a usuarios con los privilegios más básicos.
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolUsuario)) { await roleManager.CreateAsync(new AppRole() { Name = ServicesConfiguration.RolUsuario }); }
	//Este rol debe existir siempre para poder admitir registro de empleados candidatos.
	if (!await roleManager.RoleExistsAsync(ServicesConfiguration.RolCandidato)) { await roleManager.CreateAsync(new AppRole() { Name = ServicesConfiguration.RolCandidato }); }

	ServicesConfiguration.Roles = roleManager.Roles.ToList();

    //Se inicializa el usuario master
    AppUserManager userManager = scope.ServiceProvider.GetRequiredService<AppUserManager>();

    if (await userManager.FindByEmailAsync(ServicesConfiguration.MasterUser.Email ?? "") == null)
    {
        //Genera password para usuario master
        ServicesConfiguration.MasterPassword = userManager.GenerateRandomPassword(10);
        //Crea al usuario master.
        var result = await userManager.CreateAsync(ServicesConfiguration.MasterUser, ServicesConfiguration.MasterPassword);

        if (result.Succeeded)
        {
            //Asigna el rol de Master al usuario master.
            await userManager.AddToRoleAsync(ServicesConfiguration.MasterUser, ServicesConfiguration.RolMaster);

            //Envía password por correo para notificarlo.
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
