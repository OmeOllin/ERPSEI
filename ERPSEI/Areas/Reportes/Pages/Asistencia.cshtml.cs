using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.SAT;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class AsistenciaModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IEmpleadoManager _empleadoManager;
		private readonly IAsistenciaManager _asistenciaManager;
		private readonly IStringLocalizer<AsistenciaModel> _strLocalizer;
		private readonly ILogger<AsistenciaModel> _logger;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }
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

		[BindProperty]
		public Asistencia ListaAsistencia { get; set; }
		public class Asistencia
		{
			[Display(Name = "Id")]
			public int Id { get; set; }
			public string? Nombre { get; set; }
			public DateOnly? Fecha { get; set; } 
			public TimeOnly? HoraEntrada { get; set; }
			public TimeOnly? HoraSalida { get; set; }
			public int? Retardo { get; set; }
			public int? Total { get; set; }
			public int? Faltas { get; set; }
		}
		public AsistenciaModel(
			ApplicationDbContext db,
			IEmpleadoManager empleadoManager,
			IAsistenciaManager asistenciaManager,
			IStringLocalizer<AsistenciaModel> stringLocalizer,
			ILogger<AsistenciaModel> logger
		)
		{
			_db = db;
			_empleadoManager = empleadoManager;
			_asistenciaManager = asistenciaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
		}

		public async Task<JsonResult> OnGetAsistenciasList()
		{
			string jsonResponse;
			List<string> jsonAsistencias = new List<string>();
			List<Data.Entities.Empleados.Asistencia> asistencias = _asistenciaManager.GetAllAsync().Result; 

			foreach (Data.Entities.Empleados.Asistencia asis in asistencias)
			{
				// Construir el JSON para cada asistencia
				string asistenciaJson = $"{{\"id\": {asis.Id}, \"nombre\": \"{asis.Nombre}\", \"fecha\": \"{asis.Fecha}\", \"horaEntrada\": \"{asis.HoraEntrada}\", \"horaSalida\": \"{asis.HoraSalida}\", \"retardo\": {asis.Retardo}, \"total\": {asis.Total}, \"faltas\": {asis.Faltas}}}";
				jsonAsistencias.Add(asistenciaJson);
			}

			jsonResponse = $"[{String.Join(",", jsonAsistencias)}]";
			return new JsonResult(jsonResponse);
		}

		public async Task<JsonResult> OnPostSaveAsistencia()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["AsistenciaSavedUnsuccessfully"]);

			try
			{
				if (ListaAsistencia.Id <= 0) { ModelState.AddModelError("IdAsistencia", _strLocalizer["IdAsistenciaIsRequired"]); }
				
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Data.Entities.Empleados.Asistencia? existingAsistencia = await _asistenciaManager.GetByIdAsync(ListaAsistencia.Id);

					if (existingAsistencia != null)
					{
						// La asistencia ya existe, actualiza sus propiedades
						existingAsistencia.Nombre = ListaAsistencia.Nombre;
						existingAsistencia.Fecha = ListaAsistencia.Fecha;
						existingAsistencia.HoraEntrada = ListaAsistencia.HoraEntrada;
						existingAsistencia.HoraSalida = ListaAsistencia.HoraSalida;
						existingAsistencia.Retardo = ListaAsistencia.Retardo;
						existingAsistencia.Total = ListaAsistencia.Total;
						existingAsistencia.Faltas = ListaAsistencia.Faltas;

						await _asistenciaManager.UpdateAsync(existingAsistencia);
					}
					else
					{
						// La asistencia es nueva, crea una nueva instancia y guarda
						await _asistenciaManager.CreateAsync(existingAsistencia);
					}

					resp.TieneError = false;
					resp.Mensaje = _strLocalizer["AsistenciaSavedSuccessfully"];
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al guardar la asistencia.");
			}

			return new JsonResult(resp);
		}



		/*public JsonResult OnGetAsistenciaList()
		{
			List<Asistencia> asistencias = _asistenciaManager.GetAllAsync().Result;

			return new JsonResult(asistencias);
		}*/

		/*public async Task<JsonResult> OnPostDeleteAsistencias(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["AttendancesDeletedUnsuccessfully"]);
			try
			{
				await _db.Database.BeginTransactionAsync();

				List<Asistencia> asistencias = await _asistenciaManager.GetAllAsync();
				foreach (string id in ids)
				{
					int sid = 0;
					if (!int.TryParse(id, out sid)) { sid = 0; }
					Asistencia? asistencia = asistencias.FirstOrDefault(a => a.Id == sid);
					if (asistencia != null)
					{
						List<Empleado> empleadosRelacionados = await _empleadoManager.GetAllAsync(
							null, null, null, null, null, null, null, null, true
						);

						// Si existen empleados relacionados a la asistencia, se le notifica al usuario.
						if (empleadosRelacionados.Any(e => e.Id == asistencia.Id))
						{
							Empleado empleado = empleadosRelacionados.First(e => e.Id == asistencia.Id);
							resp.TieneError = true;
							resp.Mensaje = $"{_strLocalizer["AttendanceIsRelated"]}<br/><br/><i>{asistencia.Nombre}</i><br/><br/><i>{empleado.Id} - {empleado.NombreCompleto}</i>";
							break;
						}
						else
						{
							// En caso de no haber empleados relacionados, procede a eliminar el registro de asistencia.
							await _asistenciaManager.DeleteByIdAsync(sid);

							resp.TieneError = false;
							resp.Mensaje = _strLocalizer["AttendancesDeletedSuccessfully"];
						}
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
		}*/

		/*private async Task<string> GetAsistenciatList(FiltroModel? filtro = null)
		{
			string nombreEmpleado;
			string jsonResponse;
			List<string> jsonEmpleados = new List<string>();
			List<Asistencia> empleados;

			if (filtro != null)
			{
				empleados = await _empleadoManager.GetAllAsync(
					filtro.NombreEmpleado,
					filtro.FechaIngresoInicio,
					filtro.FechaIngresoFin
				);
			}
			else 
			{ 
			
			}
		}*/
	}
}
