using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
	public class GestionDeTalentoModel : PageModel
    {
		private readonly IRWCatalogoManager<Empleado> _empleadoManager;
		private readonly IRWCatalogoManager<ContactoEmergencia> _contactoEmergenciaManager;
		private readonly IStringLocalizer<GestionDeTalentoModel> _strLocalizer;
		private readonly ILogger<GestionDeTalentoModel> _logger;

		[BindProperty]
        public FiltroModel InputFiltro { get; set; }

        public class FiltroModel
        {
			[Display(Name = "FechaIngresoInicioField")]
			public string FechaIngresoInicio { get; set; } = string.Empty;

			[Display(Name = "FechaIngresoFinField")]
			public string FechaIngresoFin { get; set; } = string.Empty;

            [Display(Name = "FechaNacimientoInicioField")]
			public string FechaNacimientoInicio { get; set; } = string.Empty;

            [Display(Name = "FechaNacimientoFinField")]
			public string FechaNacimientoFin { get; set; } = string.Empty;

			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[Display(Name = "PuestoField")]
			public string Puesto { get; set; } = string.Empty;

            [Display(Name = "AreaField")]
            public string Area { get; set; } = string.Empty;

			[Display(Name = "CURPField")]
			public string CURP { get; set; } = string.Empty;

			[Display(Name = "SubareaField")]
			public string Subarea { get; set; } = string.Empty;

			[Display(Name = "OficinaField")]
			public string Oficina { get; set; } = string.Empty;
		}

        [BindProperty]
        public EmpleadoModel InputEmpleado { get; set; }

        public class EmpleadoModel
        {
			public int Id { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FirstNameField")]
			public string PrimerNombre { get; set; } = string.Empty;

			[Display(Name = "SecondNameField")]
			public string SegundoNombre { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FathersLastNameField")]
			public string ApellidoPaterno { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "MothersLastNameField")]
			public string ApellidoMaterno { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FechaNacimientoField")]
			public DateTime FechaNacimiento { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FechaIngresoField")]
			public DateTime FechaIngreso { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "DireccionField")]
			public string Direccion { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "PhoneNumberField")]
			public string Telefono { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoEmpresarialField")]
			public string Email { get; set; } = string.Empty;

			[Display(Name = "GeneroField")]
			public int GeneroId { get; set; }

			[Display(Name = "EstadoCivilField")]
			public int EstadoCivilId { get; set; }

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
			public int OficinaId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "JefeField")]
			public int JefeId { get; set; }

			[Display(Name = "NameField")]
			public string NombreContacto1 { get; set; } = string.Empty;

			[Display(Name = "PhoneNumberField")]
			public string TelefonoContacto1 { get; set; } = string.Empty;

			[Display(Name = "NameField")]
			public string NombreContacto2 { get; set; } = string.Empty;

			[Display(Name = "PhoneNumberField")]
			public string TelefonoContacto2 { get; set; } = string.Empty;
		}

        public GestionDeTalentoModel(
			IRWCatalogoManager<Empleado> empleadoManager,
			IRWCatalogoManager<ContactoEmergencia> contactoEmergenciaManager,
			IStringLocalizer<GestionDeTalentoModel> stringLocalizer,
			ILogger<GestionDeTalentoModel> logger
		)
		{
			_empleadoManager = empleadoManager;
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
			List<Empleado> empleados = _empleadoManager.GetAllAsync().Result;

			return new JsonResult(empleados);
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
						empleado.Direccion = InputEmpleado.Direccion;
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
						empleado.SegundoNombre = InputEmpleado.SegundoNombre;
						empleado.SubareaId = InputEmpleado.SubareaId;
						empleado.Telefono = InputEmpleado.Telefono;

						await _empleadoManager.UpdateAsync(empleado);

						//Elimina los contactos del empleado.
						ICollection<ContactoEmergencia>? contactos = empleado.ContactosEmergencia;
						List<string> idsContactos = new List<string>();
						if (contactos != null && contactos.Count() > 0)
						{
							foreach (ContactoEmergencia contacto in contactos) { idsContactos.Add(contacto.Id.ToString()); }
							await _contactoEmergenciaManager.DeleteMultipleByIdAsync(idsContactos.ToArray());
						}

						//Crea dos nuevos contactos para el empleado.
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto1, Telefono = InputEmpleado.TelefonoContacto1, EmpleadoId = empleado.Id }
						);
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto2, Telefono = InputEmpleado.TelefonoContacto2, EmpleadoId = empleado.Id }
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
											Direccion = InputEmpleado.Direccion,
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
											SegundoNombre = InputEmpleado.SegundoNombre,
											SubareaId = InputEmpleado.SubareaId,
											Telefono = InputEmpleado.Telefono
										});

						//Crea dos nuevos contactos para el empleado.
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto1, Telefono = InputEmpleado.TelefonoContacto1, EmpleadoId = idEmpleado }
						);
						await _contactoEmergenciaManager.CreateAsync(
							new ContactoEmergencia() { Nombre = InputEmpleado.NombreContacto2, Telefono = InputEmpleado.TelefonoContacto2, EmpleadoId = idEmpleado }
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
