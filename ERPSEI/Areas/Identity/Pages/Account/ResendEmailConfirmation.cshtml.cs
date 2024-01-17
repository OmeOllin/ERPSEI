// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ERPSEI.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.Extensions.Localization;
using ERPSEI.Data.Entities;

namespace ERPSEI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<ResendEmailConfirmationModel> _localizer;

        public ResendEmailConfirmationModel(
            AppUserManager userManager, 
            IEmailSender emailSender,
            IStringLocalizer<ResendEmailConfirmationModel> localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Required")]
            [EmailAddress(ErrorMessage = "EmailFormat")]
            [Display(Name = "EmailField")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ResendEmailConfirmation");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            _emailSender.SendEmailAsync(
                Input.Email,
                _localizer["EmailSubject"],
                $"{_localizer["EmailBodyFP"]} <a href='{callbackUrl}'> {_localizer["EmailBodySP"]}</a>.");

            return RedirectToPage("./ResendEmailConfirmationResult");
        }
    }
}
