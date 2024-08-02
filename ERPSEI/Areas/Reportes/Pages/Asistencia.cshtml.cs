using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Requests;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;
using ERPSEI.Pages.Shared;
using ERPSEI.Data.Entities.Reportes;
using ERPSEI.Data.Managers.Reportes;
using ERPSEI.Data.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Policy = "AccessPolicy")]
	public class AsistenciaModel : ERPPageModel
	{
		private readonly IHorariosManager _horariosManager;
		private readonly IAsistenciaManager _asistenciaManager;
		private readonly IEmpleadoManager _empleadoManager;
		private readonly IStringLocalizer<AsistenciaModel> _stringLocalizer;
		private readonly ILogger<AsistenciaModel> _logger;

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
		public Asistencia ListaAsistencia { get; set; }
		
		[BindProperty]
		public ImportarModel InputImportar { get; set; } = new ImportarModel();
		public class ImportarModel
		{
			[Required(ErrorMessage = "Required")]
			public IFormFile? Plantilla { get; set; }
		}
		public AsistenciaModel(
			IHorariosManager horariosManager,
			IEmpleadoManager empleadoManager,
			IAsistenciaManager asistenciaManager,
			IStringLocalizer<AsistenciaModel> stringLocalizer,
			ILogger<AsistenciaModel> logger
		)
		{
			_horariosManager = horariosManager;
			_empleadoManager = empleadoManager;
			_asistenciaManager = asistenciaManager;
			_stringLocalizer = stringLocalizer;
			_logger = logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
			InputImportar = new ImportarModel();
		}

		public async Task<JsonResult> OnGetAsistenciasList()
		{
			List<string> jsonAsistencias = new List<string>();
			List<Data.Entities.Reportes.Asistencia> asistencias = await _asistenciaManager.GetAllAsync();

			foreach (Data.Entities.Reportes.Asistencia asis in asistencias)
			{
				jsonAsistencias.Add("{" +
					$"\"Horario\": \"{asis.Horario?.NombreHorario}\", " +
					$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
					$"\"Fecha\": \"{asis.Fecha}\", " +
					$"\"Dia\": \"{asis.Dia}\", " +
					$"\"Entrada\": \"{asis.Entrada}\", " +
					$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
					$"\"Salida\": \"{asis.Salida}\", " +
					$"\"ResultadoS\": \"{asis.ResultadoS}\"" +
					"}");
			}

			string jsonResponse = $"[{string.Join(",", jsonAsistencias)}]";
			return new JsonResult(jsonResponse);
		}

		public async Task<JsonResult> OnPostFiltrarAsistencia([FromBody]FiltroModel inputFiltro)
		{
			ServerResponse resp = new ServerResponse(true, _stringLocalizer["AsistenciasFiltradosUnsuccessfully"]);

			if (inputFiltro == null)
			{
				resp.Mensaje = _stringLocalizer["Favor de seleccionar rango de fechas"];
				return new JsonResult(resp);
			}

			ServerResponse responseServ = new(true, _stringLocalizer["AsistenciasFiltradosUnsuccessfully"]);
			try
			{
				// Obtener todas las asistencias
				List<Asistencia> asistencias = await _asistenciaManager.GetAllAsync();

				// Aplicar filtros secuencialmente
				if (!string.IsNullOrEmpty(inputFiltro.NombreEmpleado))
				{
					asistencias = asistencias.Where(a => a.Empleado != null && a.Empleado.NombreCompleto.Contains(inputFiltro.NombreEmpleado, StringComparison.OrdinalIgnoreCase)).ToList();
				}

				if (inputFiltro.FechaIngresoInicio.HasValue)
				{
					DateOnly fechaInicio = DateOnly.FromDateTime(inputFiltro.FechaIngresoInicio.Value);
					asistencias = asistencias.Where(a => a.Fecha >= fechaInicio).ToList();
				}

				if (inputFiltro.FechaIngresoFin.HasValue)
				{
					DateOnly fechaFin = DateOnly.FromDateTime(inputFiltro.FechaIngresoFin.Value);
					asistencias = asistencias.Where(a => a.Fecha <= fechaFin).ToList();
				}

				// Verificar si se encontraron resultados
				if (asistencias.Count == 0)
				{
					resp.Mensaje = _stringLocalizer["Registro(s) no encontrado(s)"];
					return new JsonResult(resp);
				}

				// Construir la respuesta JSON
				List<string> jsonAsistencias = new List<string>();
				foreach (var asis in asistencias)
				{
					jsonAsistencias.Add("{" +
					$"\"Horario\": \"{asis.Horario?.NombreHorario}\", " +
					$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
					$"\"Fecha\": \"{asis.Fecha:yyyy-MM-dd}\", " + // Asumiendo que Fecha es DateTime
					$"\"Dia\": \"{asis.Dia}\", " +
					$"\"Entrada\": \"{asis.Entrada}\", " +
					$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
					$"\"Salida\": \"{asis.Salida}\", " + // Añadida coma después de "Salida"
					$"\"ResultadoS\": \"{asis.ResultadoS}\" " +
					"}");
				}

				string jsonResponse = $"[{string.Join(",", jsonAsistencias)}]";
				resp.Datos = jsonResponse;
				resp.TieneError = false;
				resp.Mensaje = _stringLocalizer["AsistenciasFiltradosSuccessfully"];

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostImportarAsistencia()
		{
			ServerResponse resp = new ServerResponse(true, _stringLocalizer["AsistenciasImportadosUnsuccessfully"]);
			try
			{
				if (Request.Form.Files.Count >= 1)
				{
					//Se procesa el archivo excel.
					using (Stream s = Request.Form.Files[0].OpenReadStream())
					{
						using (var reader = ExcelReaderFactory.CreateReader(s))
						{
							DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });
							foreach (DataRow row in result.Tables[0].Rows)
							{
								//Omite el procesamiento del row de encabezado
								if (result.Tables[0].Rows.IndexOf(row) == 0)
								{
									resp.TieneError = false;
									resp.Mensaje = _stringLocalizer["AsistenciasImportadosSuccessfully"];
									continue;
								}

								string vmsg = await CreateAsistenciaFromExcelRow(row);

								//Si la longitud del mensaje de respuesta es mayor o igual a uno, se considera que hubo errores.
								if ((vmsg ?? "").Length >= 1)
								{
									resp.TieneError = true;
									resp.Mensaje = vmsg;
									break;
								}
								else
								{
									resp.TieneError = false;
									resp.Mensaje = _stringLocalizer["AsistenciasImportadosSuccessfully"];
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				resp.TieneError = true;
				resp.Mensaje = _stringLocalizer["AsistenciasImportadosUnsuccessfully"];
			}

			return new JsonResult(resp);
		}

		public ActionResult OnGetDownloadPlantillaA()
		{
			if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
			{
				return File("/templates/PlantillaAsistencia.xlsx", MediaTypeNames.Application.Octet, "PlantillaAsistencia.xlsx");
			}
			else
			{
				return new EmptyResult();
			}
		}
		private async Task<string> CreateAsistenciaFromExcelRow(DataRow row)
		{
			Horarios? horario = await _horariosManager.GetByNameAsync(row[0].ToString()?.Trim() ?? string.Empty);
			Empleado? empleado = await _empleadoManager.GetByNameAsync(row[1].ToString()?.Trim() ?? string.Empty);

			DateOnly fecha;
			TimeSpan horaE;
			TimeSpan horaS;

			DateOnly.TryParse(row[2].ToString(), out fecha);
			TimeSpan.TryParse(row[4].ToString(), out horaE);
			TimeSpan.TryParse(row[6].ToString(), out horaS);

			Asistencia asistencia = new Asistencia()
			{
				HorarioId = horario?.Id,
				EmpleadoId = empleado?.Id,
				Fecha = fecha,
				Dia = row[3].ToString()?.Trim() ?? string.Empty,
				Entrada = horaE,
				ResultadoE = row[5].ToString()?.Trim() ?? string.Empty,
				Salida = horaS,
				ResultadoS = row[7].ToString()?.Trim() ?? string.Empty,
			};

			await _asistenciaManager.CreateAsync(asistencia);

			// Retornar una cadena vacía ya que no se realizan validaciones
			return string.Empty;
		}
	}
}
