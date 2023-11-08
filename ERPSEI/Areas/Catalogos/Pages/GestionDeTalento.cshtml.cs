using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
	public class GestionDeTalentoModel : PageModel
    {
		private readonly IRWCatalogoManager<Empleado> _empleadoManager;
		private readonly IRWCatalogoManager<Area> _areaManager;
		private readonly IRWCatalogoManager<Subarea> _subareaManager;
		private readonly IRWCatalogoManager<Puesto> _puestoManager;
		private readonly IRWCatalogoManager<Oficina> _oficinaManager;

		private readonly IRCatalogoManager<Genero> _generoManager;
		private readonly IRCatalogoManager<EstadoCivil> _estadoCivilManager;
		private readonly IContactoEmergenciaManager _contactoEmergenciaManager;
		private readonly IStringLocalizer<GestionDeTalentoModel> _strLocalizer;
		private readonly ILogger<GestionDeTalentoModel> _logger;

		[BindProperty]
        public FiltroModel InputFiltro { get; set; }

        public class FiltroModel
        {
			[DataType(DataType.DateTime)]
			[Display(Name = "FechaIngresoInicioField")]
			public string FechaIngresoInicio { get; set; } = string.Empty;

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaIngresoFinField")]
			public string FechaIngresoFin { get; set; } = string.Empty;

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaNacimientoInicioField")]
			public string FechaNacimientoInicio { get; set; } = string.Empty;

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaNacimientoFinField")]
			public string FechaNacimientoFin { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[Display(Name = "PuestoField")]
			public int PuestoId { get; set; }

            [Display(Name = "AreaField")]
            public int AreaId { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "CURPField")]
			public string CURP { get; set; } = string.Empty;

			[Display(Name = "SubareaField")]
			public int SubareaId { get; set; }

			[Display(Name = "OficinaField")]
			public int OficinaId { get; set; }
		}

        [BindProperty]
        public EmpleadoModel InputEmpleado { get; set; }

        public class EmpleadoModel
        {
			public int Id { get; set; }

			[Required(ErrorMessage = "Required")]
			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "FirstNameField")]
			public string PrimerNombre { get; set; } = string.Empty;

			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[DataType(DataType.Text)]
			[Display(Name = "SecondNameField")]
			public string? SegundoNombre { get; set; } = string.Empty;

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

			[Required(ErrorMessage = "Required")]
			[Display(Name = "SubareaField")]
			public int SubareaId { get; set; }

			[Display(Name = "OficinaField")]
			public int? OficinaId { get; set; }

			[Display(Name = "JefeField")]
			public int? JefeId { get; set; }

			[DataType(DataType.Text)]
			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Display(Name = "NameField")]
			public string? NombreContacto1 { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto1 { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(15, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Display(Name = "NameField")]
			public string? NombreContacto2 { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[Display(Name = "PhoneNumberField")]
			public string? TelefonoContacto2 { get; set; } = string.Empty;
		}

        public GestionDeTalentoModel(
			IRWCatalogoManager<Empleado> empleadoManager,
			IRWCatalogoManager<Area> areaManager,
			IRWCatalogoManager<Subarea> subareaManager,
			IRWCatalogoManager<Puesto> puestoManager,
			IRWCatalogoManager<Oficina> oficinaManager,
			IRCatalogoManager<Genero> generoManager,
			IRCatalogoManager<EstadoCivil> estadoCivilManager,
			IContactoEmergenciaManager contactoEmergenciaManager,
			IStringLocalizer<GestionDeTalentoModel> stringLocalizer,
			ILogger<GestionDeTalentoModel> logger
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

            InputFiltro = new FiltroModel();
            InputEmpleado = new EmpleadoModel();
        }

        public IActionResult OnGet()
        {
            return Page();
		}

        public JsonResult OnGetTalentList()
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
			List<string> jsonContactosEmergencia = new List<string>();
			List<Empleado> empleados = _empleadoManager.GetAllAsync().Result;

			foreach (Empleado e in empleados)
			{
				nombreArea = e.Area != null ? e.Area.Nombre : "";
				nombreSubarea = e.Subarea != null ? e.Subarea.Nombre : "";
				nombrePuesto = e.Puesto != null ? e.Puesto.Nombre : "";
				nombreOficina = e.Oficina != null ? e.Oficina.Nombre : "";
				nombreGenero = e.Genero != null ? e.Genero.Nombre : "";
				nombreEstadoCivil = e.EstadoCivil != null ? e.EstadoCivil.Nombre : "";
				nombreJefe = e.Jefe != null ? e.Jefe.NombreCompleto : "";

				if (e.ContactosEmergencia != null)
				{
					foreach (ContactoEmergencia c in e.ContactosEmergencia)
					{
						jsonContactosEmergencia.Add($"{{\"nombre\": \"{c.Nombre}\", \"telefono\": \"{c.Telefono}\"}}");   
					}
				}

				jsonEmpleados.Add(
					"{" +
						$"\"id\": {e.Id}," +
						$"\"primerNombre\": \"{e.PrimerNombre}\", " +
						$"\"segundoNombre\": \"{e.SegundoNombre}\", " +
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
						$"\"idGenero\": {e.GeneroId ?? 0}, " +
						$"\"genero\": \"{nombreGenero}\", " +
						$"\"idSubarea\": {e.SubareaId ?? 0}, " +
						$"\"subarea\": \"{nombreSubarea}\", " +
						$"\"idOficina\": {e.OficinaId ?? 0}, " +
						$"\"oficina\": \"{nombreOficina}\", " +
						$"\"idPuesto\": {e.PuestoId ?? 0}, " +
						$"\"puesto\": \"{nombrePuesto}\", " +
						$"\"idArea\": {e.AreaId ?? 0}, " +
						$"\"area\": \"{nombreArea}\", " +
						$"\"idEstadoCivil\": {e.EstadoCivilId ?? 0}, " +
						$"\"estadoCivil\": \"{nombreEstadoCivil}\", " +
						$"\"idJefe\": {e.JefeId ?? 0}, " +
						$"\"jefe\": \"{nombreJefe}\", " +
						$"\"ContactosEmergencia\": [{string.Join(",", jsonContactosEmergencia)}] " +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonEmpleados)}]";
			return new JsonResult(jsonResponse);
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

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Empleado? empleado = _empleadoManager.GetById(InputEmpleado.Id);

					if (empleado != null)
					{
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
						empleado.NombreCompleto = $"{InputEmpleado.PrimerNombre} {InputEmpleado.SegundoNombre} {InputEmpleado.ApellidoPaterno} {InputEmpleado.ApellidoMaterno}";
						empleado.OficinaId = InputEmpleado.OficinaId;
						empleado.PrimerNombre = InputEmpleado.PrimerNombre;
						empleado.PuestoId = InputEmpleado.PuestoId;
						empleado.SegundoNombre = InputEmpleado.SegundoNombre ?? "";
						empleado.SubareaId = InputEmpleado.SubareaId;
						empleado.Telefono = InputEmpleado.Telefono;

						await _empleadoManager.UpdateAsync(empleado);

						//Elimina los contactos del empleado.
						ICollection<ContactoEmergencia>? contactos = empleado.ContactosEmergencia;
						if (contactos != null)
						{
                            foreach (ContactoEmergencia c in contactos){ await _contactoEmergenciaManager.DeleteAsync(c); }
						}

						//Crea dos nuevos contactos para el empleado.
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto1 ?? "", Telefono = InputEmpleado.TelefonoContacto1 ?? "", EmpleadoId = empleado.Id }
						);
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto2 ?? "", Telefono = InputEmpleado.TelefonoContacto2 ?? "", EmpleadoId = empleado.Id }
						);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["EmpleadoSavedSuccessfully"];
					}
					else
					{
						//Crea al empleado y obtiene su id.
						int idEmpleado = await _empleadoManager.CreateAsync(new Empleado() {
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
											NombreCompleto = $"{InputEmpleado.PrimerNombre} {InputEmpleado.SegundoNombre} {InputEmpleado.ApellidoPaterno} {InputEmpleado.ApellidoMaterno}",
											OficinaId = InputEmpleado.OficinaId,
											PrimerNombre = InputEmpleado.PrimerNombre,
											PuestoId = InputEmpleado.PuestoId,
											SegundoNombre = InputEmpleado.SegundoNombre ?? "",
											SubareaId = InputEmpleado.SubareaId,
											Telefono = InputEmpleado.Telefono
										});

						//Crea dos nuevos contactos para el empleado.
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto1 ?? "", Telefono = InputEmpleado.TelefonoContacto1	?? "", EmpleadoId = idEmpleado }
						);
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto2 ?? "", Telefono = InputEmpleado.TelefonoContacto2 ?? "", EmpleadoId = idEmpleado }
						);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["EmpleadoSavedSuccessfully"];
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
