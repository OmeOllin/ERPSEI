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

namespace ERPSEI.Areas.Identity.Pages.Account
{
    public class AuthorizeUserModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly IStringLocalizer<AuthorizeUserModel> _localizer;
        private readonly IEmailSender _emailSender;

        public AuthorizeUserModel(
            AppUserManager userManager, 
            IEmailSender emailSender,
            IStringLocalizer<AuthorizeUserModel> localization)
        {
            _userManager = userManager;
            _localizer = localization;   
            _emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
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
                //Se envía notificación al correo del usuario para notificar que fue autorizado su perfil.
                _emailSender.SendEmailAsync(user.Email,_localizer["EmailSubject"], $"{_localizer["EmailBodyFP"]}");

                return RedirectToPage("./ConfirmUserAuthorization");
            }
            else
            {
                return RedirectToPage("./ConfirmUserAuthorizationFails");
            }
        }
    }
}
