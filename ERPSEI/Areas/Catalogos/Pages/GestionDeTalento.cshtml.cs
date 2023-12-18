using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;

namespace ERPSEI.Areas.Catalogos.Pages
{
	public class GestionDeTalentoModel : PageModel
	{
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
			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "FathersLastNameField")]
			public string ApellidoPaterno { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
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
			[Display(Name = "DireccionField")]
			public string? Direccion { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
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
			[RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
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
			[RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto2 { get; set; } = string.Empty;

			public ArchivoModel?[] Archivos { get; set; } = Array.Empty<ArchivoModel>();
		}

		public class ArchivoModel
		{
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
			ApplicationDbContext db
		)
		{
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

			InputFiltro = new FiltroModel();
			InputEmpleado = new EmpleadoModel();
			InputImportar = new ImportarModel();
		}

		private async Task<string> GetTalentList(FiltroModel? filtro = null)
		{
			string nombreArea;
			string nombreSubarea;
			string nombrePuesto;
			string nombreOficina;
			string nombreGenero;
			string nombreEstadoCivil;
			string nombreJefe;
			string jsonResponse;
			List<string> jsonEmpleados = new List<string>();
			List<Empleado> empleados = await _empleadoManager.GetAllAsync();

			if(filtro != null)
			{
				if(filtro.FechaIngresoInicio != null) { empleados = empleados.Where(e => e.FechaIngreso >= filtro.FechaIngresoInicio).ToList(); }
				if (filtro.FechaIngresoFin != null) { empleados = empleados.Where(e => e.FechaIngreso <= filtro.FechaIngresoFin).ToList(); }
				if (filtro.FechaNacimientoInicio != null) { empleados = empleados.Where(e => e.FechaNacimiento >= filtro.FechaNacimientoInicio).ToList(); }
				if (filtro.FechaNacimientoFin != null) { empleados = empleados.Where(e => e.FechaNacimiento <= filtro.FechaNacimientoFin).ToList(); }
				if (filtro.PuestoId != null) { empleados = empleados.Where(e => e.PuestoId == filtro.PuestoId).ToList(); }
				if (filtro.AreaId != null) { empleados = empleados.Where(e => e.AreaId == filtro.AreaId).ToList(); }
				if (filtro.SubareaId != null) { empleados = empleados.Where(e => e.SubareaId == filtro.SubareaId).ToList(); }
				if (filtro.OficinaId != null) { empleados = empleados.Where(e => e.OficinaId == filtro.OficinaId).ToList(); }
			}

			foreach (Empleado e in empleados)
			{
				Empleado? jefe = e.JefeId != null ? _empleadoManager.GetByIdAsync((int)e.JefeId).Result : null;
				List<string> jsonContactosEmergencia;
				List<string> jsonArchivos;
				string ProfilePictureSrc = string.Empty;

				nombreArea = e.Area != null ? e.Area.Nombre : "";
				nombreSubarea = e.Subarea != null ? e.Subarea.Nombre : "";
				nombrePuesto = e.Puesto != null ? e.Puesto.Nombre : "";
				nombreOficina = e.Oficina != null ? e.Oficina.Nombre : "";
				nombreGenero = e.Genero != null ? e.Genero.Nombre : "";
				nombreEstadoCivil = e.EstadoCivil != null ? e.EstadoCivil.Nombre : "";
				nombreJefe = jefe != null ? jefe.NombreCompleto : "";

				jsonContactosEmergencia = getListJsonContactosEmergencia(e.ContactosEmergencia);

				jsonArchivos = getListJsonArchivos(e.ArchivosEmpleado);

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
						$"\"direccion\": \"{e.Direccion}\", " +
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
						$"\"jefe\": \"{nombreJefe}\", " +
						$"\"curp\": \"{e.CURP}\", " +
						$"\"rfc\": \"{e.RFC}\", " +
						$"\"nss\": \"{e.NSS}\", " +
						$"\"contactosEmergencia\": [{string.Join(",", jsonContactosEmergencia)}], " +
						$"\"archivos\": [{string.Join(",", jsonArchivos)}] " +
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
		private List<string> getListJsonArchivos(ICollection<ArchivoEmpleado>? archivos)
		{
			List<string> jsonArchivos = new List<string>();
			if (archivos != null)
			{
				//Si el usuario ya tiene archivos, se llena el arreglo de datos a partir de ellos.					
				List<ArchivoEmpleado> userFiles = (from userFile in archivos
												   orderby userFile.TipoArchivoId ascending
												   select userFile).ToList();

				foreach (ArchivoEmpleado a in userFiles)
				{
					string htmlContainer = string.Empty;
					string imgSrc = string.Empty;
					string id = Guid.NewGuid().ToString();
					//Si el archivo tiene contenido
					if (a.Archivo != null && a.Archivo.Length >= 1)
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
							$"\"htmlContainer\": \"{htmlContainer}\"" +
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
							$"\"htmlContainer\": \"{htmlContainer}\"" +
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
		
		public async Task<JsonResult> OnPostDeleteEmpleados(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadosDeletedUnsuccessfully"]);
			await _db.Database.BeginTransactionAsync();
			try
			{
                foreach (string id in ids)
                {
					int intId = Convert.ToInt32(id);
					//Elimina dependencias y posteriormente el empleado.
					await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(intId);
					await _archivoEmpleadoManager.DeleteByEmpleadoIdAsync(intId);
					await _empleadoManager.DeleteByIdAsync(intId);
                }

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadosDeletedSuccessfully"];

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
			coincidences = emps.Where(e => e.NombreCompleto == $"{emp.Nombre} {emp.ApellidoPaterno} {emp.ApellidoMaterno}").ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe un empleado registrado con el nombre de {emp.Nombre} {emp.ApellidoPaterno} {emp.ApellidoMaterno}. Por favor verifique la información"; }

			coincidences = emps.Where(e => e.Email == emp.Email).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe un empleado registrado con el correo {emp.Email}. Por favor verifique la información"; }

			coincidences = emps.Where(e => e.CURP == emp.CURP).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe un empleado registrado con el CURP {emp.CURP}. Por favor verifique la información"; }

			coincidences = emps.Where(e => e.RFC == emp.RFC).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe un empleado registrado con el RFC {emp.RFC}. Por favor verifique la información"; }

			coincidences = emps.Where(e => e.NSS == emp.NSS).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe un empleado registrado con el NSS {emp.NSS}. Por favor verifique la información"; }

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

				if (idEmpleado >= 1)
				{
					//Si el empleado ya existía, lo actualiza.
					await _empleadoManager.UpdateAsync(empleado);

					//Elimina los contactos del empleado.
					await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(idEmpleado);

					//Elimina los archivos del usuario.
					await _archivoEmpleadoManager.DeleteByEmpleadoIdAsync(idEmpleado);
				}
				else
				{
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
				foreach (ArchivoModel? a in e.Archivos)
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
					using(Stream s = Request.Form.Files[0].OpenReadStream())
					{
						using(var reader = ExcelReaderFactory.CreateReader(s)) 
						{
							DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });
							foreach (DataRow row in result.Tables[0].Rows) {
								//Omite el procesamiento del row de encabezado
								if(result.Tables[0].Rows.IndexOf(row) == 0) { continue; }

								string vmsg = await CreateEmployeeFromExcelRow(row);

								//Si la longitud del mensaje de respuesta es mayor o igual a uno, se considera que hubo errores.
								if ((vmsg ?? "").Length >= 1)
								{
									resp.TieneError = true;
									resp.Mensaje = vmsg;
									break;
								}
							}
						}
					}
				}

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadosImportadosSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> CreateEmployeeFromExcelRow(DataRow row)
		{
			string validationMsg = string.Empty;
			Genero? genero = await _generoManager.GetByNameAsync(row[5].ToString() ?? string.Empty);
			EstadoCivil? estadoCivil = await _estadoCivilManager.GetByNameAsync(row[6].ToString() ?? string.Empty);
			Puesto? puesto = await _puestoManager.GetByNameAsync(row[8].ToString() ?? string.Empty);
			Area? area = await _areaManager.GetByNameAsync(row[9].ToString() ?? string.Empty);
			Subarea? subarea = await _subareaManager.GetByNameAsync(row[10].ToString() ?? string.Empty);
			Oficina? oficina = await _oficinaManager.GetByNameAsync(row[11].ToString() ?? string.Empty);
			Empleado? jefe = await _empleadoManager.GetByNameAsync(row[12].ToString() ?? string.Empty);

			DateTime fn;
			DateTime fi;
			DateTime.TryParse(row[3].ToString(), out fn);
			DateTime.TryParse(row[13].ToString(), out fi);

			EmpleadoModel e = new EmpleadoModel() {
				Nombre = row[0].ToString() ?? string.Empty,
				ApellidoPaterno = row[1].ToString() ?? string.Empty,
				ApellidoMaterno = row[2].ToString() ?? string.Empty,
				FechaNacimiento = fn,
				Telefono = row[4].ToString() ?? string.Empty,
				GeneroId = genero != null ? genero.Id : null,
				EstadoCivilId = estadoCivil != null ? estadoCivil.Id : null,
				Direccion = row[7].ToString() ?? string.Empty,
				PuestoId = puesto != null ? puesto.Id : 0,
				AreaId = area != null ? area.Id : 0,
				SubareaId = subarea != null ? subarea.Id : null,
				OficinaId = oficina != null ? oficina.Id : null,
				JefeId = jefe != null ? jefe.Id : null,
				FechaIngreso = fi,
				Email = row[14].ToString() ?? string.Empty,
				NombreContacto1 = row[15].ToString() ?? string.Empty,
				TelefonoContacto1 = row[16].ToString() ?? string.Empty,
				NombreContacto2 = row[17].ToString() ?? string.Empty,
				TelefonoContacto2 = row[18].ToString() ?? string.Empty,
				CURP = row[19].ToString() ?? string.Empty,
				RFC = row[20].ToString() ?? string.Empty,
				NSS = row[21].ToString() ?? string.Empty,
				NombrePreferido = row[22].ToString() ?? string.Empty,
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
	}
}