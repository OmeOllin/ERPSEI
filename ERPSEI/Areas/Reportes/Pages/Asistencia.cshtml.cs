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
		public class ArchivoModel
		{
			public string? Id { get; set; } = string.Empty;
			public string? nombre { get; set; } = string.Empty;
			public int? tipoArchivoId { get; set; }
			public string? extension { get; set; } = string.Empty;
			public string? imgSrc { get; set; } = string.Empty;
		}

		[BindProperty]
		public ImportarModel InputImportar { get; set; }
		public class ImportarModel
		{
			[Required(ErrorMessage = "Required")]
			public IFormFile? Plantilla { get; set; }
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
			public ArchivoModel?[] Archivos { get; set; } = Array.Empty<ArchivoModel>();
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

			if (inputFiltro == null)
			{
				resp.Mensaje = stringLocalizer["Favor de seleccionar rango de fechas"];
				return new JsonResult(resp);
			}

			ServerResponse responseServ = new (true, stringLocalizer["AsistenciasFiltradosUnsuccessfully"]);
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
					DateTime fechaFin = inputFiltro.FechaIngresoFin.Value.Date.AddDays(1).AddTicks(-1);
					asistencias = asistencias.Where(a => a.FechaHora <= fechaFin).ToList();
				}

				// Verificar si se encontraron resultados
				if (asistencias.Count == 0)
				{
					resp.Mensaje = stringLocalizer["NoRecordsFound"];
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
				resp.Mensaje = stringLocalizer["AsistenciasFiltradosSuccessfully"];
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}


		public async Task<JsonResult> OnPostImportarAsistencia()
		{
			ServerResponse resp = new ServerResponse(true, stringLocalizer["EmpleadosImportadosUnsuccessfully"]);
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
									resp.Mensaje = stringLocalizer["AsistenciaImportadoSuccessfully"];
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
									resp.Mensaje = stringLocalizer["AsistenciaImportadoSuccessfully"];
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				resp.TieneError = true;
				resp.Mensaje = stringLocalizer["AsistenciaImportadoUnsuccessfully"];
			}

			return new JsonResult(resp);
		}

		public async Task<ActionResult> OnGetDownloadPlantillaA()
		{
			
				return File("/templates/PlantillaAsistencia.xlsx", MediaTypeNames.Application.Octet, "PlantillaAsistencia.xlsx");
			
		}
		private async Task<string> CreateAsistenciaFromExcelRow(DataRow row)
		{
			string validationMsg = string.Empty;

			DateTime fechahora;
			DateOnly fecha;
			TimeSpan hora;

			DateTime.TryParse(row[1].ToString(), out fechahora);
			DateOnly.TryParse(row[2].ToString(), out fecha);
			TimeSpan.TryParse(row[3].ToString(), out hora);

			Asistencia asistencia = new Asistencia()
			{
				Id = row[0].ToString()?.Trim() ?? string.Empty,
				FechaHora = fechahora,
				Fecha = fecha,
				Hora = hora,
				Direccion = row[4].ToString()?.Trim() ?? string.Empty,
				NombreDispositivo = row[5].ToString()?.Trim() ?? string.Empty,
				SerialDispositivo = row[6].ToString()?.Trim() ?? string.Empty,
				NombreEmpleado = row[7].ToString()?.Trim() ?? string.Empty,
				NoTarjeta = row[8].ToString()?.Trim() ?? string.Empty,
			};

			List<ArchivoModel> archivos = new List<ArchivoModel>();
			// Crea los archivos del usuario.
			foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
			{
				archivos.Add(new ArchivoModel() { extension = "", imgSrc = "", nombre = "", tipoArchivoId = (int)i });
			}

			asistencia.Archivos = archivos.ToArray();

			// Retornar una cadena vacía ya que no se realizan validaciones
			return validationMsg;
		}



	}
}
