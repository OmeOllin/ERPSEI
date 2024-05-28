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

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class AsistenciaModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly IEmpleadoManager _empleadoManager;
		private readonly IRCatalogoManager<Asistencias> _asistenciaManager;
		private readonly IStringLocalizer<AsistenciaModel> _strLocalizer;
		private readonly ILogger<AsistenciaModel> _logger;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }
		public Asistencia ListaAsistencia { get; set; }

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
			public int? Id { get; set; }
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
			//IRCatalogoManager<Asistencias> asistenciaManager,
			IStringLocalizer<AsistenciaModel> stringLocalizer,
			ILogger<AsistenciaModel> logger
		)
		{
			_db = db;
			_empleadoManager = empleadoManager;
			//_asistenciaManager = asistenciaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;

			//InputFiltro = new FiltroModel();
			//ListaAsistencia = new Asistencia();
		}

		/*public JsonResult OnGetAsistenciaList()
		{
			List<Asistencia> asistencias = _asistenciaManager.GetAllAsistenciaAsync().Result;

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

	}
}
