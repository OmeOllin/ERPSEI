using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.SAT;
using ERPSEI.Requests;
using ERPSEI.Resources;
using ExcelDataReader.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
			public string Id { get; set; } = string.Empty;
			public DateTime FechaHora { get; set; }
			public DateOnly Fecha { get; set; }
			public TimeSpan Hora { get; set; }
			public string Direccion { get; set; } = string.Empty;
			public string NombreDispositivo { get; set; } = string.Empty;
			public string SerialDispositivo { get; set; } = string.Empty;
			public string NombreEmpleado { get; set; } = string.Empty;
			public string NoTarjeta { get; set; } = string.Empty;
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
			string jsonResponse = string.Empty;
			List<Data.Entities.Empleados.Asistencia> asistencias = await _asistenciaManager.GetAllAsync();
			List<string> jsonAsistencias = new List<string>();
			

			foreach (Data.Entities.Empleados.Asistencia asis in asistencias)
			{
				// Construir el JSON para cada asistencia
				jsonAsistencias.Add("{" +
					$"\"Id\": \"{asis.Id}\", " +
					$"\"NombreEmpleado\": \"{asis.NombreEmpleado}\", " +
					$"\"FechaHora\": \"{asis.FechaHora}\", " +
					$"\"Fecha\": \"{asis.Fecha}\", " +
					$"\"Hora\": \"{asis.Hora}\", " +
					$"\"Direccion\": \"{asis.Direccion}\", " +
					$"\"NombreDispositivo\": \"{asis.NombreDispositivo}\", " +
					$"\"SerialDispositivo\": \"{asis.SerialDispositivo}\" " +
					"}");
			}
			jsonResponse = $"[{String.Join(",", jsonAsistencias)}]";
			return new JsonResult(jsonResponse);
		}

		public async Task<JsonResult> OnPostFiltrarAsistencia([FromBody] FiltroModel inputFiltro)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["AsistenciasFiltradosUnsuccessfully"]);

			if (inputFiltro == null)
			{
				resp.Mensaje = _strLocalizer["Favor de seleccionar rango de fechas"];
				return new JsonResult(resp);
			}

			try
			{
				// Obtener todas las asistencias
				List<Data.Entities.Empleados.Asistencia> asistencias = await _asistenciaManager.GetAllAsync();

				// Aplicar filtros secuencialmente
				if (!string.IsNullOrEmpty(inputFiltro.NombreEmpleado))
				{
					asistencias = asistencias.Where(a => a.NombreEmpleado.Contains(inputFiltro.NombreEmpleado, StringComparison.OrdinalIgnoreCase)).ToList();
				}

				if (inputFiltro.FechaIngresoInicio.HasValue)
				{
					asistencias = asistencias.Where(a => a.FechaHora >= inputFiltro.FechaIngresoInicio.Value.Date).ToList();
				}

				if (inputFiltro.FechaIngresoFin.HasValue)
				{
					DateTime fechaFin = inputFiltro.FechaIngresoFin.Value.Date.AddDays(1).AddTicks(-1);
					asistencias = asistencias.Where(a => a.FechaHora <= fechaFin).ToList();
				}

				// Verificar si se encontraron resultados
				if (asistencias.Count == 0)
				{
					resp.Mensaje = _strLocalizer["NoRecordsFound"];
					return new JsonResult(resp);
				}


				// Construir la respuesta JSON
				List<string> jsonAsistencias = new List<string>();
				foreach (var asis in asistencias)
				{
					jsonAsistencias.Add("{" +
						$"\"Id\": \"{asis.Id}\", " +
						$"\"NombreEmpleado\": \"{asis.NombreEmpleado}\", " +
						$"\"FechaHora\": \"{asis.FechaHora}\", " +
						$"\"Fecha\": \"{asis.Fecha}\", " +
						$"\"Hora\": \"{asis.Hora}\", " +
						$"\"Direccion\": \"{asis.Direccion}\", " +
						$"\"NombreDispositivo\": \"{asis.NombreDispositivo}\", " +
						$"\"SerialDispositivo\": \"{asis.SerialDispositivo}\" " +
						"}");
				}

				string jsonResponse = $"[{String.Join(",", jsonAsistencias)}]";
				resp.Datos = jsonResponse;
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["AsistenciasFiltradosSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}


	}
}
