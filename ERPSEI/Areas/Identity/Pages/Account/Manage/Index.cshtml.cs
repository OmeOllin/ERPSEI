#nullable disable

using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Email;
using ERPSEI.Resources;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        private readonly IEmailSender _emailSender;
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
            IEmailSender emailSender,
            ApplicationDbContext db)
        {
            _contactoEmergenciaManager = contactoEmergenciaManager;
            _empleadoManager = empleadoManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _userFileManager = userFileManager;
            _localizer = localizer;
            _emailSender = emailSender;
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
            [Display(Name = "NameField")]
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
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Display(Name = "PhoneNumberField")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Required")]
            [DataType(DataType.MultilineText)]
			[StringLength(300, ErrorMessage = "FieldLength", MinimumLength = 1)]
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
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
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
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
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

        private async Task<string> validarSiExisteEmpleado(Empleado emp, bool curpAsKey)
        {
            List<Empleado> coincidences = new List<Empleado>();
            List<Empleado> emps = await _empleadoManager.GetAllAsync();

            if (curpAsKey)
            {
                //Excluyo al empleado con el CURP actual.
                emps = emps.Where(e => e.CURP != emp.CURP).ToList();
            }
            else
            {
                //Excluyo al empleado con el Id actual.
                emps = emps.Where(e => e.Id != emp.Id).ToList();
            }

            //Valido que no exista empleado que tenga los mismos datos.
            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.NombreCompleto ?? "").Length >= 1 && e.NombreCompleto == $"{emp.Nombre} {emp.ApellidoPaterno} {emp.ApellidoMaterno}").ToList();
            if (coincidences.Count() >= 1) { return $"{_localizer["ErrorEmpleadoExistenteA"]} {_localizer["Nombre"]} {emp.Nombre} {emp.ApellidoPaterno} {emp.ApellidoMaterno}. {_localizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.Email ?? "").Length >= 1 && e.Email == emp.Email).ToList();
            if (coincidences.Count() >= 1) { return $"{_localizer["ErrorEmpleadoExistenteA"]} {_localizer["Correo"]} {emp.Email}. {_localizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.CURP ?? "").Length >= 1 && e.CURP == emp.CURP).ToList();
            if (coincidences.Count() >= 1) { return $"{_localizer["ErrorEmpleadoExistenteA"]} {_localizer["CURP"]} {emp.CURP}. {_localizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.RFC ?? "").Length >= 1 && e.RFC == emp.RFC).ToList();
            if (coincidences.Count() >= 1) { return $"{_localizer["ErrorEmpleadoExistenteA"]} {_localizer["RFC"]} {emp.RFC}. {_localizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.NSS ?? "").Length >= 1 && e.NSS == emp.NSS).ToList();
            if (coincidences.Count() >= 1) { return $"{_localizer["ErrorEmpleadoExistenteA"]} {_localizer["NSS"]} {emp.NSS}. {_localizer["ErrorEmpleadoExistenteB"]}."; }

            return string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool isNewEmployee = false;

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

            Empleado emp = await _empleadoManager.GetByIdAsync(user.EmpleadoId??0);
            isNewEmployee = emp == null;
            //Se crea el usuario por primera vez
            if (isNewEmployee) { emp = new Empleado(); }

            //Actualiza información principal del empleado.
            emp.Nombre = Input.FirstName.Trim();
            emp.NombrePreferido = Input.NombrePreferido.Trim() ?? "";
            emp.ApellidoPaterno = Input.FathersLastName.Trim();
            emp.ApellidoMaterno = Input.MothersLastName.Trim();
            emp.NombreCompleto = $"{Input.FirstName.Trim()} {Input.FathersLastName.Trim()} {Input.MothersLastName.Trim()}";
            emp.FechaNacimiento = Input.FechaNacimiento;
            emp.Telefono = Input.PhoneNumber ?? "";
            emp.GeneroId = Input.GeneroId;
            emp.EstadoCivilId = Input.EstadoCivilId;
            emp.Direccion = Input.Direccion.Trim();
            emp.CURP = Input.CURP.Trim();
            emp.RFC = Input.RFC.Trim();
            emp.NSS = Input.NSS.Trim();

            //Actualiza información principal del usuario.
            user.PhoneNumber = Input.PhoneNumber ?? "";

            //Valida que no exista un empleado registrado con los mismos datos. En caso de haber, se deja el mensaje en resp.Mensajes para ser mostrado al usuario.
            string msg = await validarSiExisteEmpleado(emp, false);

            //Si la longitud del mensaje de respuesta es mayor o igual a uno, se considera que hubo errores.
            if ((msg ?? "").Length >= 1)
            {
                StatusMessage = $"Error: {msg}";
                return RedirectToPage();
            }

            //Inicia una transacción.
            await _db.Database.BeginTransactionAsync();
            try
            {
                if (isNewEmployee)
                {
                    //Se crea el empleado.
                    user.EmpleadoId = await _empleadoManager.CreateAsync(emp);

                    //Se envía correo para solicitar autorización del usuario.
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateUserTokenAsync(user, "UserAuthorization", "UserAuthorization");
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrlAuth = Url.Page(
                        "/Account/AuthorizeUser",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, actionId = "1"},
                        protocol: Request.Scheme);

                    var callbackUrlReject = Url.Page(
                        "/Account/AuthorizeUser",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, actionId = "2" },
                        protocol: Request.Scheme);

                    //Se envía notificación al correo configurado para autorizar procesos de los candidatos
                    _emailSender.SendEmailAsync(ServicesConfiguration.MasterUser.Email, 
                        _localizer["EmailSubject"], $"{_localizer["EmailBodyFP"]} {user.Email} {_localizer["EmailBodySP"]}.<br /><br />{_localizer["EmailBodyAuthA"]} <a href='{callbackUrlAuth}'>{_localizer["EmailBodyAuthB"]}</a><br /><br />{_localizer["EmailBodyRejectA"]} <a href='{callbackUrlReject}'>{_localizer["EmailBodyRejectB"]}</a>.");
                }
                else
                {
                    //Se actualiza el empleado.
                    await _empleadoManager.UpdateAsync(emp);
                }

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
                await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(user.EmpleadoId ?? 0);

                //Crea dos nuevos contactos para el empleado.
                await _contactoEmergenciaManager.CreateAsync(
                    new ContactoEmergencia() { Nombre = Input.NombreContacto1 ?? string.Empty, Telefono = Input.TelefonoContacto1 ?? string.Empty, EmpleadoId = user.EmpleadoId ?? 0 }
                );
                await _contactoEmergenciaManager.CreateAsync(
                    new ContactoEmergencia() { Nombre = Input.NombreContacto2 ?? string.Empty, Telefono = Input.TelefonoContacto2 ?? string.Empty, EmpleadoId = user.EmpleadoId ?? 0 }
                );

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

				if (!user.IsPreregisterAuthorized)
				{
					return Redirect("~/Identity/Account/PendingUserAuthorization");
				}
			}
            catch (Exception)
            {
                //Revierte la transacción.
                await _db.Database.RollbackTransactionAsync();
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