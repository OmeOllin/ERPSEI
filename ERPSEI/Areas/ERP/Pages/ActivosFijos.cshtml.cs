//using ERPSEI.Data;
//using ERPSEI.Data.Entities.Empleados;
//using ERPSEI.Data.Managers.Empleados;
//using ERPSEI.Requests;
//using ExcelDataReader;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Localization;
//using Newtonsoft.Json;
//using System.ComponentModel.DataAnnotations;
//using System.Data;
//using System.Net.Mime;
//using ERPSEI.Pages.Shared;
//using ERPSEI.Data.Entities.Reportes;
//using ERPSEI.Data.Managers.Reportes;
//using Microsoft.DotNet.MSIdentity.Shared;
//using ERPSEI.Data.Entities.Empresas;
//using ERPSEI.Data.Managers;

//namespace ERPSEI.Areas.Catalogos.Pages
//{
//	//[Authorize(Policy = "AccessPolicy")]
//	public class ActivosFijosModel : ERPPageModel
//	{
//		private readonly IHorariosManager horariosManager;
//		private readonly IAsistenciaManager asistenciaManager;
//		private readonly IEmpleadoManager empleadoManager;
//		//private readonly IStringLocalizer<AsistenciaModel> stringLocalizer;
//		//private readonly ILogger<AsistenciaModel> logger;

//		[BindProperty]
//		public FiltroModel InputFiltro { get; set; } = new FiltroModel();
//		public class FiltroModel
//		{
//			[Display(Name = "FolioNameField")]
//			public int? Folio { get; set; }

//			[DataType(DataType.Text)]
//			[Display(Name = "ResponsableNameField")]
//			public string? Responsable { get; set; }

//			[DataType(DataType.Text)]
//			[Display(Name = "CategoriaNameField")]
//			public string? Categoria { get; set; }

//			[DataType(DataType.Text)]
//			[Display(Name = "TipoNameField")]
//			public string? Tipo { get; set; }

//			[DataType(DataType.DateTime)]
//			[Display(Name = "FechaCompraInicioField")]
//			public DateTime? FechaCompraInicio { get; set; }

//			[DataType(DataType.DateTime)]
//			[Display(Name = "FechaCompraFinField")]
//			public DateTime? FechaCompraFin { get; set; }

//			[Display(Name = "EstatusField")]
//			public bool? Estatus { get; set; }
//		}

//		[BindProperty]
//		public Asistencia ListaAsistencia { get; set; }

//		[BindProperty]
//		public ImportarModel InputImportar { get; set; } = new ImportarModel();
//		public class ImportarModel
//		{
//			[Required(ErrorMessage = "Required")]
//			public IFormFile? Plantilla { get; set; }
//		}
//		public ActivosFijosModel(
//			IHorariosManager _horariosManager,
//			IEmpleadoManager _empleadoManager,
//			IAsistenciaManager _asistenciaManager
//			//IStringLocalizer<AsistenciaModel> _stringLocalizer,
//			//ILogger<AsistenciaModel> _logger
//		)
//		{
//			horariosManager = _horariosManager;
//			empleadoManager = _empleadoManager;
//			asistenciaManager = _asistenciaManager;
//			//stringLocalizer = _stringLocalizer;
//			//logger = _logger;

//			InputFiltro = new FiltroModel();
//			ListaAsistencia = new Asistencia();
//			InputImportar = new ImportarModel();
//		}

//		public async Task<JsonResult> OnGetAsistenciasList()
//		{
//			List<string> jsonAsistencias = new List<string>();
//			List<Asistencia> asistencias = await asistenciaManager.GetAllAsync();

//			foreach (Asistencia asis in asistencias)
//			{
//				jsonAsistencias.Add("{" +
//					$"\"Horario\": \"{asis.Horario?.NombreHorario}\", " +
//					$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
//					$"\"Fecha\": \"{asis.Fecha}\", " +
//					$"\"Dia\": \"{asis.Dia}\", " +
//					$"\"Entrada\": \"{asis.Entrada}\", " +
//					$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
//					$"\"Salida\": \"{asis.Salida}\", " +
//					$"\"ResultadoS\": \"{asis.ResultadoS}\"" +
//					"}");
//			}

//			string jsonResponse = $"[{string.Join(",", jsonAsistencias)}]";
//			return new JsonResult(jsonResponse);
//		}

