// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ERPSEI.Data;
using ERPSEI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserFileManager _userFileManager;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            AppUserManager userManager,
            SignInManager<AppUser> signInManager,
            IUserFileManager userFileManager,
            IStringLocalizer<IndexModel> localizer,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userFileManager = userFileManager;
            _localizer = localizer;
            _db = db;

            Input = new InputModel();
            FilesFromGet = new List<FileFromGet>();

            //Se llena el arreglo de datos de los archivos.
            foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
            {
                //Omite el tipo imagen de perfil.
                if ((int)i == 0) { continue; }
                FilesFromGet.Add(new FileFromGet() { TypeId = (int)i });
            }
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProfilePictureSrc { get; set; }

        public class FileFromGet
        {
            public string Id { get; set; }
            public string Src { get; set; }
            public int TypeId { get; set; }
        }

        public class FileToPost
        {
            public IFormFile File { get; set; }
        }

        public List<FileFromGet> FilesFromGet { get; set; }

        [BindProperty]
        public List<FileToPost> FilesToPost { get; set; }

        public class InputModel
        {
            public IFormFile ProfilePicture {  get; set; }

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

            Input.Username = userName;
            Input.FirstName = user.FirstName;
            Input.SecondName = user.SecondName;
            Input.FathersLastName = user.FathersLastName;
            Input.MothersLastName = user.MothersLastName;
            Input.PhoneNumber = phoneNumber;

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

            //Carga los archivos del usuario.
            List<UserFile> userFiles = await _userFileManager.GetFilesByUserIdAsync(user.Id);
            //Recorre los archivos del usuario.
            foreach (UserFile file in userFiles)
            {
                //Si el archivo tiene contenido
                if (file.File != null && file.File.Length >= 1)
                {
                    //Asigna la información del archivo al arreglo de datos.
                    string b64 = Convert.ToBase64String(file.File);
                    string imgSrc = $"data:image/png;base64,{b64}";
                    string id = Guid.NewGuid().ToString();
                    FileTypes type = (FileTypes)file.FileTypeId;
                    string src; 
                    if (file.Extension == "pdf")
                    {
                        src = $"<canvas id = '{id}' b64 = '{b64}' class = 'canvaspdf'></canvas>";
                    }
                    else
                    {
                        src = $"<img id = '{id}' src = '{imgSrc}' style='min-height: 200px;'/>";
                    }


                    FileFromGet fd = FilesFromGet.Find(x => x.TypeId == file.FileTypeId);
                    if (fd != null) { 
                        fd.Id = file.Id;
                        fd.Src = src;
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

            //Inicia una transacción.
            await _db.Database.BeginTransactionAsync();
            try
            {

                //Si el usuario estableció una imagen de perfil
                if (Input.ProfilePicture != null && Input.ProfilePicture.Length >= 1)
                {
                    await saveUploadedFile(user, Input.ProfilePicture, FileTypes.ImagenPerfil);
                }

                int fileType = 0;
                foreach (FileToPost file in FilesToPost)
                {
                    FileFromGet fg = FilesFromGet.Find(x => x.TypeId == fileType);
                    if (fg == null)
                    {
                        UserFile fileToRemove = (from UserFile uf in userFiles where uf.FileTypeId == fileType select uf).FirstOrDefault();
                        if (fileToRemove != null) { await _userFileManager.DeleteAsync(fileToRemove); }
                    }

                    if (file != null && file.File.Length >= 0) { 
                        await saveUploadedFile(user, file.File, (FileTypes)fileType);
                    }

                    fileType++;
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

                //Confirma la transacción
                await _db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                //Revierte la transacción.
                await _db.Database.RollbackTransactionAsync();
                throw;
            }
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