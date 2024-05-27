using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class AsistenciaModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IEmpleadoManager _empleadoManager;
		private readonly IRWCatalogoManager<Puesto> _puestoManager;
		private readonly IStringLocalizer<PuestosModel> _strLocalizer;
		private readonly ILogger<PuestosModel> _logger;

		[BindProperty]
		public InputModel Input { get; set; }
		public FiltroModel InputFiltro { get; set; }
		public Asistencia ListaAsistencia { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;
		}
		public class FiltroModel
		{
			[DataType(DataType.Text)]
			[Display(Name = "EmployeeNameField")]
			public string? NombreEmpleado { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaInicioField")]
			public DateTime? FechaIngresoInicio { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaFinField")]
			public DateTime? FechaIngresoFin { get; set; }
		}
		public class Asistencia
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[Display(Name = "EmployeeNameField")]
			public string? Nombre { get; set; }

			[DataType(DataType.Date)]
			[Display(Name = "FechaInicioField")]
			public DateTime? Fecha { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "HoraEntradaField")]
			public DateTime? HoraEntrada { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "HoraSalidaField")]
			public DateTime? HoraSalida { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "RetardoField")]
			public string? Retardo { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "TotalField")]
			public string? Total { get; set; }

			[DataType(DataType.Text)]
			[Display(Name = "FaltasField")]
			public string? Faltas { get; set; }
		}
		public AsistenciaModel(
			ApplicationDbContext db,
			IEmpleadoManager empleadoManager,
			IRWCatalogoManager<Puesto> puestoManager,
			IStringLocalizer<PuestosModel> stringLocalizer,
			ILogger<PuestosModel> logger
		)
		{
			Input = new InputModel();
			_db = db;
			_empleadoManager = empleadoManager;
			_puestoManager = puestoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
		}

		public JsonResult OnGetPuestosList()
		{
			List<Puesto> puestos = _puestoManager.GetAllAsync().Result;

			return new JsonResult(puestos);
		}

		public async Task<JsonResult> OnPostDeleteAsistencias(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PositionsDeletedUnsuccessfully"]);
			try
			{
				await _db.Database.BeginTransactionAsync();

				List<Puesto> puestos = await _puestoManager.GetAllAsync();
				foreach (string id in ids)
				{
					int sid = 0;
					if (!int.TryParse(id, out sid)) { sid = 0; }
					Puesto? puesto = puestos.Where(p => p.Id == sid).FirstOrDefault();
					List<Empleado> empleados = await _empleadoManager.GetAllAsync(null, null, null, null, sid, null, null, null, true);
					List<Empleado> empleadosActivosRelacionados = empleados.Where(e => e.Deshabilitado == 0).ToList();
					//Si existen empleados que tengan el registro asignado, se le notifica al usuario.
					if (empleadosActivosRelacionados.Count() > 0)
					{
						List<string> names = new List<string>();
						foreach (Empleado e in empleadosActivosRelacionados) { names.Add($"<i>{e.Id} - {e.NombreCompleto}</i>"); }
						resp.TieneError = true;
						resp.Mensaje = $"{_strLocalizer["PositionIsRelated"]}<br/><br/><i>{puesto?.Nombre}</i><br/><br/>{string.Join("<br/>", names)}";
						break;
					}
					else
					{
						//En caso de no haber empleados con el registro asignado, procede a eliminar referencias y registro.
						foreach (Empleado e in empleados)
						{
							e.PuestoId = null;
							await _empleadoManager.UpdateAsync(e);
						}
						await _puestoManager.DeleteByIdAsync(sid);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["PositionsDeletedSuccessfully"];
					}
				}

				if (resp.TieneError) { throw new Exception(resp.Mensaje); }

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				await _db.Database.RollbackTransactionAsync();
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSavePuesto()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PositionSavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Puesto? puesto = await _puestoManager.GetByNameAsync(Input.Nombre);

					if (puesto != null && puesto.Id != Input.Id)
					{
						resp.Mensaje = _strLocalizer["ErrorPuestoExistente"];
					}
					else
					{
						int id = 0;
						puesto = await _puestoManager.GetByIdAsync(Input.Id);

						if (puesto != null) { id = puesto.Id; } else { puesto = new Puesto(); }

						puesto.Nombre = Input.Nombre;

						if (id >= 1)
						{
							await _puestoManager.UpdateAsync(puesto);
						}
						else
						{
							await _puestoManager.CreateAsync(puesto);

						}

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
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