//		/*public async Task<JsonResult> OnPostFiltrarAsistencia()
//		{
//			ServerResponse resp = new(true, stringLocalizer["AsistenciasFiltradasUnsuccessfully"]);
//			try
//			{
//				if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
//				{
//					resp.Datos = await GetListaAsistencias(InputFiltro);
//					resp.TieneError = false;
//					resp.Mensaje = stringLocalizer["AsistenciasFiltradasSuccessfully"];
//				}
//				else
//				{
//					resp.Mensaje = stringLocalizer["AccesoDenegado"];
//				}
//			}
//			catch (Exception ex)
//			{
//				logger.LogError(ex.Message);
//			}

//			return new JsonResult(resp);
//		}*/
//		private async Task<string> GetListaAsistencias(FiltroModel? filtro = null)
//		{
//			string jsonResponse;
//			List<string> jsonAsistencias = [];
//			List<Asistencia> asistencias = [];

//			/*if (filtro != null)
//			{
//				asistencias = await asistenciaManager.GetAllAsync(
//					filtro.NombreEmpleado,
//					filtro.FechaIngresoInicio,
//					filtro.FechaIngresoFin);
//			}
//			else
//			{
//				asistencias = await asistenciaManager.GetAllAsync();
//			}*/

//			foreach (var asis in asistencias)
//			{
//				jsonAsistencias.Add("{" +
//				$"\"Horario\": \"{asis.Horario?.NombreHorario}\", " +
//				$"\"NombreEmpleado\": \"{asis.Empleado?.NombreCompleto}\", " +
//				$"\"Fecha\": \"{asis.Fecha}\", " +
//				$"\"Dia\": \"{asis.Dia}\", " +
//				$"\"Entrada\": \"{asis.Entrada}\", " +
//				$"\"ResultadoE\": \"{asis.ResultadoE}\", " +
//				$"\"Salida\": \"{asis.Salida}\", " +
//				$"\"ResultadoS\": \"{asis.ResultadoS}\" " +
//				"}");
//			}
//			jsonResponse = $"[{string.Join(",", jsonAsistencias)}]";
//			return jsonResponse;
//		}
		
//		public ActionResult OnGetDownloadPlantilla()
//		{
//			if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
//			{
//				return File("/templates/PlantillaAsistencia.xlsx", MediaTypeNames.Application.Octet, "PlantillaAsistencia.xlsx");
//			}
//			else
//			{
//				return new EmptyResult();
//			}
//		}

//		/*public async Task<JsonResult> OnPostImportarAsistencias()
//		{
//			ServerResponse resp = new(true, stringLocalizer["AsistenciasImportadasUnsuccessfully"]);
//			try
//			{
//				if (PuedeTodo || PuedeEditar)
//				{
//					if (Request.Form.Files.Count >= 1)
//					{
//						//Se procesa el archivo excel.
//						using Stream s = Request.Form.Files[0].OpenReadStream();
//						using var reader = ExcelReaderFactory.CreateReader(s);
//						DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration() { FilterSheet = (tableReader, sheetIndex) => sheetIndex == 0 });

//						DataRow? firstRow = null;

//						foreach (DataRow row in result.Tables[0].Rows)
//						{
//							// Omite el procesamiento del row de encabezado
//							if (result.Tables[0].Rows.IndexOf(row) == 0)
//							{
//								resp.TieneError = false;
//								resp.Mensaje = stringLocalizer["¡Asistencias importadas satisfactoriamente!"];
//								continue;
//							}

//							// Si firstRow es nulo, significa que estamos procesando el primer registro del par
//							if (firstRow == null)
//							{
//								firstRow = row; // Guardamos el primer registro temporalmente
//							}
//							else
//							{
//								// Aquí, estamos procesando el segundo registro del par
//								string vmsg = await CreateAsistenciaFromExcelRow(firstRow, row);

//								// Si hubo errores, detenemos el proceso
//								if ((vmsg ?? "").Length >= 1)
//								{
//									resp.TieneError = true;
//									resp.Mensaje = vmsg;
//									break;
//								}
//								else
//								{
//									resp.TieneError = false;
//									resp.Mensaje = stringLocalizer["¡Asistencias importadas satisfactoriamente!"];
//								}

//								// Reiniciamos firstRow para el siguiente par
//								firstRow = null;
//							}
//						}

//						// Si queda un registro no procesado (por número impar de filas), lo procesamos
//						if (firstRow != null)
//						{
//							string vmsg = await CreateAsistenciaFromExcelRow(firstRow, null);

