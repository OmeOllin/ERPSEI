// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using ERPSEI.Data.Entities;
using ERPSEI.Email;
using ERPSEI.Data.Managers;
using ERPSEI.Data;

namespace ERPSEI.Areas.Identity.Pages.Account
{
    public class AuthorizeUserModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly IEmpleadoManager _empleadoManager;
        private readonly IContactoEmergenciaManager _contactoEmergenciaManager;
        private readonly IArchivoEmpleadoManager _archivoEmpleadoManager;
        private readonly IStringLocalizer<AuthorizeUserModel> _localizer;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        public AuthorizeUserModel(
            AppUserManager userManager,
            IEmpleadoManager empleadoManager,
            IArchivoEmpleadoManager archivoEmpleadoManager,
            IContactoEmergenciaManager contactoEmergenciaManager,
            ApplicationDbContext db,
            IEmailSender emailSender,
            IStringLocalizer<AuthorizeUserModel> localization)
        {
            _userManager = userManager;
            _empleadoManager = empleadoManager;
            _archivoEmpleadoManager = archivoEmpleadoManager;
            _contactoEmergenciaManager = contactoEmergenciaManager;
            _localizer = localization;   
            _db = db;
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code, string actionId)
        {
            if (userId == null || code == null || actionId == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            bool result = await _userManager.VerifyUserTokenAsync(user, "UserAuthorization", "UserAuthorization", code);
            if(result)
            {
                if(actionId == "1")
                {
                    //AUTORIZADO
                    //Se envía notificación al correo del usuario para notificar que fue autorizado su perfil.
                    _emailSender.SendEmailAsync(user.Email,_localizer["EmailSubjectAuth"], $"{_localizer["EmailBodyFPAuth"]}");
                    //Se actualiza el usuario en base de datos
                    user.IsPreregisterAuthorized = true;
                    await _userManager.UpdateAsync(user);
                    return RedirectToPage("./ConfirmUserAuthorization");
                }
                else if(actionId == "2")
                {
                    //RECHAZADO
                    //Se envía notificación al correo del usuario para notificar que fue rechazado su perfil.
                    _emailSender.SendEmailAsync(user.Email, _localizer["EmailSubjectReject"], $"{_localizer["EmailBodyFPReject"]}");
                    await _db.Database.BeginTransactionAsync();
                    try
                    {
                        //Se eliminan los archivos del empleado
                        await _archivoEmpleadoManager.DeleteByEmpleadoIdAsync(user.EmpleadoId ?? 0);
                        //Se eliminan los contactos del empleado
                        await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(user.EmpleadoId ?? 0);
                        //Se elimina el usuario
                        await _userManager.DeleteAsync(user);
                        //Se elimina el empleado
                        await _empleadoManager.DeleteByIdAsync(user.EmpleadoId ?? 0);

                        await _db.Database.CommitTransactionAsync();

                        return RedirectToPage("./ConfirmUserRejection");
                    }
                    catch (Exception ex)
                    {
                        await _db.Database.RollbackTransactionAsync();
                        StatusMessage = $"Error: {ex.Message}";
                        return RedirectToPage();
                    }
                }
            }

            return RedirectToPage("./ConfirmUserAuthorizationFails");
        }
    }
}
