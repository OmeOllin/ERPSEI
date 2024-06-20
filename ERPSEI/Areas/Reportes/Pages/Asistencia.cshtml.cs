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
	[Authorize(Policy = "AccessPolicy")]
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
			public DateTime? FechaHora { get; set; }
			public DateOnly? Fecha { get; set; }
			public TimeSpan? Hora { get; set; }
			public string? Direccion { get; set; }
			public string? NombreDispositivo { get; set; }
			public string? SerialDispositivo { get; set; }
			public string? NombreEmpleado { get; set; }
			public int? NoTarjeta { get; set; }
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
				string asistenciaJson = $"{{\"id\": {asis.Id}, \"nombre\": \"{asis.NombreEmpleado}\", \"FechaHora\": \"{asis.FechaHora}\", \"Fecha\": \"{asis.Fecha}\", \"Hora\": \"{asis.Hora}\", \"Dirección\": {asis.Direccion}, \"NombreDispositivo\": {asis.NombreDispositivo}, \"SerialDispositivo\": {asis.SerialDispositivo}, \"NoTarjeta\": {asis.NoTarjeta}}}";
				jsonAsistencias.Add(asistenciaJson);
			}

			jsonResponse = $"[{String.Join(",", jsonAsistencias)}]";
			return new JsonResult(jsonResponse);
		}
	}
}
