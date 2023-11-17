using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
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

			[Required(ErrorMessage = "Required")]
			[StringLength(30, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

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
			[Required(ErrorMessage = "Required")]
			[Display(Name = "PhoneNumberField")]
			public string Telefono { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoEmpresarialField")]
			public string Email { get; set; } = string.Empty;

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
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto2 { get; set; } = string.Empty;
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
				List<string> jsonContactosEmergencia = new List<string>();
				List<string> jsonArchivos = new List<string>();

				nombreArea = e.Area != null ? e.Area.Nombre : "";
				nombreSubarea = e.Subarea != null ? e.Subarea.Nombre : "";
				nombrePuesto = e.Puesto != null ? e.Puesto.Nombre : "";
				nombreOficina = e.Oficina != null ? e.Oficina.Nombre : "";
				nombreGenero = e.Genero != null ? e.Genero.Nombre : "";
				nombreEstadoCivil = e.EstadoCivil != null ? e.EstadoCivil.Nombre : "";
				nombreJefe = e.Jefe != null ? e.Jefe.NombreCompleto : "";

				if (e.ContactosEmergencia != null)
				{
					List<ContactoEmergencia> contactos = (from c in e.ContactosEmergencia
														  orderby c.Id ascending
														  select c).ToList();
					foreach (ContactoEmergencia c in contactos)
					{
						jsonContactosEmergencia.Add($"{{\"nombre\": \"{c.Nombre}\", \"telefono\": \"{c.Telefono}\"}}");
					}
				}

				if (e.ArchivosEmpleado != null)
				{
					//Si el usuario ya tiene archivos, se llena el arreglo de datos a partir de ellos.					
					List<ArchivoEmpleado> userFiles = (from userFile in e.ArchivosEmpleado
													   orderby userFile.TipoArchivoId ascending
													   select userFile).ToList();

					foreach (ArchivoEmpleado a in userFiles)
					{
						string file = "";
						//Si el archivo tiene contenido
						if (a.Archivo != null && a.Archivo.Length >= 1)
						{
							//Asigna la información del archivo al arreglo de datos.
							string b64 = Convert.ToBase64String(a.Archivo);
							string imgSrc = $"data:image/png;base64,{b64}";
							string id = Guid.NewGuid().ToString();

							if (a.Extension == "pdf")
							{
								file = $"<canvas id = '{id}' b64 = '{b64}' class = 'canvaspdf'></canvas>";
							}
							else
							{
								file = $"<img id = '{id}' src = '{imgSrc}' style='min-height: 200px;'/>";
							}
						}

						jsonArchivos.Add(
							"{" +
								$"\"id\": {a.Id}," +
								$"\"nombre\": \"{a.Nombre}\"," +
								$"\"tipoArchivoId\": {a.TipoArchivoId}," +
								$"\"extension\": \"{a.Extension}\"," +
								$"\"archivo\": \"{file}\"" +
							"}"
						);
					}
				}
				else
				{
					//Si el usuario no tiene archivos, se llena el arreglo de datos a partir del enum.
					foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
					{
						//Omite el tipo imagen de perfil.
						if ((int)i == 0) { continue; }
						jsonArchivos.Add(
							"{" +
								$"\"id\": \"{Guid.NewGuid()}\"," +
								$"\"nombre\": \"\"," +
								$"\"tipoArchivoId\": {(int)i}," +
								$"\"extension\": \"\"," +
								$"\"archivo\": \"\"" +
							"}"
						);
					}
				}

				jsonEmpleados.Add(
					"{" +
						$"\"id\": {e.Id}," +
						$"\"nombre\": \"{e.Nombre}\", " +
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

		public IActionResult OnGet()
		{

			return Page();
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
			try
			{
				await _empleadoManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadosDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		public async Task<JsonResult> OnPostSaveEmpleado()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpleadoSavedUnsuccessfully"]);

			if (!ModelState.IsValid)
			{
				resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				return new JsonResult(resp);
			}

			await _db.Database.BeginTransactionAsync();
			try
			{
				Empleado? empleado = await _empleadoManager.GetByIdAsync(InputEmpleado.Id);
				int idEmpleado = 0;
				if (empleado != null)
				{
					idEmpleado = empleado.Id;

					empleado.ApellidoMaterno = InputEmpleado.ApellidoMaterno;
					empleado.ApellidoPaterno = InputEmpleado.ApellidoPaterno;
					empleado.AreaId = InputEmpleado.AreaId;
					empleado.Direccion = InputEmpleado.Direccion ?? "";
					empleado.Email = InputEmpleado.Email;
					empleado.EstadoCivilId = InputEmpleado.EstadoCivilId;
					empleado.FechaIngreso = InputEmpleado.FechaIngreso;
					empleado.FechaNacimiento = InputEmpleado.FechaNacimiento;
					empleado.GeneroId = InputEmpleado.GeneroId;
					empleado.JefeId = InputEmpleado.JefeId;
					empleado.NombreCompleto = $"{InputEmpleado.Nombre} {InputEmpleado.ApellidoPaterno} {InputEmpleado.ApellidoMaterno}";
					empleado.OficinaId = InputEmpleado.OficinaId;
					empleado.Nombre = InputEmpleado.Nombre;
					empleado.PuestoId = InputEmpleado.PuestoId;
					empleado.SubareaId = InputEmpleado.SubareaId;
					empleado.Telefono = InputEmpleado.Telefono;

					await _empleadoManager.UpdateAsync(empleado);

					//Elimina los contactos del empleado.
					await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(empleado.Id);
				}
				else
				{
					//Crea al empleado y obtiene su id.
					idEmpleado = await _empleadoManager.CreateAsync(new Empleado() {
																		ApellidoMaterno = InputEmpleado.ApellidoMaterno,
																		ApellidoPaterno = InputEmpleado.ApellidoPaterno,
																		AreaId = InputEmpleado.AreaId,
																		Direccion = InputEmpleado.Direccion ?? "",
																		Email = InputEmpleado.Email,
																		EstadoCivilId = InputEmpleado.EstadoCivilId,
																		FechaIngreso = InputEmpleado.FechaIngreso,
																		FechaNacimiento = InputEmpleado.FechaNacimiento,
																		GeneroId = InputEmpleado.GeneroId,
																		JefeId = InputEmpleado.JefeId,
																		NombreCompleto = $"{InputEmpleado.Nombre} {InputEmpleado.ApellidoPaterno} {InputEmpleado.ApellidoMaterno}",
																		OficinaId = InputEmpleado.OficinaId,
																		Nombre = InputEmpleado.Nombre,
																		PuestoId = InputEmpleado.PuestoId,
																		SubareaId = InputEmpleado.SubareaId,
																		Telefono = InputEmpleado.Telefono
																	});
				}

				//Crea dos nuevos contactos para el empleado.
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto1 ?? "", Telefono = InputEmpleado.TelefonoContacto1 ?? "", EmpleadoId = idEmpleado }
				);
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto2 ?? "", Telefono = InputEmpleado.TelefonoContacto2 ?? "", EmpleadoId = idEmpleado }
				);

				await _db.Database.CommitTransactionAsync();

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpleadoSavedSuccessfully"];
			}
			catch (Exception ex)
			{
				await _db.Database.RollbackTransactionAsync();
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
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

								await CreateEmployeeFromExcelRow(row);
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
		private async Task CreateEmployeeFromExcelRow(DataRow row)
		{
			await _db.Database.BeginTransactionAsync();
			try
			{
				//Se busca el empleado por su nombre completo.
				Empleado? empleado = await _empleadoManager.GetByCURPAsync(row[19].ToString() ?? "");
				int idEmpleado = 0;
				Area? area = await _areaManager.GetByNameAsync(row[9].ToString() ?? "");
				EstadoCivil? estadoCivil = await _estadoCivilManager.GetByNameAsync(row[6].ToString() ?? "");
				Genero? genero = await _generoManager.GetByNameAsync(row[5].ToString() ?? "");
				Empleado? jefe = await _empleadoManager.GetByNameAsync(row[12].ToString() ?? "");
				Oficina? oficina = await _oficinaManager.GetByNameAsync(row[11].ToString() ?? "");
				Puesto? puesto = await _puestoManager.GetByNameAsync(row[8].ToString() ?? "");
				Subarea? subarea = await _subareaManager.GetByNameAsync(row[10].ToString() ?? "");

				if (empleado != null)
				{
					idEmpleado = empleado.Id;

					empleado.ApellidoMaterno = row[2].ToString() ?? "";
					empleado.ApellidoPaterno = row[1].ToString() ?? "";
					empleado.AreaId = area != null ? area.Id : null;
					empleado.Direccion = row[7].ToString() ?? "";
					empleado.Email = row[14].ToString() ?? "";
					empleado.EstadoCivilId = estadoCivil != null ? estadoCivil.Id : null;
					empleado.FechaIngreso = DateTime.Parse(row[13].ToString() ?? "");
					empleado.FechaNacimiento = DateTime.Parse(row[3].ToString() ?? "");
					empleado.GeneroId = genero != null ? genero.Id : null;
					empleado.JefeId = jefe != null ? jefe.Id : null;
					empleado.NombreCompleto = $"{row[0].ToString() ?? ""} {row[1].ToString() ?? ""} {row[2].ToString() ?? ""}";
					empleado.OficinaId = oficina != null ? oficina.Id : null;
					empleado.Nombre = row[0].ToString() ?? "";
					empleado.PuestoId = puesto != null ? puesto.Id : null;
					empleado.SubareaId = subarea != null ? subarea.Id : null;
					empleado.Telefono = row[4].ToString() ?? "";
					empleado.CURP = row[19].ToString() ?? "";
					empleado.RFC = row[20].ToString() ?? "";
					empleado.NSS = row[21].ToString() ?? "";

					await _empleadoManager.UpdateAsync(empleado);

					//Elimina los contactos del empleado.
					await _contactoEmergenciaManager.DeleteByEmpleadoIdAsync(empleado.Id);
				}
				else 
				{
					//Crea al empleado y obtiene su id.
					idEmpleado = await _empleadoManager.CreateAsync(new Empleado()
					{
						ApellidoMaterno = row[2].ToString() ?? "",
						ApellidoPaterno = row[1].ToString() ?? "",
						AreaId = area != null ? area.Id : null,
						Direccion = row[7].ToString() ?? "",
						Email = row[14].ToString() ?? "",
						EstadoCivilId = estadoCivil != null ? estadoCivil.Id : null,
						FechaIngreso = DateTime.Parse(row[13].ToString() ?? ""),
						FechaNacimiento = DateTime.Parse(row[3].ToString() ?? ""),
						GeneroId = genero != null ? genero.Id : null,
						JefeId = jefe != null ? jefe.Id : null,
						NombreCompleto = $"{row[0].ToString() ?? ""} {row[1].ToString() ?? ""} {row[2].ToString() ?? ""}",
						OficinaId = oficina != null ? oficina.Id : null,
						Nombre = row[0].ToString() ?? "",
						PuestoId = puesto != null ? puesto.Id : null,
						SubareaId = subarea != null ? subarea.Id : null,
						Telefono = row[4].ToString() ?? "",
						CURP = row[19].ToString() ?? "",
						RFC = row[20].ToString() ?? "",
						NSS = row[21].ToString() ?? ""
					});
				}

				//Crea dos nuevos contactos para el empleado.
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = row[15].ToString() ?? "", Telefono = row[16].ToString() ?? "", EmpleadoId = idEmpleado }
				);
				await _contactoEmergenciaManager.CreateAsync(
					new ContactoEmergencia() { Nombre = row[17].ToString() ?? "", Telefono = row[18].ToString() ?? "", EmpleadoId = idEmpleado }
				);

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await _db.Database.RollbackTransactionAsync();
				throw;
			}
		}
	}
}
