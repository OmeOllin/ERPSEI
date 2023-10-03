// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ERPSEI.Data.Entities;
using ERPSEI.Data.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using SQLitePCL;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserFileManager _userFileManager;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(
            AppUserManager userManager,
            SignInManager<AppUser> signInManager,
            IUserFileManager userFileManager,
            IStringLocalizer<IndexModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userFileManager = userFileManager;
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

        public string ActaNacimientoSrc { get; set; }
		public string ActaNacimientoId { get; set; }

		public string CURPSrc { get; set; }
        public string CURPId { get; set; }

        public string CLABESrc { get; set; }
        public string CLABEId { get; set; }

        public string ComprobanteDomiciloSrc { get; set; }
        public string ComprobanteDomiciloId { get; set; }

        public string ContactosEmergenciaSrc { get; set; }
        public string ContactosEmergenciaId { get; set; }

        public string CSFSrc { get; set; }
        public string CSFId { get; set; }

        public string INESrc { get; set; }
        public string INEId { get; set; }

        public string RFCSrc { get; set; }
        public string RFCId { get; set; }

        public string ComprobanteEstudiosSrc { get; set; }
        public string ComprobanteEstudiosId { get; set; }

        public string NSSSrc { get; set; }
        public string NSSId { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public IFormFile ProfilePicture {  get; set; }

            public IFormFile Acta { get; set; }

            public IFormFile CURP { get; set; }

            public IFormFile CLABE { get; set; }

            public IFormFile ComprobanteDomicilio { get; set; }

            public IFormFile ContactosEmergencia { get; set; }

            public IFormFile CSF { get; set; }

            public IFormFile INE { get; set; }

            public IFormFile RFC { get; set; }

            public IFormFile ComprobanteEstudios { get; set; }

            public IFormFile NSS { get; set; }

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
            if(user.ProfilePicture != null && user.ProfilePicture.Length >= 1) 
            { 
                //Se usa para mostrarla
                ProfilePictureSrc = $"data:image/png;base64,{Convert.ToBase64String(user.ProfilePicture)}"; 
            }
            else
            {
                //De lo contrario, se usa la imagen default.
                ProfilePictureSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.jpg";
            }

            List<UserFile> files = await _userFileManager.GetFilesByUserIdAsync(user.Id);

            foreach (UserFile file in files)
            {
                if (file.File != null && file.File.Length >= 1)
                {
                    string b64 = Convert.ToBase64String(file.File);
                    string imgSrc = $"data:image/png;base64,{b64}";
                    string id = "";
                    switch (file.FileTypeId)
                    {
                        case (int)FileTypes.ActaNacimiento:
                            id = "firstSourceContainer_children";
							ActaNacimientoId = file.Id;
							if (file.Extension == "pdf") 
                            { 
                                ActaNacimientoSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";

							}
                            else
                            {
                                ActaNacimientoSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.CURP:
                            id = "secondSourceContainer_children";
                            CURPId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                CURPSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                CURPSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.CLABE:
                            id = "thirdSourceContainer_children";
                            CLABEId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                CLABESrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                CLABESrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.ComprobanteDomicilio:
                            id = "fourthSourceContainer_children";
                            ComprobanteDomiciloId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                ComprobanteDomiciloSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                ComprobanteDomiciloSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.ContactosEmergencia:
                            id = "fifthSourceContainer_children";
                            ContactosEmergenciaId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                ContactosEmergenciaSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                ContactosEmergenciaSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.CSF:
                            id = "sixthSourceContainer_children";
                            CSFId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                CSFSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                CSFSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.INE:
                            id = "seventhSourceContainer_children";
                            INEId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                INESrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                INESrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.RFC:
                            id = "eighthSourceContainer_children";
                            RFCId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                RFCSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                RFCSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.ComprobanteEstudios:
                            id = "ninethSourceContainer_children";
                            ComprobanteEstudiosId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                ComprobanteEstudiosSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                ComprobanteEstudiosSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        case (int)FileTypes.NSS:
                            id = "tenthSourceContainer_children";
                            NSSId = file.Id;
                            if (file.Extension == "pdf")
                            {
                                NSSSrc = "<canvas id = '" + id + "' b64='" + b64 + "' class = 'canvaspdf document-container'></canvas>";
                            }
                            else
                            {
                                NSSSrc = "<img id = '" + id + "' class = 'document-container' src = '" + imgSrc + "'/>";
                            }
                            break;
                        default:
                            break;
                    }
                }
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
            List<UserFile> userFiles = await _userFileManager.GetFilesByUserIdAsync(user.Id);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //Actualiza información principal del usuario.
            user.FirstName = Input.FirstName;
            user.SecondName = Input.SecondName ?? "";
            user.FathersLastName = Input.FathersLastName;
            user.MothersLastName = Input.MothersLastName;
            user.PhoneNumber = Input.PhoneNumber ?? "";


            //Si el usuario estableció una imagen de perfil
            if (Input.ProfilePicture != null && Input.ProfilePicture.Length >= 1)
            {
                await saveUploadedFile(user, Input.ProfilePicture, FileTypes.ImagenPerfil);
            }

            //Si el usuario estableció acta de nacimiento
            if (Input.Acta != null && Input.Acta.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.ActaNacimiento select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.Acta, FileTypes.ActaNacimiento);
            }

            //Si el usuario estableció CURP
            if (Input.CURP != null && Input.CURP.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.CURP select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.CURP, FileTypes.CURP);
            }

            //Si el usuario estableció CLABE
            if (Input.CLABE != null && Input.CLABE.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.CLABE select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.CLABE, FileTypes.CLABE);
            }

            //Si el usuario estableció Comprobante de domicilio
            if (Input.ComprobanteDomicilio  != null && Input.ComprobanteDomicilio.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.ComprobanteDomicilio select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.ComprobanteDomicilio, FileTypes.ComprobanteDomicilio);
            }

            //Si el usuario estableció Contactos de emergencia
            if (Input.ContactosEmergencia != null && Input.ContactosEmergencia.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.ContactosEmergencia select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.ContactosEmergencia, FileTypes.ContactosEmergencia);
            }

            //Si el usuario estableció Constancia de situación fiscal
            if (Input.CSF != null && Input.CSF.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.CSF select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.CSF, FileTypes.CSF);
            }

            //Si el usuario estableció INE
            if (Input.INE != null && Input.INE.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.INE select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.INE, FileTypes.INE);
            }

            //Si el usuario estableció RFC
            if (Input.RFC != null && Input.RFC.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.RFC select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.RFC, FileTypes.RFC);
            }

            //Si el usuario estableció Comprobante de estudios
            if (Input.ComprobanteEstudios != null && Input.ComprobanteEstudios.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.ComprobanteEstudios select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.ComprobanteEstudios, FileTypes.ComprobanteEstudios);
            }

            //Si el usuario estableció NSS
            if (Input.NSS != null && Input.NSS.Length >= 1)
            {
                UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == (int)FileTypes.NSS select uf).FirstOrDefault();
                if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                await saveUploadedFile(user, Input.NSS, FileTypes.NSS);
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

        private async Task saveUploadedFile(AppUser user, IFormFile file, FileTypes type)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName).Substring(1);
            //Transforma el archivo a memoryStream.
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                int maxSizeInBytes = 1000000;
                //Verifica que el tamaño máximo no exceda el megabyte
                if (memoryStream.Length < maxSizeInBytes)
                {
                    if (type == FileTypes.ImagenPerfil)
                    {
                        //Se guarda el arreglo de bytes de la imagen.
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                    else
                    {
                        //Se guarda el arreglo de bytes del archivo
                        await _userFileManager.CreateAsync(new UserFile()
                        {
                            Name = fileName,
                            Extension = fileExtension,
                            File = memoryStream.ToArray(),
                            FileTypeId = (int)type,
                            UserId = user.Id
                        });
                    }
                }
                else
                {
                    //Se notifica error del tamaño máximo de archivo.
                    if (type == FileTypes.ImagenPerfil)
                    {
                        StatusMessage = $"{_localizer["ProfilePictureTooLarge"]} {maxSizeInBytes / 1000000} Mb";
                    }
                    else
                    {
                        StatusMessage = $"{_localizer["FileTooLarge"]} {maxSizeInBytes / 1000000} Mb";
                    }
                }
            }
        }
    }
}
