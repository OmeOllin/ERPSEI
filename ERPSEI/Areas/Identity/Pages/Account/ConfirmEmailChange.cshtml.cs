// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Localization;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Usuarios;

namespace ERPSEI.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStringLocalizer<ConfirmEmailChangeModel> _localizer;

        public ConfirmEmailChangeModel(
            AppUserManager userManager, 
            SignInManager<AppUser> signInManager,
            IStringLocalizer<ConfirmEmailChangeModel> stringLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = stringLocalizer;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{userId}'.");
            }

            string previousEmail = user.Email;

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer["EmailChangeFails"];
                return Page();
            }

            result = await _userManager.SetUserNameAsync(user, email);
            if (!result.Succeeded)
            {
                await _userManager.ChangeEmailAsync(user, previousEmail, code);
                StatusMessage = _localizer["EmailChangeFails"];
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["EmailChangeSuccessful"];
            return Page();
        }
    }
}
