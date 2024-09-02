using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using ERPSEI.Data.Managers.Empleados;
using ERPSEI.Data.Managers.Reportes;
using ERPSEI.Pages.Shared;
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
using ERPSEI.Resources;
public static class SessionExtensions
{
	public static void SetObjectAsJson(this ISession session, string key, object value)
	{
		session.SetString(key, JsonConvert.SerializeObject(value));
	}

	public static T GetObjectFromJson<T>(this ISession session, string key)
	{
		var value = session.GetString(key);
		return value == null ? default(T?) : JsonConvert.DeserializeObject<T>(value);
	}
}

namespace ERPSEI.Areas.Reportes.Pages
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
			public IFormFile? Plantilla { get; set; }
		}

		[BindProperty]
		public InputModel Input { get; set; }
		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }
			public string ResultadoE { get; set; } = string.Empty;
			public string ResultadoS { get; set; } = string.Empty;
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

			Input = new InputModel();
			InputFiltro = new FiltroModel();
			ListaAsistencia = new Asistencia();
			InputImportar = new ImportarModel();
		}

		public async Task<JsonResult> OnGetAsistenciasCalculo()
		{
			// Recuperar el filtro de la sesión
			var filtro = HttpContext.Session.GetObjectFromJson<FiltroModel>("FiltroAsistencia");

			// Obtener las asistencias basadas en el filtro
			List<Asistencia> asistencias = await ObtenerAsistenciasPorFiltro(filtro);

			// Si no se encontraron asistencias y no hay filtro, obtener las asistencias de la fecha actual
			if (!asistencias.Any() && (filtro == null || !filtro.FechaIngresoInicio.HasValue))
			{
				DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
				asistencias = await asistenciaManager.GetAllAsync();
				asistencias = asistencias.Where(asis => asis.Fecha == fechaActual).ToList();
			}

			// Procesar cada asistencia
			foreach (var asistencia in asistencias)
			{
				// Si no hay entrada, marcar como "OMISIÓN/FALTA"
				if (asistencia.Entrada == null)
				{
					asistencia.ResultadoE = "OMISIÓN/FALTA";
				}

				// Si hay salida, marcar como "NORMAL"
				if (asistencia.Salida != null)
				{
					asistencia.ResultadoS = "NORMAL";
				}
			}

			// Generar el resumen de asistencias
			var resumenAsistencias = asistencias
				.GroupBy(a => a.Empleado?.NombreCompleto)
				.Select(g => new
				{
					nombre = g.Key,
					retardos = g.Count(a => a.ResultadoE == "RETARDO"),
					omisionesFaltas = g.Count(a => a.ResultadoE == "OMISIÓN/FALTA"),
					acumuladoRet = g.Count(a => a.ResultadoE == "RETARDO"),
					totalFaltas = g.Count(a => a.ResultadoE == "OMISIÓN/FALTA") + (g.Count(a => a.ResultadoE == "RETARDO") / 2)
				})
				.ToList();

			return new JsonResult(resumenAsistencias);
		}

		public async Task<JsonResult> OnGetAsistenciasList()
		{
			List<string> jsonAsistencias = new List<string>();
			List<Asistencia> asistencias = await asistenciaManager.GetAllAsync();

			foreach (Asistencia asis in asistencias)
			{
				jsonAsistencias.Add("{" +
					$"\"Id\": \"{asis.Id}\", " +
					"\"Horario\": \"" + (
					asis.Empleado?.HorarioId == 1 ? "General" :
					asis.Empleado?.HorarioId == 2 ? "General Intendencia" :
					asis.Empleado?.HorarioId == 3 ? "General Recepción" :
					asis.Empleado?.HorarioId == 4 ? "Polanco" :
					asis.Empleado?.HorarioId == 5 ? "Polanco Intendencia" :
					asis.Empleado?.HorarioId == 6 ? "Polanco Recepción" :
					asis.Empleado?.Horario?.Descripcion ?? "No Definido") + "\", " +
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

		public async Task<JsonResult> OnPostFiltrarAsistencia()
		{
			ServerResponse resp = new(true, stringLocalizer["AsistenciasFiltradasUnsuccessfully"]);
			try
			{
				if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
				{
					// Guardar el filtro en la sesión (u otra forma de persistencia).
					HttpContext.Session.SetObjectAsJson("FiltroAsistencia", InputFiltro);

					resp.Datos = await GetListaAsistencias(InputFiltro);
					resp.TieneError = false;
					resp.Mensaje = stringLocalizer["AsistenciasFiltradasSuccessfully"];
				}
				else
				{
					resp.Mensaje = stringLocalizer["AccesoDenegado"];
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		private async Task<string> GetListaAsistencias(FiltroModel? filtro = null)
		{
			// Obtener las asistencias filtradas (si hay filtro) o todas (si no hay filtro)
			List<Asistencia> asistencias = await ObtenerAsistenciasPorFiltro(filtro);

			// Si se proporciona un filtro y hay resultados, generamos directamente la respuesta JSON
			if (filtro != null && asistencias.Any())
			{
				return GenerarJsonAsistencias(asistencias);
			}

			// Si no hay filtro o si no se encontraron asistencias con el filtro, procedemos con la lógica completa
			(DateOnly fIni, DateOnly fFin) = InicializarFechas(filtro);

			// Obtener los empleados que se les calcula asistencia
			List<Empleado> empleadosAsistencias = await ObtenerEmpleadosConAsistencias();

			// Crear las asistencias finales basadas en los empleados y fechas
			List<Asistencia> asistenciasFinales = await CrearAsistenciasFinales(asistencias, empleadosAsistencias, fIni, fFin);

			// Generar la respuesta JSON con las asistencias finales
			string jsonResponse = GenerarJsonAsistencias(asistenciasFinales);
			return jsonResponse;
		}

		private async Task<List<Asistencia>> ObtenerAsistenciasPorFiltro(FiltroModel? filtro)
		{
			if (filtro != null && !string.IsNullOrEmpty(filtro.NombreEmpleado))
			{
				return await asistenciaManager.GetAllAsync(filtro.NombreEmpleado, filtro.FechaIngresoInicio, filtro.FechaIngresoFin);
			}
			else if (filtro != null && !string.IsNullOrEmpty(filtro.NombreEmpleado))
			{
				// Filtrar solo por nombre, sin considerar las fechas
				return await asistenciaManager.GetAllAsync(filtro.NombreEmpleado, null, null);
			}
			else if (filtro != null && filtro.FechaIngresoInicio.HasValue && filtro.FechaIngresoFin.HasValue)
			{
				return await asistenciaManager.GetAllAsync(filtro.NombreEmpleado, filtro.FechaIngresoInicio, filtro.FechaIngresoFin);
			}
			else if (filtro != null && filtro.FechaIngresoInicio.HasValue && !filtro.FechaIngresoFin.HasValue)
			{
				return await asistenciaManager.GetAllAsync(filtro.NombreEmpleado, filtro.FechaIngresoInicio, filtro.FechaIngresoFin);
			}
			else
			{
				return await asistenciaManager.GetAllAsync();
			}
		}

		private (DateOnly, DateOnly) InicializarFechas(FiltroModel? filtro)
		{
			DateOnly fIni = filtro?.FechaIngresoInicio.HasValue == true
				? DateOnly.FromDateTime(filtro.FechaIngresoInicio.Value)
				: DateOnly.FromDateTime(DateTime.Now);

			DateOnly fFin = filtro?.FechaIngresoFin.HasValue == true
				? DateOnly.FromDateTime(filtro.FechaIngresoFin.Value)
				: fIni;

			return (fIni, fFin);
		}

		private async Task<List<Empleado>> ObtenerEmpleadosConAsistencias()
		{
			List<Empleado> empleadosAsistencias = await empleadoManager.GetAllAsync();
			return empleadosAsistencias.Where(e => (e.HorarioId ?? 0) != 0 && (e.CalcularAsistencia ?? false)).ToList();
		}

		private async Task<List<Asistencia>> CrearAsistenciasFinales(List<Asistencia> asistencias, List<Empleado> empleados, DateOnly fIni, DateOnly fFin)
		{
			List<Asistencia> asistenciasFinales = new();

			foreach (Empleado e in empleados)
			{
				for (DateOnly f = fIni; f <= fFin; f = f.AddDays(1))
				{
					List<int>? diasDescanso = e.Horario?.HorarioDetalles?.Where(hd => !hd.Activado).Select(hd => hd.NumeroDiaSemana).ToList();
					if (diasDescanso != null && diasDescanso.Contains((int)f.DayOfWeek)) { continue; }

					Asistencia? asistenciaExistente = asistencias.FirstOrDefault(a => a.Empleado.Id == e.Id && a.Fecha == f);

					if (asistenciaExistente != null)
					{
						asistenciasFinales.Add(asistenciaExistente);
					}
					else
					{
						Asistencia newA = new()
						{
							Dia = f.ToString("dddd"),
							Fecha = f,
							EmpleadoId = e.Id,
							Entrada = TimeSpan.Zero,
							Salida = TimeSpan.Zero,
							ResultadoE = "OMISIÓN/FALTA",
							ResultadoS = "OMISIÓN/FALTA"
						};

						newA.Id = await asistenciaManager.CreateAsync(newA);
						if (newA.Id != 0)
						{
							newA.Empleado = e;
							asistenciasFinales.Add(newA);
						}
					}
				}
			}

			return asistenciasFinales;
		}
		private string GenerarJsonAsistencias(List<Asistencia> asistenciasFinales)
		{
			List<string> jsonAsistencias = asistenciasFinales.Select(asis => "{" +
				$"\"Id\": \"{asis.Id}\", " +
				"\"Horario\": \"" +(
				asis.Empleado?.HorarioId == 1 ? "General" :
				asis.Empleado?.HorarioId == 2 ? "General Intendencia" :
				asis.Empleado?.HorarioId == 3 ? "General Recepción" :
				asis.Empleado?.HorarioId == 4 ? "Polanco" :
				asis.Empleado?.HorarioId == 5 ? "Polanco Intendencia" :
				asis.Empleado?.HorarioId == 6 ? "Polanco Recepción" :
				asis.Empleado?.Horario?.Descripcion ?? "No Definido") + "\", " +
				$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
				$"\"Fecha\": \"{asis.Fecha}\", " +
				$"\"Dia\": \"{asis.Dia}\", " +
				$"\"Entrada\": \"{asis.Entrada}\", " +
				$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
				$"\"Salida\": \"{asis.Salida}\", " +
				$"\"ResultadoS\": \"{asis.ResultadoS}\" " +
				"}").ToList();

			return $"[{string.Join(",", jsonAsistencias)}]";
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
				if (PuedeTodo || PuedeEditar)
				{
					if (Request.Form.Files.Count >= 1)
					{
						//Se procesa el archivo excel.
						using Stream s = Request.Form.Files[0].OpenReadStream();
						using var reader = ExcelReaderFactory.CreateReader(s);
						DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });

						DataRow? firstRow = null;

						foreach (DataRow row in result.Tables[0].Rows)
						{
							// Omite el procesamiento del row de encabezado
							if (result.Tables[0].Rows.IndexOf(row) == 0)
							{
								resp.TieneError = false;
								resp.Mensaje = stringLocalizer["¡Asistencias importadas satisfactoriamente!"];
								continue;
							}

							// Si firstRow es nulo, significa que estamos procesando el primer registro del par
							if (firstRow == null)
							{
								firstRow = row; // Guardamos el primer registro temporalmente
							}
							else
							{
								// Aquí, estamos procesando el segundo registro del par
								string vmsg = await CreateAsistenciaFromExcelRow(firstRow, row);

								// Si hubo errores, detenemos el proceso
								if ((vmsg ?? "").Length >= 1)
								{
									resp.TieneError = true;
									resp.Mensaje = vmsg;
									break;
								}
								else
								{
									resp.TieneError = false;
									resp.Mensaje = stringLocalizer["¡Asistencias importadas satisfactoriamente!"];
								}

								// Reiniciamos firstRow para el siguiente par
								firstRow = null;
							}
						}

						// Si queda un registro no procesado (por número impar de filas), lo procesamos
						if (firstRow != null)
						{
							string vmsg = await CreateAsistenciaFromExcelRow(firstRow, null);

							if ((vmsg ?? "").Length >= 1)
							{
								resp.TieneError = true;
								resp.Mensaje = vmsg;
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
				resp.Mensaje = stringLocalizer["Error al importar las asistencias"];
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveAsistencia()
		{
			ServerResponse resp = new(true, stringLocalizer["AsistenciaSavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k]?.Errors ?? []).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					// Intenta obtener la asistencia por ID
					Asistencia? asistencia = await asistenciaManager.GetByIdAsync(Input.Id);

					if (asistencia == null)
					{
						resp.Mensaje = stringLocalizer["ErrorAsistenciaNoEncontrada"];
					}
					else
					{
						// Actualiza solo los campos ResultadoE y ResultadoS
						asistencia.ResultadoE = Input.ResultadoE;
						asistencia.ResultadoS = Input.ResultadoS;

						// Guarda los cambios en la base de datos
						await asistenciaManager.UpdateAsync(asistencia);

						resp.TieneError = false;
						resp.Mensaje = stringLocalizer["AsistenciaSavedSuccessfully"];
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}


		private async Task<string> CreateAsistenciaFromExcelRow(DataRow firstRow, DataRow? secondRow)
		{
			Horario? horario = await horariosManager.GetByNameAsync(firstRow[0].ToString()?.Trim() ?? string.Empty);
			Empleado? empleado = await empleadoManager.GetByNameAsync(firstRow[1].ToString()?.Trim() ?? string.Empty);

			DateOnly fechaOnly;

			// Intentar convertir la hora de entrada
			TimeSpan horaE;
			if (!TimeSpan.TryParse(firstRow[4].ToString()?.Trim(), out horaE))
			{
				// Intentar convertir usando DateTime como fallback
				if (DateTime.TryParse(firstRow[4].ToString()?.Trim(), out DateTime entradaDateTime))
				{
					horaE = entradaDateTime.TimeOfDay;
				}
				else
				{
					horaE = TimeSpan.Zero; // Manejo del error si falla la conversión
				}
			}

			// Intentar convertir la hora de salida
			TimeSpan horaS;
			if (!TimeSpan.TryParse(secondRow?[4].ToString()?.Trim(), out horaS))
			{
				// Intentar convertir usando DateTime como fallback
				if (DateTime.TryParse(secondRow?[4].ToString()?.Trim(), out DateTime salidaDateTime))
				{
					horaS = salidaDateTime.TimeOfDay;
				}
				else
				{
					horaS = TimeSpan.Zero; // Manejo del error si falla la conversión
				}
			}

			// Obtener y convertir la fecha
			string? fechaStr = firstRow[2].ToString();
			if (DateTime.TryParse(fechaStr, out DateTime fechaDateTime))
			{
				fechaOnly = DateOnly.FromDateTime(fechaDateTime);
			}
			else
			{
				fechaOnly = default;
			}

			// Inicializar el resultado por defecto
			string resultadoE = firstRow[5].ToString()?.Trim() ?? string.Empty;

			HorarioDetalle? hd = horario?.HorarioDetalles?.Where(hd => hd.NumeroDiaSemana == (int)fechaOnly.DayOfWeek).FirstOrDefault();
			if (horario != null && hd != null)
			{
				TimeSpan entrada = hd.Entrada;
				TimeSpan toleranciaEntrada = hd.ToleranciaEntrada;
				TimeSpan toleranciaFalta = hd.ToleranciaFalta;

				if (horaE > toleranciaEntrada)
				{
					resultadoE = "RETARDO";
				}
				if (horaE <= entrada || horaE >= entrada && horaE <= toleranciaEntrada)
				{
					resultadoE = "NORMAL";
				}
				if (horaE > toleranciaFalta)
				{
					resultadoE = "OMISIÓN/FALTA";
				}
				if (horaE == TimeSpan.Zero)
				{
					resultadoE = "OMISIÓN/FALTA";
				}
				
			}
			else
			{
				resultadoE = "OMISIÓN/FALTA";
			}

			// Inicializar el resultado por defecto
			string resultadoS = secondRow?[5].ToString()?.Trim() ?? string.Empty;

			if (horario != null && hd != null)
			{
				TimeSpan salida = hd.Salida;
				TimeSpan toleranciaSalida = hd.ToleranciaSalida;

				if (horaS >= toleranciaSalida || horaS >= salida)
				{
					resultadoS = "NORMAL";
				}
				if (horaS < toleranciaSalida)
				{
					resultadoS = "TEMPRANO";
				}
				if (horaS == TimeSpan.Zero)
				{
					resultadoS = "OMISIÓN/FALTA";
				}
			}
			else 
			{
				resultadoS = "OMISIÓN/FALTA";
			}

			Asistencia asistencia = new Asistencia()
			{
				EmpleadoId = empleado?.Id,
				Fecha = fechaOnly,
				Dia = firstRow[5].ToString()?.Trim() ?? string.Empty,
				Entrada = horaE,
				ResultadoE = resultadoE,
				Salida = horaS,
				ResultadoS = resultadoS,
			};

			await asistenciaManager.CreateAsync(asistencia);

			return string.Empty;
		}

	}
}
