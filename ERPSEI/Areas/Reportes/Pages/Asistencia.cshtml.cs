using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Requests;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;
using ERPSEI.Pages.Shared;
using ERPSEI.Data.Entities.Reportes;
using ERPSEI.Data.Managers.Reportes;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Policy = "AccessPolicy")]
	public class AsistenciaModel : ERPPageModel
	{
		private readonly IHorariosManager horariosManager;
		private readonly IAsistenciaManager asistenciaManager;
		private readonly IEmpleadoManager empleadoManager;
		private readonly IStringLocalizer<AsistenciaModel> stringLocalizer;
		private readonly ILogger<AsistenciaModel> logger;

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
			IHorariosManager _horariosManager,
			IEmpleadoManager _empleadoManager,
			IAsistenciaManager _asistenciaManager,
			IStringLocalizer<AsistenciaModel> _stringLocalizer,
			ILogger<AsistenciaModel> _logger
		)
		{
			horariosManager = _horariosManager;
			empleadoManager = _empleadoManager;
			asistenciaManager = _asistenciaManager;
			stringLocalizer = _stringLocalizer;
			logger = _logger;

			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
			InputImportar = new ImportarModel();
		}

		public async Task<JsonResult> OnGetAsistenciasList()
		{
			List<string> jsonAsistencias = new List<string>();
			List<Asistencia> asistencias = await asistenciaManager.GetAllAsync();

			foreach (Asistencia asis in asistencias)
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

		public async Task<JsonResult> OnPostFiltrarAsistencia([FromBody] FiltroModel inputFiltro)
		{
			ServerResponse resp = new ServerResponse(true, stringLocalizer["AsistenciasFiltradosUnsuccessfully"]);

			try
			{
				// Obtener todas las asistencias
				List<Asistencia> asistencias = await asistenciaManager.GetAllAsync();
				List<string> jsonAsistencias = new List<string>();
				foreach (var asis in asistencias)
				{
					jsonAsistencias.Add("{" +
					$"\"Horario\": \"{asis.Horario?.NombreHorario}\", " +
					$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
					$"\"Fecha\": \"{asis.Fecha}\", " +
					$"\"Dia\": \"{asis.Dia}\", " +
					$"\"Entrada\": \"{asis.Entrada}\", " +
					$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
					$"\"Salida\": \"{asis.Salida}\", " +
					$"\"ResultadoS\": \"{asis.ResultadoS}\" " +
					"}");
				}

				string jsonResponse = $"[{string.Join(",", jsonAsistencias)}]";
				resp.Datos = jsonResponse;
				resp.TieneError = false;
				resp.Mensaje = stringLocalizer["AsistenciasFiltradosSuccessfully"];
				resp.TieneError = false;
				resp.Mensaje = stringLocalizer["EmpleadosFiltradosSuccessfully"];
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}


		public ActionResult OnGetDownloadPlantilla()
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
		
		public async Task<JsonResult> OnPostImportarAsistencias()
		{
			ServerResponse resp = new(true, stringLocalizer["AsistenciasImportadasUnsuccessfully"]);
			try
			{
				if (PuedeTodo || PuedeEditar){
					if (Request.Form.Files.Count >= 1)
					{
						//Se procesa el archivo excel.
						using Stream s = Request.Form.Files[0].OpenReadStream();
						using var reader = ExcelReaderFactory.CreateReader(s);
						DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });

						foreach (DataRow row in result.Tables[0].Rows)
						{
							//Omite el procesamiento del row de encabezado
							if (result.Tables[0].Rows.IndexOf(row) == 0)
							{
								resp.TieneError = false;
								resp.Mensaje = stringLocalizer["AsistenciasImportadasSuccessfully"];
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
								resp.Mensaje = stringLocalizer["AsistenciasImportadasSuccessfully"];
							}
						}
					}
				}
				else
				{
					resp.Mensaje = stringLocalizer["AccesoDenegado"];
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				resp.TieneError = true;
				resp.Mensaje = stringLocalizer["AsistenciasImportadosUnsuccessfully"];
			}

			return new JsonResult(resp);
		}

		private async Task<string> CreateAsistenciaFromExcelRow(DataRow row)
		{
			Horarios? horario = await horariosManager.GetByNameAsync(row[0].ToString()?.Trim() ?? string.Empty);
			Empleado? empleado = await empleadoManager.GetByNameAsync(row[1].ToString()?.Trim() ?? string.Empty);

			//DateOnly.TryParse(row[2].ToString(), out DateOnly fecha);
			DateOnly fechaOnly;
			TimeSpan.TryParse(row[4].ToString()?.Trim(), out TimeSpan horaE);
			//TimeSpan.TryParse(row[6].ToString()?.Trim(), out TimeSpan horaS);

			// Obtener y convertir la fecha
			string fechaStr = row[2].ToString();
			if (DateTime.TryParse(fechaStr, out DateTime fechaDateTime))
			{
				fechaOnly = DateOnly.FromDateTime(fechaDateTime);
			}
			else
			{
				fechaOnly = default; // O algún valor predeterminado
			}

			// Inicializar el resultado por defecto
			string resultadoE = row[5].ToString()?.Trim() ?? string.Empty;

			// Verificar si la hora está dentro del rango permitido
			if (horario != null)
			{
				TimeSpan entrada = horario.Entrada;
				TimeSpan toleranciaEntrada = horario.ToleranciaEntrada;
				TimeSpan toleranciaFalta = horario.ToleranciaFalta;

				// Verificar si la hora de entrada es mayor que la tolerancia
				if (horaE > toleranciaEntrada)
				{
					resultadoE = "RETARDO";
				}
				if (horaE < entrada && horaE <= toleranciaEntrada)
				{
					resultadoE = "NORMAL";
				}
				if (horaE > toleranciaFalta)
				{
					resultadoE = "OMISIÓN/FALTA";
				}
			}

			Asistencia asistencia = new Asistencia()
			{
				HorarioId = horario?.Id,
				EmpleadoId = empleado?.Id,
				Fecha = fechaOnly,
				Dia = row[5].ToString()?.Trim() ?? string.Empty,
				Entrada = horaE,
				ResultadoE = resultadoE,
				Salida = horario?.Salida,
				ResultadoS = resultadoE,
			};

			await asistenciaManager.CreateAsync(asistencia);

			// Retornar una cadena vacía ya que no se realizan validaciones
			return string.Empty;
		}
	}
}
