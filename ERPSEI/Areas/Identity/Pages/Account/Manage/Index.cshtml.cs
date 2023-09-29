// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using ERPSEI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(
            AppUserManager userManager,
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

        public string ProfilePictureSrc { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public IFormFile ProfilePicture {  get; set; }

            public IFormFile FirstDocument { get; set; }

            public IFormFile SecondDocument { get; set; }

            [Display(Name = "UserNameField")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(@"^[A-ZÁÉÍÓÚ][a-záéíóú]+$", ErrorMessage = "PersonName")]
            [Display(Name = "FirstNameField")]
            public string FirstName { get; set; }

            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(@"^[A-ZÁÉÍÓÚ][a-záéíóú]+$", ErrorMessage = "PersonName")]
            [Display(Name = "SecondNameField")]
            public string SecondName {  get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(@"^[A-ZÁÉÍÓÚ][a-záéíóú]+$", ErrorMessage = "PersonName")]
            [Display(Name = "FathersLastNameField")]
            public string FathersLastName {  get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(@"^[A-ZÁÉÍÓÚ][a-záéíóú]+$", ErrorMessage = "PersonName")]
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
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Input = new InputModel
            {
                Username = userName,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                FathersLastName = user.FathersLastName,
                MothersLastName = user.MothersLastName,
                PhoneNumber = phoneNumber
            };

            //Si el usuario tiene imagen de perfil
            if(user.ProfilePicture != null && user.ProfilePicture.Length >= 1) { 
                //Se usa para mostrarla
                ProfilePictureSrc = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}"; 
            }
            else
            {
                //De lo contrario, se usa la imagen default.
                ProfilePictureSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.jpg";
            }
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

            user.FirstName = Input.FirstName;
            user.SecondName = Input.SecondName ?? "";
            user.FathersLastName = Input.FathersLastName;
            user.MothersLastName = Input.MothersLastName;
            user.PhoneNumber = Input.PhoneNumber ?? "";

            //Si el usuario estableció una imagen de perfil
            if (Input.ProfilePicture != null && Input.ProfilePicture.Length >= 1)
            {
                //Transforma la imagen a memoryStream
                using (var memoryStream = new MemoryStream())
                {
                    await Input.ProfilePicture.CopyToAsync(memoryStream);
                    int maxSizeInBytes = 1000000;
                    //Verifica que el tamaño máximo no exceda el megabyte
                    if (memoryStream.Length < maxSizeInBytes)
                    {
                        //Se guarda el arreglo de bytes de la imagen.
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                    else
                    {
                        //Se notifica error del tamaño máximo de archivo.
                        StatusMessage = $"{_localizer["ProfilePictureTooLarge"]} {maxSizeInBytes / 1000000} Mb";
                        return RedirectToPage();
                    }
                }
            }

            //Se actualiza el usuario.
            var setResult = await _userManager.UpdateAsync(user);
            if(!setResult.Succeeded)
            {
                StatusMessage = _localizer["SaveUserFails"];
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["UserProfileChangeSuccessful"];
            return RedirectToPage();
        }
    }
}
