// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IArchivoEmpleadoManager _userFileManager;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            AppUserManager userManager,
            SignInManager<AppUser> signInManager,
            IArchivoEmpleadoManager userFileManager,
            IStringLocalizer<IndexModel> localizer,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userFileManager = userFileManager;
            _localizer = localizer;
            _db = db;
        }



        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public List<FileFromGet> FilesFromGet { get; set; }



        public string ProfilePictureSrc { get; set; }

        public class FileFromGet
        {
            public string FileId { get; set; }
            public string Src { get; set; }
            public int TypeId { get; set; }
            public IFormFile File { get; set; }
        }

        public class InputModel
        {
            public IFormFile ProfilePicture {  get; set; }

            [Display(Name = "UserNameField")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "FirstNameField")]
            public string FirstName { get; set; }

            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "SecondNameField")]
            public string SecondName {  get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "FathersLastNameField")]
            public string FathersLastName {  get; set; }

            [Required(ErrorMessage = "Required")]
            [StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "MothersLastNameField")]
            public string MothersLastName { get; set; }

            [Phone(ErrorMessage = "PhoneFormat")]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "PhoneNumberField")]
            public string PhoneNumber { get; set; }
        }




        private async Task LoadUserFilesAsync(int empleadoId) {
            //Carga y recorre los archivos del usuario.
            List<ArchivoEmpleado> userFiles = await _userFileManager.GetFilesByEmpleadoIdAsync(empleadoId);
            //Ordena los archivos del usuario por tipo de archivo de manera ascendente
            userFiles = (from userFile in userFiles
                         orderby userFile.TipoArchivoId ascending
                         select userFile).ToList();
            if(userFiles == null || userFiles.Count == 0)
            {
                //Si el usuario no tiene archivos, se llena el arreglo de datos a partir del enum.
                foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
                {
                    //Omite el tipo imagen de perfil.
                    if ((int)i == 0) { continue; }
                    FilesFromGet.Add(new FileFromGet() { FileId = new Guid().ToString(), TypeId = (int)i, Src = "" });
                }
            }
            else
            {
                //Si el usuario ya tiene archivos, se llena el arreglo de datos a partir de ellos.
                foreach (ArchivoEmpleado file in userFiles)
                {
                    FileFromGet fg = new FileFromGet() { FileId = file.Id, TypeId = file.TipoArchivoId ?? 0, Src = "" };
                    //Si el archivo tiene contenido
                    if (file.Archivo != null && file.Archivo.Length >= 1)
                    {
                        //Asigna la información del archivo al arreglo de datos.
                        string b64 = Convert.ToBase64String(file.Archivo);
                        string imgSrc = $"data:image/png;base64,{b64}";
                        string id = Guid.NewGuid().ToString();

                        if (file.Extension == "pdf")
                        {
                            fg.Src = $"<canvas id = '{id}' b64 = '{b64}' class = 'canvaspdf'></canvas>";
                        }
                        else
                        {
                            fg.Src = $"<img id = '{id}' src = '{imgSrc}' style='min-height: 200px;'/>";
                        }
                    }

                    FilesFromGet.Add(fg);
                }
            }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input.Username = userName;
            Input.FirstName = user.Empleado.Nombre;
            Input.FathersLastName = user.Empleado.ApellidoPaterno;
            Input.MothersLastName = user.Empleado.ApellidoMaterno;
            Input.PhoneNumber = phoneNumber;

            //Si el usuario tiene imagen de perfil
            //if (user.Empleado.ProfilePicture != null && user.Empleado.ProfilePicture.Length >= 1)
            //{
            //    //Se usa para mostrarla
            //    ProfilePictureSrc = $"data:image/png;base64,{Convert.ToBase64String(user.Empleado.ProfilePicture)}";
            //}
            //else
            //{
            //    //De lo contrario, se usa la imagen default.
            //    ProfilePictureSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.png";
            //}

            await LoadUserFilesAsync(user.EmpleadoId ?? 0);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{_userManager.GetUserId(User)}'.");
            }

            Input = new InputModel();
            FilesFromGet = new List<FileFromGet>();

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

            //Actualiza información principal del usuario.
            user.Empleado.Nombre = Input.FirstName;
            user.Empleado.ApellidoPaterno = Input.FathersLastName;
            user.Empleado.ApellidoMaterno = Input.MothersLastName;
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

                int fileType = 1;
                foreach (FileFromGet file in FilesFromGet)
                {
                    if (file == null) { continue; }

                    if (file.File != null && file.File.Length >= 0) 
                    {
                        //Si el usuario subió un archivo, borra el existente y sube el nuevo.
                        await _userFileManager.DeleteByIdAsync(file.FileId);
                        await saveUploadedFile(user, file.File, (FileTypes)fileType); 
                    }
                    else if(file.FileId.Length <= 0)
                    {
                        //Si el usuario no subió archivo pero quitó el que estaba asignado, entonces borra el existente y sube uno vacío.
                        await _userFileManager.DeleteByIdAsync(file.FileId);
                        await saveEmptyFile(user.EmpleadoId ?? 0, (FileTypes)fileType);
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

        private async Task saveEmptyFile(int empleadoId, FileTypes type)
        {
            //Se guarda el archivo vacío
            await _userFileManager.CreateAsync(new ArchivoEmpleado()
            {
                TipoArchivoId = (int)type,
                EmpleadoId = empleadoId
			});
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
                        //user.Empleado.ProfilePicture = memoryStream.ToArray();
                    }
                    else
                    {
                        //Se guarda el arreglo de bytes del archivo
                        await _userFileManager.CreateAsync(new ArchivoEmpleado()
                        {
                            Nombre = fileName,
                            Extension = fileExtension,
                            Archivo = memoryStream.ToArray(),
                            TipoArchivoId = (int)type,
                            EmpleadoId = user.EmpleadoId ?? 0
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