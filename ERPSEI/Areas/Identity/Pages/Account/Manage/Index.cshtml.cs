#nullable disable

using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IContactoEmergenciaManager _contactoEmergenciaManager;
        private readonly IEmpleadoManager _empleadoManager;
        private readonly AppUserManager _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IArchivoEmpleadoManager _userFileManager;
        private readonly IStringLocalizer<IndexModel> _localizer;
        private readonly ApplicationDbContext _db;

        private readonly long maxFileSizeInBytes = 5242880; //5mb = (5 * 1024) * 1024;
        private readonly long oneMegabyteSizeInBytes = 1048576; // 1mb = (1 * 1024) * 1024

        public IndexModel(
            IContactoEmergenciaManager contactoEmergenciaManager,
            IEmpleadoManager empleadoManager,
            AppUserManager userManager,
            SignInManager<AppUser> signInManager,
            IArchivoEmpleadoManager userFileManager,
            IStringLocalizer<IndexModel> localizer,
            ApplicationDbContext db)
        {
            _contactoEmergenciaManager = contactoEmergenciaManager;
            _empleadoManager = empleadoManager;
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


        public string ProfilePictureId { get; set; }
        public string ProfilePictureSrc { get; set; }

        public class FileFromGet
        {
            public string Name { get; set; }
            public string Extension { get; set; }
            public string FileId { get; set; }
            public string Src { get; set; }
            public int TypeId { get; set; }
            public long FileSize { get; set; }
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
            [DataType(DataType.Text)]
            [Display(Name = "PreferredNameField")]
            public string NombrePreferido { get; set; } = string.Empty;

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

            [DataType(DataType.DateTime)]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "FechaNacimientoField")]
            public DateTime FechaNacimiento { get; set; }

            [Phone(ErrorMessage = "PhoneFormat")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
            [Display(Name = "PhoneNumberField")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Required")]
            [DataType(DataType.MultilineText)]
            [Display(Name = "DireccionField")]
            public string Direccion { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [StringLength(18, ErrorMessage = "FieldLength", MinimumLength = 18)]
            [RegularExpression(RegularExpressions.AlphanumNoSpaceNoUnderscore, ErrorMessage = "AlphanumNoSpaceNoUnderscore")]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "CURPField")]
            public string CURP { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [StringLength(13, ErrorMessage = "FieldLength", MinimumLength = 13)]
            [RegularExpression(RegularExpressions.AlphanumNoSpaceNoUnderscore, ErrorMessage = "AlphanumNoSpaceNoUnderscore")]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "RFCField")]
            public string RFC { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [StringLength(11, ErrorMessage = "FieldLength", MinimumLength = 9)]
            [RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "Numeric")]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "NSSField")]
            public string NSS { get; set; } = string.Empty;

            [Display(Name = "GeneroField")]
            public int? GeneroId { get; set; }

            [Display(Name = "EstadoCivilField")]
            public int? EstadoCivilId { get; set; }

            [DataType(DataType.Text)]
            [StringLength(60, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "NameField")]
            public string NombreContacto1 { get; set; } = string.Empty;

            [Phone(ErrorMessage = "PhoneFormat")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
            [Display(Name = "PhoneNumberField")]
            public string TelefonoContacto1 { get; set; } = string.Empty;

            [DataType(DataType.Text)]
            [StringLength(60, ErrorMessage = "FieldLength", MinimumLength = 2)]
            [RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
            [Display(Name = "NameField")]
            public string NombreContacto2 { get; set; } = string.Empty;

            [Phone(ErrorMessage = "PhoneFormat")]
            [StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
            [Display(Name = "PhoneNumberField")]
            public string TelefonoContacto2 { get; set; } = string.Empty;
        }


        private void LoadUserContacts(List<ContactoEmergencia> userContacts)
        {
            if(userContacts != null && userContacts.Count >= 1)
            {
                Input.NombreContacto1 = userContacts[0].Nombre;
                Input.TelefonoContacto1 = userContacts[0].Telefono;
            }

            if (userContacts != null && userContacts.Count >= 2)
            {
                Input.NombreContacto2 = userContacts[1].Nombre;
                Input.TelefonoContacto2 = userContacts[1].Telefono;
            }
        }

        private void LoadUserFiles(List<SemiArchivoEmpleado> userFiles) {
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
                    if ((int)i == (int)FileTypes.ImagenPerfil) { continue; }
                    FilesFromGet.Add(new FileFromGet() { FileId = new Guid().ToString(), TypeId = (int)i, Src = "" });
                }
            }
            else
            {
                //Si el usuario ya tiene archivos, se llena el arreglo de datos a partir de ellos.
                foreach (SemiArchivoEmpleado file in userFiles)
                {
                    if(file.TipoArchivoId == (int)FileTypes.ImagenPerfil) { continue; }  

                    FileFromGet fg = new FileFromGet() { FileId = file.Id, TypeId = file.TipoArchivoId ?? 0, Src = "", Name = file.Nombre, Extension = file.Extension, FileSize = file.FileSize };

                    FilesFromGet.Add(fg);
                }
            }
        }

        private async Task LoadAsync(AppUser user)
        {
            SemiArchivoEmpleado imagenPerfil = null;
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            ICollection<ContactoEmergencia> contactos = new List<ContactoEmergencia>();
            List<SemiArchivoEmpleado> archivos = new List<SemiArchivoEmpleado>();

			Input.Username = userName;

            user.Empleado = await _empleadoManager.GetByIdAsync(user.EmpleadoId??0);
            if(user.Empleado != null)
            {
                Input.FirstName = user.Empleado.Nombre;
                Input.NombrePreferido = user.Empleado.NombrePreferido;
                Input.FathersLastName = user.Empleado.ApellidoPaterno;
                Input.MothersLastName = user.Empleado.ApellidoMaterno;
                Input.FechaNacimiento = user.Empleado.FechaNacimiento;
                Input.GeneroId = user.Empleado.GeneroId;
                Input.EstadoCivilId = user.Empleado.EstadoCivilId;
                Input.Direccion = user.Empleado.Direccion;
                Input.CURP = user.Empleado.CURP;
                Input.RFC = user.Empleado.RFC;
                Input.NSS = user.Empleado.NSS;

                contactos = await _contactoEmergenciaManager.GetContactosByEmpleadoIdAsync(user.EmpleadoId ?? 0);
                LoadUserContacts(contactos.ToList());

                archivos = await _userFileManager.GetFilesByEmpleadoIdAsync(user.EmpleadoId ?? 0);
                foreach (SemiArchivoEmpleado a in archivos)
                {
                    if (a.TipoArchivoId == (int)FileTypes.ImagenPerfil)
                    {
                        ArchivoEmpleado ae = _userFileManager.GetFileById(a.Id);
                        if (ae != null) { a.Archivo = ae.Archivo; }
                        break;
                    }
                }
                imagenPerfil = archivos.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil).FirstOrDefault();
			}

            //Si el usuario tiene imagen de perfil
            if (imagenPerfil != null && imagenPerfil.FileSize >= 1)
            {
                //Se usa para mostrarla
                ProfilePictureSrc = $"data:image/png;base64,{Convert.ToBase64String(imagenPerfil.Archivo)}";
                //Se guarda referencia del id del archivo
                ProfilePictureId = imagenPerfil.Id;
            }
            else
            {
                //De lo contrario, se usa la imagen default.
                ProfilePictureSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.png";
                ProfilePictureId = string.Empty;
            }

            Input.PhoneNumber = phoneNumber;

            LoadUserFiles(archivos);
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
            Empleado emp = await _empleadoManager.GetByIdAsync(user.EmpleadoId??0);
            if (user == null || emp == null)
            {
                return NotFound($"{_localizer["UserLoadFails"]} '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //Actualiza información principal del empleado.
            emp.Nombre = Input.FirstName;
            emp.NombrePreferido = Input.NombrePreferido ?? "";
            emp.ApellidoPaterno = Input.FathersLastName;
            emp.ApellidoMaterno = Input.MothersLastName;
            emp.FechaNacimiento = Input.FechaNacimiento;
            emp.Telefono = Input.PhoneNumber ?? "";
            emp.GeneroId = Input.GeneroId;
            emp.EstadoCivilId = Input.EstadoCivilId;
            emp.Direccion = Input.Direccion.Trim();
            emp.CURP = Input.CURP;
            emp.RFC = Input.RFC;
            emp.NSS = Input.NSS;

            //Actualiza información principal del usuario.
            user.PhoneNumber = Input.PhoneNumber ?? "";

            //Inicia una transacción.
            await _db.Database.BeginTransactionAsync();
            try
            {
                //Si el usuario estableció una imagen de perfil
                if (Input.ProfilePicture != null && Input.ProfilePicture.Length >= 1)
                {
                    //Si el usuario subió un archivo, borra el existente y sube el nuevo.
                    if (ProfilePictureId != null && ProfilePictureId.Length >= 1) { await _userFileManager.DeleteByIdAsync(ProfilePictureId); }
                    await saveUploadedFile(user, Input.ProfilePicture, (int)FileTypes.ImagenPerfil);
                }

                int fileType = 2;
                foreach (FileFromGet file in FilesFromGet)
                {
                    if (file == null) { continue; }

                    if (file.File != null && file.File.Length >= 1) 
                    {
                        //Si el usuario subió un archivo, borra el existente y sube el nuevo.
                        await _userFileManager.DeleteByIdAsync(file.FileId);
                        await saveUploadedFile(user, file.File, fileType); 
                    }
                    else if(file.FileSize <= 0)
                    {
                        //Si el usuario no subió archivo pero quitó el que estaba asignado, entonces borra el existente y sube uno vacío.
                        await _userFileManager.DeleteByIdAsync(file.FileId);
                        await saveEmptyFile(user.EmpleadoId ?? 0, fileType);
                    }

                    fileType++;
                }

                //Elimina los contactos del empleado.
                await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(emp.Id);

                //Crea dos nuevos contactos para el empleado.
                await _contactoEmergenciaManager.CreateAsync(
                    new ContactoEmergencia() { Nombre = Input.NombreContacto1 ?? string.Empty, Telefono = Input.TelefonoContacto1 ?? string.Empty, EmpleadoId = emp.Id }
                );
                await _contactoEmergenciaManager.CreateAsync(
                    new ContactoEmergencia() { Nombre = Input.NombreContacto2 ?? string.Empty, Telefono = Input.TelefonoContacto2 ?? string.Empty, EmpleadoId = emp.Id }
                );

                //Se actualiza el empleado.
                await _empleadoManager.UpdateAsync(emp);


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

        private async Task saveEmptyFile(int empleadoId, int typeId)
        {
            //Se guarda el archivo vacío
            await _userFileManager.CreateAsync(new ArchivoEmpleado()
            {
                TipoArchivoId = typeId,
                EmpleadoId = empleadoId
			});
        }

        private async Task saveUploadedFile(AppUser user, IFormFile file, int typeId)
        {
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName).Substring(1);
            //Transforma el archivo a memoryStream.
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                //Verifica que el tamaño del archivo no exceda el tamaño máximo.
                if (memoryStream.Length < maxFileSizeInBytes)
                {
                    //Se guarda el arreglo de bytes del archivo
                    await _userFileManager.CreateAsync(new ArchivoEmpleado()
                    {
                        Nombre = fileName,
                        Extension = fileExtension,
                        Archivo = memoryStream.ToArray(),
                        TipoArchivoId = typeId,
                        EmpleadoId = user.EmpleadoId ?? 0
                    });
                    
                }
                else
                {
                    //Se notifica error del tamaño máximo de archivo.
                    if (typeId == (int)FileTypes.ImagenPerfil)
                    {
                        StatusMessage = $"{_localizer["ProfilePictureTooLarge"]} {maxFileSizeInBytes / oneMegabyteSizeInBytes} Mb";
                    }
                    else
                    {
                        StatusMessage = $"{_localizer["FileTooLarge"]} {maxFileSizeInBytes / oneMegabyteSizeInBytes} Mb";
                    }
                }
            }
        }
    }
}