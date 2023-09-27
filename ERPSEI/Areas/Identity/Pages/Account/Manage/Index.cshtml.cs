// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using ERPSEI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IStringLocalizer<IndexModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            public IFormFile ProfilePicture {  get; set; }

            [Required(ErrorMessage = "Required")]
            [Display(Name = "UserNameField")]
            public string Username { get; set; }

            [Display(Name = "FirstNameField")]
            public string FirstName { get; set; }

            [Display(Name = "SecondNameField")]
            public string SecondName {  get; set; }

            [Display(Name = "FathersLastNameField")]
            public string FathersLastName {  get; set; }

            [Display(Name = "MothersLastNameField")]
            public string MothersLastName { get; set; }

            [Phone(ErrorMessage = "PhoneFormat")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "PhoneNumberField")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var firstName = await _userManager.GetFirstName
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userName = await _userManager.GetUserNameAsync(user);
            if (Input.Username != userName)
            {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if(!setUsernameResult.Succeeded) 
                {
                    //string details = String.Join(" - ", from error in setUsernameResult.Errors.ToList() select error.Description);
                    StatusMessage = _localizer["UserNameChangeFails"];
                    return RedirectToPage();
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = _localizer["UserPhoneChangeFails"];
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["UserProfileChangeSuccessful"];
            return RedirectToPage();
        }
    }
}
