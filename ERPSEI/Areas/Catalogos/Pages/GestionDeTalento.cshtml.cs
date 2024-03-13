using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Email;
using ERPSEI.Requests;
using ERPSEI.Resources;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;
using System.Text;
using System.Text.Encodings.Web;

namespace ERPSEI.Areas.Catalogos.Pages
{
    [Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
    public class GestionDeTalentoModel : PageModel
	{
		private readonly IUserStore<AppUser> _userStore;
		private readonly IUserEmailStore<AppUser> _emailStore;
		private readonly AppUserManager _userManager;
		private readonly IEmpleadoManager _empleadoManager;
		private readonly IRWCatalogoManager<Area> _areaManager;
		private readonly IRWCatalogoManager<Subarea> _subareaManager;
		private readonly IRWCatalogoManager<Puesto> _puestoManager;
		private readonly IRWCatalogoManager<Oficina> _oficinaManager;

		private readonly IRCatalogoManager<Genero> _generoManager;
		private readonly IRCatalogoManager<EstadoCivil> _estadoCivilManager;
		private readonly IContactoEmergenciaManager _contactoEmergenciaManager;
		private readonly IArchivoEmpleadoManager _archivoEmpleadoManager;
		private readonly IStringLocalizer<GestionDeTalentoModel> _strLocalizer;
		private readonly ILogger<GestionDeTalentoModel> _logger;
		private readonly ApplicationDbContext _db;

		private readonly IEmailSender _emailSender;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }

		public class FiltroModel
		{
			[DataType(DataType.DateTime)]
			[Display(Name = "FechaIngresoInicioField")]
			public DateTime? FechaIngresoInicio { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaIngresoFinField")]
			public DateTime? FechaIngresoFin { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaNacimientoInicioField")]
			public DateTime? FechaNacimientoInicio { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaNacimientoFinField")]
			public DateTime? FechaNacimientoFin { get; set; }

			[Display(Name = "PuestoField")]
			public int? PuestoId { get; set; }

			[Display(Name = "AreaField")]
			public int? AreaId { get; set; }

			[Display(Name = "SubareaField")]
			public int? SubareaId { get; set; }

			[Display(Name = "OficinaField")]
			public int? OficinaId { get; set; }
		}

		[BindProperty]
		public EmpleadoModel InputEmpleado { get; set; }

		public class EmpleadoModel
		{
			public int Id { get; set; }

			public IFormFile? ProfilePicture { get; set; }

			[Required(ErrorMessage = "Required")]
			[StringLength(30, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "PreferredNameField")]
			public string? NombrePreferido { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[StringLength(30, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "FathersLastNameField")]
			public string ApellidoPaterno { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[StringLength(30, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "MothersLastNameField")]
			public string ApellidoMaterno { get; set; } = string.Empty;

			[DataType(DataType.DateTime)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "FechaNacimientoField")]
			public DateTime FechaNacimiento { get; set; }

			[DataType(DataType.DateTime)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "FechaIngresoField")]
			public DateTime FechaIngreso { get; set; }

			[Required(ErrorMessage = "Required")]
			[DataType(DataType.MultilineText)]
			[StringLength(300, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Display(Name = "DireccionField")]
			public string? Direccion { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "PhoneNumberField")]
			public string Telefono { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoEmpresarialField")]
			public string Email { get; set; } = string.Empty;

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

			[Required(ErrorMessage = "Required")]
			[Display(Name = "PuestoField")]
			public int PuestoId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "AreaField")]
			public int AreaId { get; set; }

			[Display(Name = "SubareaField")]
			public int? SubareaId { get; set; }

			[Display(Name = "OficinaField")]
			public int? OficinaId { get; set; }

			[Display(Name = "JefeField")]
			public int? JefeId { get; set; }

			[DataType(DataType.Text)]
			[StringLength(60, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Display(Name = "NameField")]
			public string? NombreContacto1 { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto1 { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(60, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Display(Name = "NameField")]
			public string? NombreContacto2 { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto2 { get; set; } = string.Empty;

			public ArchivoModel?[] Archivos { get; set; } = Array.Empty<ArchivoModel>();
		}

		public class ArchivoModel
		{
			public string? Id { get; set; } = string.Empty;
			public string? nombre { get; set; } = string.Empty;
			public int? tipoArchivoId { get; set; }
			public string? extension { get; set; } = string.Empty;
			public string? imgSrc { get; set; } = string.Empty;
		}

		[BindProperty]
		public ImportarModel InputImportar { get; set; }
		public class ImportarModel
		{
			[Required(ErrorMessage = "Required")]
			public IFormFile? Plantilla { get; set; }
		}

		public GestionDeTalentoModel(
			IUserStore<AppUser> store,
			AppUserManager userManager,
			IEmpleadoManager empleadoManager,
			IRWCatalogoManager<Area> areaManager,
			IRWCatalogoManager<Subarea> subareaManager,
			IRWCatalogoManager<Puesto> puestoManager,
			IRWCatalogoManager<Oficina> oficinaManager,
			IRCatalogoManager<Genero> generoManager,
			IRCatalogoManager<EstadoCivil> estadoCivilManager,
			IContactoEmergenciaManager contactoEmergenciaManager,
			IArchivoEmpleadoManager archivoEmpleadoManager,
			IStringLocalizer<GestionDeTalentoModel> stringLocalizer,
			ILogger<GestionDeTalentoModel> logger,
			ApplicationDbContext db,
			IEmailSender emailSender
		)
		{
			_userStore = store;
			_userManager = userManager;
			_emailStore = GetEmailStore();
			_empleadoManager = empleadoManager;
			_areaManager = areaManager;
			_subareaManager = subareaManager;
			_puestoManager = puestoManager;
			_oficinaManager = oficinaManager;
			_generoManager = generoManager;
			_estadoCivilManager = estadoCivilManager;
			_contactoEmergenciaManager = contactoEmergenciaManager;
			_archivoEmpleadoManager = archivoEmpleadoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
			_db = db;
			_emailSender = emailSender;

			InputFiltro = new FiltroModel();
			InputEmpleado = new EmpleadoModel();
			InputImportar = new ImportarModel();
		}

		private IUserEmailStore<AppUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("La UI default requiere almacenar un usuario con correo electrónico.");
			}
			return (IUserEmailStore<AppUser>)_userStore;
		}


		private async Task<string> GetDatosAdicionalesEmpleado(int idEmpleado)
		{
			string jsonResponse;
			string nombreJefe;
			Empleado? e = await _empleadoManager.GetByIdWithAdicionalesAsync(idEmpleado);

			if (e == null) { throw new Exception($"No se encontró información del empleado id {idEmpleado}"); }

			Empleado? jefe = e.JefeId != null ? await _empleadoManager.GetByIdAsync((int)e.JefeId) : null;
			List<SemiArchivoEmpleado> archivos = await _archivoEmpleadoManager.GetFilesByEmpleadoIdAsync(idEmpleado);
            foreach (SemiArchivoEmpleado a in archivos)
            {
				if(a.TipoArchivoId == (int)FileTypes.ImagenPerfil)
				{
					ArchivoEmpleado? ae = _archivoEmpleadoManager.GetFileById(a.Id);
					if (ae != null) { a.Archivo = ae.Archivo; }
					break;   
				}
            }
            List<string> jsonArchivos;
			List<string> jsonContactosEmergencia;

			nombreJefe = jefe != null ? jefe.NombreCompleto : "";

			jsonContactosEmergencia = getListJsonContactosEmergencia(e.ContactosEmergencia);

			jsonArchivos = getListJsonArchivos(archivos);

			jsonResponse = $"{{" +
								$"\"jefeId\": {e.JefeId ?? 0}, " +
								$"\"jefe\": \"{nombreJefe}\", " +
								$"\"contactosEmergencia\": [{string.Join(",", jsonContactosEmergencia)}], " +
								$"\"archivos\": [{string.Join(",", jsonArchivos)}] " +
							$"}}";

			return jsonResponse;
		}
		private async Task<string> GetTalentList(FiltroModel? filtro = null)
		{
			string nombreArea;
			string nombreSubarea;
			string nombrePuesto;
			string nombreOficina;
			string nombreGenero;
			string nombreEstadoCivil;
			bool usuarioConfirmado = false;
			string jsonResponse;
			List<string> jsonEmpleados = new List<string>();
			List<Empleado> empleados;

			if (filtro != null)
			{
				empleados = await _empleadoManager.GetAllAsync(
					filtro.FechaIngresoInicio, 
					filtro.FechaIngresoFin, 
					filtro.FechaNacimientoInicio, 
					filtro.FechaNacimientoFin,
					filtro.PuestoId,
					filtro.AreaId,
					filtro.SubareaId,
					filtro.OficinaId
				);
			}
			else
			{
				empleados = await _empleadoManager.GetAllAsync();
			}

			foreach (Empleado e in empleados)
			{
				nombreArea = e.Area != null ? e.Area.Nombre : "";
				nombreSubarea = e.Subarea != null ? e.Subarea.Nombre : "";
				nombrePuesto = e.Puesto != null ? e.Puesto.Nombre : "";
				nombreOficina = e.Oficina != null ? e.Oficina.Nombre : "";
				nombreGenero = e.Genero != null ? e.Genero.Nombre : "";
				nombreEstadoCivil = e.EstadoCivil != null ? e.EstadoCivil.Nombre : "";
				AppUser? usuario = e.UserId != null && e.UserId.Length >= 1 ? await _userManager.FindByIdAsync(e.UserId) : null;
				usuarioConfirmado = usuario != null && usuario.EmailConfirmed;

					jsonEmpleados.Add(
					"{" +
						$"\"id\": {e.Id}," +
						$"\"nombre\": \"{e.Nombre}\", " +
						$"\"nombrePreferido\": \"{e.NombrePreferido}\", " +
						$"\"apellidoPaterno\": \"{e.ApellidoPaterno}\", " +
						$"\"apellidoMaterno\": \"{e.ApellidoMaterno}\", " +
						$"\"nombreCompleto\": \"{e.NombreCompleto}\", " +
						$"\"fechaIngreso\": \"{e.FechaIngreso:dd/MM/yyyy}\", " +
						$"\"fechaIngresoJS\": \"{e.FechaIngreso:yyyy-MM-dd}\", " +
						$"\"fechaNacimiento\": \"{e.FechaNacimiento:dd/MM/yyyy}\", " +
						$"\"fechaNacimientoJS\": \"{e.FechaNacimiento:yyyy-MM-dd}\", " +
						$"\"direccion\": \"{e.Direccion.Trim()}\", " +
						$"\"telefono\": \"{e.Telefono}\", " +
						$"\"email\": \"{e.Email}\", " +
						$"\"generoId\": {e.GeneroId ?? 0}, " +
						$"\"genero\": \"{nombreGenero}\", " +
						$"\"subareaId\": {e.SubareaId ?? 0}, " +
						$"\"subarea\": \"{nombreSubarea}\", " +
						$"\"oficinaId\": {e.OficinaId ?? 0}, " +
						$"\"oficina\": \"{nombreOficina}\", " +
						$"\"puestoId\": {e.PuestoId ?? 0}, " +
						$"\"puesto\": \"{nombrePuesto}\", " +
						$"\"areaId\": {e.AreaId ?? 0}, " +
						$"\"area\": \"{nombreArea}\", " +
						$"\"estadoCivilId\": {e.EstadoCivilId ?? 0}, " +
						$"\"estadoCivil\": \"{nombreEstadoCivil}\", " +
						$"\"jefeId\": {e.JefeId ?? 0}, " +
						$"\"jefe\": \"\", " +
						$"\"curp\": \"{e.CURP}\", " +
						$"\"rfc\": \"{e.RFC}\", " +
						$"\"nss\": \"{e.NSS}\", " +
						$"\"usuarioId\": \"{e.UserId}\", " +
						$"\"usuarioValido\": \"{(usuarioConfirmado ? "1" : "0")}\", " +
						$"\"contactosEmergencia\": [], " +
						$"\"archivos\": [] " +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonEmpleados)}]";

			return jsonResponse;
		}
		private List<string> getListJsonContactosEmergencia(ICollection<ContactoEmergencia>? contactos)
		{
			List<string> jsonContactosEmergencia = new List<string>();
			if (contactos != null)
			{
				List<ContactoEmergencia> contacts = (from c in contactos
													  orderby c.Id ascending
													  select c).ToList();
				foreach (ContactoEmergencia c in contacts)
				{
					jsonContactosEmergencia.Add($"{{\"nombre\": \"{c.Nombre}\", \"telefono\": \"{c.Telefono}\"}}");
				}
			}
			return jsonContactosEmergencia;
		}
		private List<string> getListJsonArchivos(ICollection<SemiArchivoEmpleado>? archivos)
		{
			List<string> jsonArchivos = new List<string>();
			if (archivos != null)
			{
				//Si el usuario ya tiene archivos, se llena el arreglo de datos a partir de ellos.					
				List<SemiArchivoEmpleado> userFiles = (from userFile in archivos
												   orderby userFile.TipoArchivoId ascending
												   select userFile).ToList();

				foreach (SemiArchivoEmpleado a in userFiles)
				{
					string htmlContainer = string.Empty;
					string imgSrc = string.Empty;
					string id = Guid.NewGuid().ToString();
					//Si el archivo tiene contenido
					if (a.FileSize >= 1)
					{
						//Asigna la información del archivo al arreglo de datos.
						string b64 = Convert.ToBase64String(a.Archivo);
						bool isJPG = a.Extension == "jpg" || a.Extension == "jpeg";
						bool isPNG = a.Extension == "png";
						bool isPDF = a.Extension == "pdf";

						if (isPDF)
						{
							imgSrc = $"data:application/pdf;base64,{b64}";
							htmlContainer = $"<canvas id = '{id}' b64 = '{b64}' class = 'canvaspdf'></canvas>";
						}
						else if (isJPG || isPNG)
						{
							if (isJPG)
							{
								imgSrc = $"data:image/jpeg;base64,{b64}";
							}
							else if (isPNG)
							{
								imgSrc = $"data:image/png;base64,{b64}";
							}
							htmlContainer = $"<img id = '{id}' src = '{imgSrc}' style='max-height: 200px;'/>";
						}
					}

					jsonArchivos.Add(
						"{" +
							$"\"id\": \"{a.Id}\"," +
                            $"\"nombre\": \"{a.Nombre}\"," +
							$"\"tipoArchivoId\": {a.TipoArchivoId}," +
							$"\"extension\": \"{a.Extension}\"," +
							$"\"imgSrc\": \"{imgSrc}\"," +
							$"\"htmlContainer\": \"{htmlContainer}\"," +
                            $"\"fileSize\": \"{a.FileSize}\"" +
						"}"
					);
				}
			}
			else
			{
				//Si el usuario no tiene archivos, se llena el arreglo de datos a partir del enum.
				foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
				{
					string imgSrc = string.Empty;
					string htmlContainer = string.Empty;
					//Si el tipo de archivo es Imagen de perfil, entonces agrega valor default.
					if ((int)i == 0) { imgSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.png"; }
					if ((int)i != 0) { htmlContainer = "<i class='bi bi-file-image opacity-50' style='font-size:105px'></i>"; }
					jsonArchivos.Add(
						"{" +
							$"\"id\": \"{Guid.NewGuid()}\"," +
							$"\"nombre\": \"\"," +
							$"\"tipoArchivoId\": {(int)i}," +
							$"\"extension\": \"\"," +
							$"\"imgSrc\": \"{imgSrc}\"," +
							$"\"htmlContainer\": \"{htmlContainer}\"," +
                            $"\"fileSize\": \"0\"" +
                        "}"
					);
				}
			}

			return jsonArchivos;
		}

		public ActionResult OnGetDownloadPlantilla()
		{
			return File("/templates/PlantillaEmpleados.xlsx", MediaTypeNames.Application.Octet, "PlantillaEmpleados.xlsx");
		}

		public async Task<JsonResult> OnPostDatosAdicionalesEmpleado(int idEmpleado)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadoConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetDatosAdicionalesEmpleado(idEmpleado);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadoConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		public async Task<JsonResult> OnPostFiltrarEmpleados()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosFiltradosUnsuccessfully"]);
			try
			{
				resp.Datos = await GetTalentList(InputFiltro);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadosFiltradosSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		
		public async Task<JsonResult> OnPostDisableEmpleados(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosDisabledUnsuccessfully"]);
			await _db.Database.BeginTransactionAsync();
			try
			{
                foreach (string id in ids)
                {
					int intId = Convert.ToInt32(id);
					//Deshabilita el empleado y por ende, el usuario relacionado.
					await _empleadoManager.DisableByIdAsync(intId);
                }

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadosDisabledSuccessfully"];

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				await _db.Database.RollbackTransactionAsync();
			}

			return new JsonResult(resp);
		}
		
		public async Task<JsonResult> OnPostSaveEmpleado()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadoSavedUnsuccessfully"]);

			//Se remueve el campo Plantilla para que no sea validado ya que no pertenece a este proceso.
			ModelState.Remove("Plantilla");

			if (!ModelState.IsValid)
			{
				resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				return new JsonResult(resp);
			}
			try
			{
				//Valida que no exista un empleado registrado con los mismos datos. En caso de haber, se deja el mensaje en resp.Mensajes para ser mostrado al usuario.
				resp.Mensaje = await validarSiExisteEmpleado(InputEmpleado, false);

				//Si la longitud del mensaje de respuesta es menor o igual a cero, se considera que no hubo errores anteriores.
				if ((resp.Mensaje ?? "").Length <= 0)
				{
					//Procede a crear o actualizar el empleado.
					await createOrUpdateEmployee(InputEmpleado);

					resp.TieneError = false;
					resp.Mensaje = _strLocalizer["EmpleadoSavedSuccessfully"];
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> validarSiExisteEmpleado(EmpleadoModel emp, bool curpAsKey)
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
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpleadoExistenteA"]} {_strLocalizer["Nombre"]} {emp.Nombre} {emp.ApellidoPaterno} {emp.ApellidoMaterno}. {_strLocalizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.Email ?? "").Length >= 1 && e.Email == emp.Email).ToList();
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpleadoExistenteA"]} {_strLocalizer["Correo"]} {emp.Email}. {_strLocalizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.CURP ?? "").Length >= 1 && e.CURP == emp.CURP).ToList();
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpleadoExistenteA"]} {_strLocalizer["CURP"]} {emp.CURP}. {_strLocalizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.RFC ?? "").Length >= 1 && e.RFC == emp.RFC).ToList();
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpleadoExistenteA"]} {_strLocalizer["RFC"]} {emp.RFC}. {_strLocalizer["ErrorEmpleadoExistenteB"]}."; }

            coincidences = emps.Where(e => e.Deshabilitado == 0 && (e.NSS ?? "").Length >= 1 && e.NSS == emp.NSS).ToList();
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpleadoExistenteA"]} {_strLocalizer["NSS"]} {emp.NSS}. {_strLocalizer["ErrorEmpleadoExistenteB"]}."; }

            return string.Empty;
		}
		private async Task createOrUpdateEmployee(EmpleadoModel e)
		{
			try
			{
				await _db.Database.BeginTransactionAsync();

				int idEmpleado = 0;

				//Se busca empleado por id
				Empleado? empleado = await _empleadoManager.GetByIdAsync(e.Id);
				//Si no se encontró empleado por id, se busca el empleado por su CURP. 
				if (empleado == null) { empleado = await _empleadoManager.GetByCURPAsync(e.CURP ?? string.Empty); }

				//Si se encontró empleado, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
				if (empleado != null) { idEmpleado = empleado.Id; } else { empleado = new Empleado(); }

				//Llena los datos del empleado.
				empleado.ApellidoMaterno = e.ApellidoMaterno;
				empleado.ApellidoPaterno = e.ApellidoPaterno;
				empleado.AreaId = e.AreaId;
				empleado.Direccion = e.Direccion ?? string.Empty;
				empleado.Email = e.Email;
				empleado.EstadoCivilId = e.EstadoCivilId;
				empleado.FechaIngreso = e.FechaIngreso;
				empleado.FechaNacimiento = e.FechaNacimiento;
				empleado.GeneroId = e.GeneroId;
				empleado.JefeId = e.JefeId;
				empleado.NombreCompleto = $"{e.Nombre} {e.ApellidoPaterno} {e.ApellidoMaterno}";
				empleado.OficinaId = e.OficinaId;
				empleado.Nombre = e.Nombre;
				empleado.NombrePreferido = e.NombrePreferido ?? string.Empty;
				empleado.PuestoId = e.PuestoId;
				empleado.SubareaId = e.SubareaId;
				empleado.Telefono = e.Telefono;
				empleado.CURP = e.CURP ?? string.Empty;
				empleado.RFC = e.RFC ?? string.Empty;
				empleado.NSS = e.NSS ?? string.Empty;

				List<ArchivoModel?> archivosActualizables;

                if (idEmpleado >= 1)
				{
					//Si existe el empleado, los archivos actualizables serán aquellos que traigan imgSrc, pues significa que el usuario añadió el archivo en la vista.
					archivosActualizables = e.Archivos.Where(a => a?.imgSrc?.Length >= 1).ToList();

					//Si el empleado ya existía, lo actualiza.
					await _empleadoManager.UpdateAsync(empleado);

					//Elimina los contactos del empleado.
					await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(idEmpleado);

                    //Elimina los archivos del usuario.
                    foreach (ArchivoModel? a in archivosActualizables)
                    {
                        if (a == null) { continue; }

                        await _archivoEmpleadoManager.DeleteByIdAsync(a.Id ?? string.Empty);
                    }
				}
				else
				{
                    //Si no existe el empleado, los archivos actualizables serán todos.
                    archivosActualizables = e.Archivos.ToList();

                    //De lo contrario, crea al empleado y obtiene su id.
                    idEmpleado = await _empleadoManager.CreateAsync(empleado);
				}

				//Crea dos nuevos contactos para el empleado.
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = e.NombreContacto1 ?? string.Empty, Telefono = e.TelefonoContacto1 ?? string.Empty, EmpleadoId = idEmpleado }
				);
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = e.NombreContacto2 ?? string.Empty, Telefono = e.TelefonoContacto2 ?? string.Empty, EmpleadoId = idEmpleado }
				);

				//Crea los archivos del usuario.
				foreach (ArchivoModel? a in archivosActualizables)
				{
					if (a != null)
					{
						await _archivoEmpleadoManager.CreateAsync(
							new ArchivoEmpleado() { Archivo = (a.imgSrc ?? string.Empty).Length >= 1 ? Convert.FromBase64String(a.imgSrc ?? string.Empty) : Array.Empty<byte>(), EmpleadoId = idEmpleado, Extension = a.extension ?? string.Empty, Nombre = a.nombre ?? string.Empty, TipoArchivoId = a.tipoArchivoId }
						);
					}
				}


				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await _db.Database.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<JsonResult> OnPostImportarEmpleados()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosImportadosUnsuccessfully"]);
			try
			{
				if (Request.Form.Files.Count >= 1)
				{
					//Se procesa el archivo excel.
					using (Stream s = Request.Form.Files[0].OpenReadStream())
					{
						using (var reader = ExcelReaderFactory.CreateReader(s))
						{
							DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });
							foreach (DataRow row in result.Tables[0].Rows)
							{
								//Omite el procesamiento del row de encabezado
								if (result.Tables[0].Rows.IndexOf(row) == 0) {
									resp.TieneError = false;
									resp.Mensaje = _strLocalizer["EmpleadosImportadosSuccessfully"];
									continue; 
								}

								string vmsg = await CreateEmployeeFromExcelRow(row);

								//Si la longitud del mensaje de respuesta es mayor o igual a uno, se considera que hubo errores.
								if ((vmsg ?? "").Length >= 1)
								{
									resp.TieneError = true;
									resp.Mensaje = vmsg;
									break;
								}
								else
								{
									resp.TieneError = false;
									resp.Mensaje = _strLocalizer["EmpleadosImportadosSuccessfully"];
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				resp.TieneError = true;
				resp.Mensaje = _strLocalizer["EmpresasImportadasUnsuccessfully"];
			}

			return new JsonResult(resp);
		}
		private async Task<string> CreateEmployeeFromExcelRow(DataRow row)
		{
			string validationMsg = string.Empty;
			Genero? genero = await _generoManager.GetByNameAsync(row[5].ToString()?.Trim() ?? string.Empty);
			EstadoCivil? estadoCivil = await _estadoCivilManager.GetByNameAsync(row[6].ToString()?.Trim() ?? string.Empty);
			Puesto? puesto = await _puestoManager.GetByNameAsync(row[8].ToString()?.Trim() ?? string.Empty);
			Area? area = await _areaManager.GetByNameAsync(row[9].ToString()?.Trim() ?? string.Empty);
			Subarea? subarea = await _subareaManager.GetByNameAsync(row[10].ToString()?.Trim() ?? string.Empty);
			Oficina? oficina = await _oficinaManager.GetByNameAsync(row[11].ToString()?.Trim() ?? string.Empty);
			Empleado? jefe = await _empleadoManager.GetByNameAsync(row[12].ToString()?.Trim() ?? string.Empty);

			DateTime fn;
			DateTime fi;
			DateTime.TryParse(row[3].ToString(), out fn);
			DateTime.TryParse(row[13].ToString(), out fi);

			EmpleadoModel e = new EmpleadoModel() {
				Nombre = row[0].ToString()?.Trim() ?? string.Empty,
				ApellidoPaterno = row[1].ToString()?.Trim() ?? string.Empty,
				ApellidoMaterno = row[2].ToString()?.Trim() ?? string.Empty,
				FechaNacimiento = fn,
				Telefono = row[4].ToString()?.Trim() ?? string.Empty,
				GeneroId = genero != null ? genero.Id : null,
				EstadoCivilId = estadoCivil != null ? estadoCivil.Id : null,
				Direccion = row[7].ToString()?.Trim() ?? string.Empty,
				PuestoId = puesto != null ? puesto.Id : 0,
				AreaId = area != null ? area.Id : 0,
				SubareaId = subarea != null ? subarea.Id : null,
				OficinaId = oficina != null ? oficina.Id : null,
				JefeId = jefe != null ? jefe.Id : null,
				FechaIngreso = fi,
				Email = row[14].ToString()?.Trim() ?? string.Empty,
				NombreContacto1 = row[15].ToString()?.Trim() ?? string.Empty,
				TelefonoContacto1 = row[16].ToString()?.Trim() ?? string.Empty,
				NombreContacto2 = row[17].ToString()?.Trim() ?? string.Empty,
				TelefonoContacto2 = row[18].ToString()?.Trim() ?? string.Empty,
				CURP = row[19].ToString()?.Trim() ?? string.Empty,
				RFC = row[20].ToString()?.Trim() ?? string.Empty,
				NSS = row[21].ToString()?.Trim() ?? string.Empty,
				NombrePreferido = row[22].ToString()?.Trim() ?? string.Empty,
			};

			List<ArchivoModel> archivos = new List<ArchivoModel>();
			//Crea los archivos del usuario.
			foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
			{
				archivos.Add(new ArchivoModel() { extension = "", imgSrc = "", nombre = "", tipoArchivoId = (int)i });
			}

			e.Archivos = archivos.ToArray();

			//Valida que no exista un empleado registrado con los mismos datos. En caso de haber, se deja el mensaje en resp.Mensajes para ser mostrado al usuario.
			validationMsg = await validarSiExisteEmpleado(e, true);

			//Si la longitud del mensaje de respuesta es menor o igual a cero, se considera que no hubo errores anteriores.
			if ((validationMsg ?? "").Length <= 0)
			{
				//Procede a crear o actualizar el empleado.
				await createOrUpdateEmployee(e);
			}

			return validationMsg ?? "";
		}

		private AppUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<AppUser>();
			}
			catch
			{
				throw new InvalidOperationException($"No se puede crear una instancia de '{nameof(AppUser)}'. " +
					$"Asegurese que '{nameof(AppUser)}' no es una clase abstracta y tiene un constructor sin parámetros, o alternativamente " +
					$"sobrecargue la página de registro en /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}
		public async Task<IActionResult> OnPostInvitarEmpleado(int id)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadoInvitadoUnsuccessfully"]);
			try
			{
				//Se busca al empleado por Id
				Empleado? emp = await _empleadoManager.GetByIdAsync(id);
				if (emp != null)
				{
					string password = _userManager.GenerateRandomPassword(10);
					AppUser ? user;

					if ((emp.UserId??"").Length <= 0)
					{
						//Si el empleado NO tiene usuario, entonces procede a crearlo.
						user = CreateUser();
						user.Email = emp.Email;
						user.EmpleadoId = emp.Id;
						user.NormalizedEmail = emp.Email.ToUpper();
						user.NormalizedUserName = emp.Email.ToUpper();
						user.PasswordResetNeeded = true;
						user.IsPreregisterAuthorized = true;
						user.PhoneNumber = emp.Telefono;
						user.UserName = emp.Email;

						//Crea el usuario.
						await _userStore.SetUserNameAsync(user, emp.Email, CancellationToken.None);
						await _emailStore.SetEmailAsync(user, emp.Email, CancellationToken.None);
						var result = await _userManager.CreateAsync(user, password);

						if (result.Succeeded)
						{
							//Se asigna rol de usuario
							await _userManager.AddToRoleAsync(user, ServicesConfiguration.RolUsuario);

							//Asigna el usuario al empleado.
							emp.UserId = user.Id;
							await _empleadoManager.UpdateAsync(emp);
						}
					}
					else
					{

						//De lo contrario, obtiene al usuario por Id
						user = await _userManager.FindByIdAsync(emp.UserId??"");

						if (user != null)
						{
							//Cambia el password del usuario.
							await _userManager.RemovePasswordAsync(user);
							await _userManager.AddPasswordAsync(user, password);
						}
					}

					if(user != null)
					{
						//Envía una invitación al empleado mediante correo electrónico.
						string userCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
						userCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(userCode));

						//El usuario tendrá que confirmar su correo para poder iniciar sesión.
						string userURL = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = user.Id, code = userCode },
							protocol: Request.Scheme)??string.Empty;

						string emailBody = $"" +
							$"{_strLocalizer["EmailBodyFP"]} {_strLocalizer["EmailBodySP"]} {HtmlEncoder.Default.Encode(userURL)}" +
							$"<br />" +
							$"<br />" +
							$"{_strLocalizer["EmailBodyTP"]}" +
							$"<br />" +
							$"<br />" +
							$"User: {emp.Email}" +
							$"<br />" +
							$"Password: {password}" +
							$"<br />" +
							$"<br />" +
							$"{_strLocalizer["EmailBodyFOP"]}";

                        _emailSender.SendEmailAsync(emp.Email,_strLocalizer["EmailSubject"], emailBody);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["EmpleadoInvitadoSuccessfully"];
					}

				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
	}
}