//							if ((vmsg ?? "").Length >= 1)
//							{
//								resp.TieneError = true;
//								resp.Mensaje = vmsg;
//							}
//						}
//					}
//				}
//				else
//				{
//					resp.Mensaje = stringLocalizer["AccesoDenegado"];
//				}
//			}
//			catch (Exception ex)
//			{
//				logger.LogError(ex.Message);
//				resp.TieneError = true;
//				resp.Mensaje = stringLocalizer["Error al importar las asistencias"];
//			}

//			return new JsonResult(resp);
//		}
//		*/
//		private async Task<string> CreateAsistenciaFromExcelRow(DataRow firstRow, DataRow? secondRow)
//		{
//			Horarios? horario = await horariosManager.GetByNameAsync(firstRow[0].ToString()?.Trim() ?? string.Empty);
//			Empleado? empleado = await empleadoManager.GetByNameAsync(firstRow[1].ToString()?.Trim() ?? string.Empty);

//			DateOnly fechaOnly;

//			// Intentar convertir la hora de entrada
//			TimeSpan horaE;
//			if (!TimeSpan.TryParse(firstRow[4].ToString()?.Trim(), out horaE))
//			{
//				// Intentar convertir usando DateTime como fallback
//				if (DateTime.TryParse(firstRow[4].ToString()?.Trim(), out DateTime entradaDateTime))
//				{
//					horaE = entradaDateTime.TimeOfDay;
//				}
//				else
//				{
//					horaE = TimeSpan.Zero; // Manejo del error si falla la conversión
//				}
//			}

//			// Intentar convertir la hora de salida
//			TimeSpan horaS;
//			if (!TimeSpan.TryParse(secondRow?[4].ToString()?.Trim(), out horaS))
//			{
//				// Intentar convertir usando DateTime como fallback
//				if (DateTime.TryParse(secondRow?[4].ToString()?.Trim(), out DateTime salidaDateTime))
//				{
//					horaS = salidaDateTime.TimeOfDay;
//				}
//				else
//				{
//					horaS = TimeSpan.Zero; // Manejo del error si falla la conversión
//				}
//			}

//			// Obtener y convertir la fecha
//			string? fechaStr = firstRow[2].ToString();
//			if (DateTime.TryParse(fechaStr, out DateTime fechaDateTime))
//			{
//				fechaOnly = DateOnly.FromDateTime(fechaDateTime);
//			}
//			else
//			{
//				fechaOnly = default;
//			}

//			// Inicializar el resultado por defecto
//			string resultadoE = firstRow[5].ToString()?.Trim() ?? string.Empty;

//			if (horario != null)
//			{
//				TimeSpan entrada = horario.Entrada;
//				TimeSpan toleranciaEntrada = horario.ToleranciaEntrada;
//				TimeSpan toleranciaFalta = horario.ToleranciaFalta;

//				if (horaE > toleranciaEntrada)
//				{
//					resultadoE = "RETARDO";
//				}
//				if (horaE <= entrada || horaE >= entrada && horaE <= toleranciaEntrada)
//				{
//					resultadoE = "NORMAL";
//				}
//				if (horaE > toleranciaFalta)
//				{
//					resultadoE = "OMISIÓN/FALTA";
//				}
//				if (horaE == TimeSpan.Zero) 
//				{
//					resultadoE = "OMISIÓN/FALTA";
//				}
//			}
//			else
//			{
//				resultadoE = "OMISIÓN/FALTA";
//			}


//			// Inicializar el resultado por defecto
//			string resultadoS = secondRow?[5].ToString()?.Trim() ?? string.Empty;

//			if (horario != null)
//			{
//				TimeSpan salida = horario.Salida;
//				TimeSpan toleranciaSalida = horario.ToleranciaSalida;

//				if (horaS >= toleranciaSalida || horaS >= salida)
//				{
//					resultadoS = "NORMAL";
//				}
//				if (horaS < toleranciaSalida)
//				{
//					resultadoS = "TEMPRANO";
//				}
//				if (horaS == TimeSpan.Zero)
//				{
//					resultadoS = "OMISIÓN/FALTA";
//				}
//			}
//			else 
//			{
//				resultadoS = "OMISIÓN/FALTA";
//			}



//			Asistencia asistencia = new Asistencia()
//			{
//				HorarioId = horario?.Id,
//				EmpleadoId = empleado?.Id,
//				Fecha = fechaOnly,
//				Dia = firstRow[5].ToString()?.Trim() ?? string.Empty,
//				Entrada = horaE,
//				ResultadoE = resultadoE,
//				Salida = horaS,
//				ResultadoS = resultadoS,
//			};

//			await asistenciaManager.CreateAsync(asistencia);

//			return string.Empty;
//		}

//	}
//}
