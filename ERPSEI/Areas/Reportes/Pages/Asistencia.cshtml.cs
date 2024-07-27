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
	public class AsistenciaModel(
		ApplicationDbContext db,
		IEmpleadoManager empleadoManager,
		IAsistenciaManager asistenciaManager,
		IStringLocalizer<AsistenciaModel> stringLocalizer,
		ILogger<AsistenciaModel> logger
		) : PageModel
	{

		[BindProperty]
		public FiltroModel InputFiltro { get; set; } = new FiltroModel();
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
		public Asistencia ListaAsistencia { get; set; } = new Asistencia();
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

		public Task<JsonResult> OnGetAsistenciasList()
		{
			string jsonResponse;
			List<string> jsonAsistencias = [];
			List<Data.Entities.Empleados.Asistencia> asistencias = asistenciaManager.GetAllAsync().Result; 

			foreach (Data.Entities.Empleados.Asistencia asis in asistencias)
			{
				// Construir el JSON para cada asistencia
				string asistenciaJson = $"{{\"id\": {asis.Id}, \"nombre\": \"{asis.NombreEmpleado}\", \"FechaHora\": \"{asis.FechaHora}\", \"Fecha\": \"{asis.Fecha}\", \"Hora\": \"{asis.Hora}\", \"Dirección\": {asis.Direccion}, \"NombreDispositivo\": {asis.NombreDispositivo}, \"SerialDispositivo\": {asis.SerialDispositivo}, \"NoTarjeta\": {asis.NoTarjeta}}}";
				jsonAsistencias.Add(asistenciaJson);
			}

			jsonResponse = $"[{String.Join(",", jsonAsistencias)}]";
			return Task.FromResult(new JsonResult(jsonResponse));
		}
	}
}
