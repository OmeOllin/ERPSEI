using ERPSEI;
using ERPSEI.Data.Entities;
using ERPSEI.Email;
using Microsoft.AspNetCore.Identity;

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
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync(ServicesConfiguration.Master)) { await roleManager.CreateAsync(new IdentityRole(ServicesConfiguration.Master)); }
    if (!await roleManager.RoleExistsAsync(ServicesConfiguration.Administrador)) { await roleManager.CreateAsync(new IdentityRole(ServicesConfiguration.Administrador)); }
    if (!await roleManager.RoleExistsAsync(ServicesConfiguration.Usuario)) { await roleManager.CreateAsync(new IdentityRole(ServicesConfiguration.Usuario)); }
    if (!await roleManager.RoleExistsAsync(ServicesConfiguration.Candidato)) { await roleManager.CreateAsync(new IdentityRole(ServicesConfiguration.Candidato)); }

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
            await userManager.AddToRoleAsync(ServicesConfiguration.MasterUser, ServicesConfiguration.Master);

            //Env�a password por correo para notificarlo.
            IEmailSender emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            await emailSender.SendEmailAsync(ServicesConfiguration.MasterUser.Email ?? "", "Login Password", $"Use this password to login: {ServicesConfiguration.MasterPassword}");
        }
    }
}

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
