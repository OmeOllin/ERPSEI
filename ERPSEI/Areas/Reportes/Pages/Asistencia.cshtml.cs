using ERPSEI.Data;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

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

		public async Task<JsonResult> OnGetAsistenciasList()
		{ 
			string jsonResponse;
			List<string> jsonAsistencias = [];
			List<Data.Entities.Empleados.Asistencia> asistencias = await asistenciaManager.GetAllAsync(); 

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
			ServerResponse resp = new ServerResponse(true, stringLocalizer["AsistenciasFiltradosUnsuccessfully"]);
			try
			{
				// Obtener todas las asistencias
				List<Data.Entities.Empleados.Asistencia> asistencias = await asistenciaManager.GetAllAsync();

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
					// Asegurarse de incluir todas las asistencias hasta el final del día especificado
					DateTime fechaFin = inputFiltro.FechaIngresoFin.Value.Date.AddDays(1).AddTicks(-1);
					asistencias = asistencias.Where(a => a.FechaHora <= fechaFin).ToList();
				}
				// Si no se aplicaron filtros, retornar un resultado vacío para no actualizar la página
				if (string.IsNullOrEmpty(inputFiltro.NombreEmpleado) && !inputFiltro.FechaIngresoInicio.HasValue && !inputFiltro.FechaIngresoFin.HasValue)
				{
					return new JsonResult(resp); // Retorna una respuesta vacía si no se aplicaron filtros
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
				resp.Mensaje = stringLocalizer["AsistenciasFiltradosSuccessfully"];
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}


	}
}
