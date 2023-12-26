using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Requests;
using ERPSEI.Resources;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;

namespace ERPSEI.Areas.Catalogos.Pages
{
	public class EmpresasModel : PageModel
	{
		private readonly IEmpresaManager _empresaManager;
		private readonly IArchivoEmpresaManager _archivoEmpresaManager;
		private readonly IStringLocalizer<EmpresasModel> _strLocalizer;
		private readonly ILogger<EmpresasModel> _logger;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }

		public class FiltroModel
		{
			[Display(Name = "OrigenField")]
			public string? Origen { get; set; }

			[Display(Name = "NivelField")]
			public string? Nivel { get; set; }

			[Display(Name = "AdministradorField")]
			public string? Administrador { get; set; }

			[Display(Name = "AccionistaField")]
			public string? Accionista { get; set; }
		}

		[BindProperty]
		public EmpresaModel InputEmpresa { get; set; }

		public class EmpresaModel
		{
			public int Id { get; set; }

			public IFormFile? ProfilePicture { get; set; }

			[DataType(DataType.Text)]
			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.AlphanumSpace, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "RazonSocialField")]
			public string RazonSocial { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.AlphanumSpace, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "OrigenField")]
			public string Origen { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.AlphanumSpace, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "NivelField")]
			public string Nivel { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(13, ErrorMessage = "FieldLength", MinimumLength = 13)]
			[RegularExpression(RegularExpressions.AlphanumNoSpaceNoUnderscore, ErrorMessage = "AlphanumNoSpaceNoUnderscore")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "RFCField")]
			public string RFC { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[DataType(DataType.MultilineText)]
			[Display(Name = "DomicilioFiscalField")]
			public string DomicilioFiscal { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "AdministradorField")]
			public string Administrador { get; set; } = string.Empty;

			[DataType(DataType.Text)]
			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.PersonName, ErrorMessage = "PersonName")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "AccionistaField")]
			public string Accionista { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoGeneralField")]
			public string CorreoGeneral { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoBancosField")]
			public string CorreoBancos { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoFiscalField")]
			public string CorreoFiscal { get; set; } = string.Empty;

			[Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.Numeric, ErrorMessage = "Numeric")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "PhoneNumberField")]
			public string Telefono { get; set; } = string.Empty;

			public ArchivoModel?[] Archivos { get; set; } = Array.Empty<ArchivoModel>();
		}

		public class ArchivoModel
		{
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

		public EmpresasModel(
			IEmpresaManager empresaManager,
			IArchivoEmpresaManager archivoEmpresaManager,
			IStringLocalizer<EmpresasModel> stringLocalizer,
			ILogger<EmpresasModel> logger,
			ApplicationDbContext db
		)
		{
			_empresaManager = empresaManager;
			_archivoEmpresaManager = archivoEmpresaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
			_db = db;

			InputFiltro = new FiltroModel();
			InputEmpresa = new EmpresaModel();
			InputImportar = new ImportarModel();
		}

		private async Task<string> getListaEmpresas(FiltroModel? filtro = null)
		{
			string jsonResponse;
			List<string> jsonEmpresas = new List<string>();
			List<Empresa> empresas = await _empresaManager.GetAllAsync();

			if(filtro != null)
			{
				if(filtro.Origen != null) { empresas = empresas.Where(e => e.Origen == filtro.Origen).ToList(); }
				if (filtro.Nivel != null) { empresas = empresas.Where(e => e.Nivel == filtro.Nivel).ToList(); }
				if (filtro.Administrador != null) { empresas = empresas.Where(e => e.Administrador == filtro.Administrador).ToList(); }
				if (filtro.Accionista != null) { empresas = empresas.Where(e => e.Accionista == filtro.Accionista).ToList(); }
			}

			foreach (Empresa e in empresas)
			{
				List<string> jsonArchivos;

				jsonArchivos = getListJsonArchivos(e.ArchivosEmpresa);

				jsonEmpresas.Add(
					"{" +
						$"\"id\": {e.Id}," +
						$"\"razonSocial\": \"{e.RazonSocial}\", " +
						$"\"origen\": \"{e.Origen}\", " +
						$"\"nivel\": \"{e.Nivel}\", " +
						$"\"rfc\": \"{e.RFC}\", " +
						$"\"domicilioFiscal\": \"{e.DomicilioFiscal}\", " +
						$"\"administrador\": \"{e.Administrador}\", " +
						$"\"accionista\": \"{e.Accionista}\", " +
						$"\"correoGeneral\": \"{e.CorreoGeneral}\", " +
						$"\"correoBancos\": \"{e.CorreoBancos}\", " +
						$"\"correoFiscal\": \"{e.CorreoFiscal}\", " +
						$"\"telefono\": \"{e.Telefono}\", " +
						$"\"archivos\": [{string.Join(",", jsonArchivos)}] " +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

			return jsonResponse;
		}
		private List<string> getListJsonArchivos(ICollection<ArchivoEmpresa>? archivos)
		{
			List<string> jsonArchivos = new List<string>();
			if (archivos != null)
			{
				//Si la empresa ya tiene archivos, se llena el arreglo de datos a partir de ellos.					
				List<ArchivoEmpresa> empresaFiles = (from empresaFile in archivos
												   orderby empresaFile.TipoArchivoId ascending
												   select empresaFile).ToList();

				foreach (ArchivoEmpresa a in empresaFiles)
				{
					string htmlContainer = string.Empty;
					string imgSrc = string.Empty;
					string id = Guid.NewGuid().ToString();
					//Si el archivo tiene contenido
					if (a.Archivo != null && a.Archivo.Length >= 1)
					{
						//Asigna la información del archivo al arreglo de datos.
						string b64 = Convert.ToBase64String(a.Archivo);
						bool isJPG = a.Extension == "jpg" || a.Extension == "jpeg";
						bool isPNG = a.Extension == "png";
						bool isPDF = a.Extension == "pdf";

						if (isPDF)
						{
							imgSrc = $"data:application/pdf;base64,{b64}";
							htmlContainer = $"<canvas id = '{id}' b64 = '{b64}' class = 'canvaspdf'></canvas>";
						}
						else if (isJPG || isPNG)
						{
							if (isJPG)
							{
								imgSrc = $"data:image/jpeg;base64,{b64}";
							}
							else if (isPNG)
							{
								imgSrc = $"data:image/png;base64,{b64}";
							}
							htmlContainer = $"<img id = '{id}' src = '{imgSrc}' style='max-height: 200px;'/>";
						}
					}

					jsonArchivos.Add(
						"{" +
							$"\"id\": \"{a.Id}\"," +
							$"\"nombre\": \"{a.Nombre}\"," +
							$"\"tipoArchivoId\": {a.TipoArchivoId}," +
							$"\"extension\": \"{a.Extension}\"," +
							$"\"imgSrc\": \"{imgSrc}\"," +
							$"\"htmlContainer\": \"{htmlContainer}\"" +
						"}"
					);
				}
			}
			else
			{
				//Si la empresa no tiene archivos, se llena el arreglo de datos a partir del enum.
				foreach (Data.Entities.Empresas.FileTypes i in Enum.GetValues(typeof(Data.Entities.Empresas.FileTypes)))
				{
					string imgSrc = string.Empty;
					string htmlContainer = string.Empty;
					//Si el tipo de archivo es Imagen de perfil, entonces agrega valor default.
					if ((int)i == 0) { imgSrc = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/img/default_profile_pic.png"; }
					if ((int)i != 0) { htmlContainer = "<i class='bi bi-file-image opacity-50' style='font-size:105px'></i>"; }
					jsonArchivos.Add(
						"{" +
							$"\"id\": \"{Guid.NewGuid()}\"," +
							$"\"nombre\": \"\"," +
							$"\"tipoArchivoId\": {(int)i}," +
							$"\"extension\": \"\"," +
							$"\"imgSrc\": \"{imgSrc}\"," +
							$"\"htmlContainer\": \"{htmlContainer}\"" +
						"}"
					);
				}
			}

			return jsonArchivos;
		}

		public ActionResult OnGetDownloadPlantilla()
		{
			return File("/templates/PlantillaEmpresas.xlsx", MediaTypeNames.Application.Octet, "PlantillaEmpresas.xlsx");
		}

		public async Task<JsonResult> OnPostFiltrarEmpresas()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresasFiltradasUnsuccessfully"]);
			try
			{
				resp.Datos = await getListaEmpresas(InputFiltro);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpresasFiltradasSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		
		public async Task<JsonResult> OnPostDeleteEmpresas(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresasDeletedUnsuccessfully"]);
			await _db.Database.BeginTransactionAsync();
			try
			{
                foreach (string id in ids)
                {
					int intId = Convert.ToInt32(id);
					//Elimina dependencias y posteriormente la empresa.
					await _archivoEmpresaManager.DeleteByEmpresaIdAsync(intId);
					await _empresaManager.DeleteByIdAsync(intId);
                }

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["EmpresasDeletedSuccessfully"];

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				await _db.Database.RollbackTransactionAsync();
			}

			return new JsonResult(resp);
		}
		
		public async Task<JsonResult> OnPostSaveEmpresa()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresaSavedUnsuccessfully"]);

			//Se remueve el campo Plantilla para que no sea validado ya que no pertenece a este proceso.
			ModelState.Remove("Plantilla");

			if (!ModelState.IsValid)
			{
				resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				return new JsonResult(resp);
			}
			try
			{
				//Valida que no exista una empresa registrada con los mismos datos. En caso de haber, se deja el mensaje en resp.Mensajes para ser mostrado al usuario.
				resp.Mensaje = await validarSiExisteEmpresa(InputEmpresa, false);

				//Si la longitud del mensaje de respuesta es menor o igual a cero, se considera que no hubo errores anteriores.
				if ((resp.Mensaje ?? "").Length <= 0)
				{
					//Procede a crear o actualizar la empresa.
					await createOrUpdateCompany(InputEmpresa);

					resp.TieneError = false;
					resp.Mensaje = _strLocalizer["EmpresaSavedSuccessfully"];
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> validarSiExisteEmpresa(EmpresaModel emp, bool rfcAsKey)
		{
			List<Empresa> coincidences = new List<Empresa>();
			List<Empresa> emps = await _empresaManager.GetAllAsync();

			if (rfcAsKey)
			{
				//Excluyo a la empresa con el RFC actual.
				emps = emps.Where(e => e.RFC != emp.RFC).ToList();
			}
			else
			{
				//Excluyo a la empresa con el Id actual.
				emps = emps.Where(e => e.Id != emp.Id).ToList();
			}

			//Valido que no exista empresa que tenga los mismos datos.
			coincidences = emps.Where(e => (e.CorreoGeneral ?? "").Length >= 1 && e.CorreoGeneral == emp.CorreoGeneral).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe una empresa registrada con el correo general {emp.CorreoGeneral}. Por favor verifique la información"; }

			coincidences = emps.Where(e => (e.CorreoBancos ?? "").Length >= 1 && e.CorreoBancos == emp.CorreoBancos).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe una empresa registrada con el correo bancos {emp.CorreoBancos}. Por favor verifique la información"; }

			coincidences = emps.Where(e => (e.CorreoFiscal ?? "").Length >= 1 && e.CorreoFiscal == emp.CorreoFiscal).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe una empresa registrada con el correo fiscal {emp.CorreoFiscal}. Por favor verifique la información"; }

			coincidences = emps.Where(e => (e.RFC ?? "").Length >= 1 && e.RFC == emp.RFC).ToList();
			if (coincidences.Count() >= 1) { return $"Ya existe una empresa registrada con el RFC {emp.RFC}. Por favor verifique la información"; }

			return string.Empty;
		}
		private async Task createOrUpdateCompany(EmpresaModel e)
		{
			try
			{
				await _db.Database.BeginTransactionAsync();

				int idEmpresa = 0;

				//Se busca empresa por id
				Empresa? empresa = await _empresaManager.GetByIdAsync(e.Id);
				//Si no se encontró empresa por id, se busca el empresa por su RFC. 
				if (empresa == null) { empresa = await _empresaManager.GetByRFCAsync(e.RFC ?? string.Empty); }

				//Si se encontró empresa, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
				if (empresa != null) { idEmpresa = empresa.Id; } else { empresa = new Empresa(); }

				//Llena los datos de la empresa.
				empresa.RazonSocial = e.RazonSocial;
				empresa.Origen = e.Origen;
				empresa.Nivel = e.Nivel;
				empresa.RFC = e.RFC ?? string.Empty;
				empresa.DomicilioFiscal = e.DomicilioFiscal ?? string.Empty;
				empresa.Administrador = e.Administrador ?? string.Empty;
				empresa.Accionista = e.Accionista ?? string.Empty;
				empresa.CorreoGeneral = e.CorreoGeneral;
				empresa.CorreoBancos = e.CorreoBancos;
				empresa.CorreoFiscal = e.CorreoFiscal;
				empresa.Telefono = e.Telefono;

				if (idEmpresa >= 1)
				{
					//Si la empresa ya existía, la actualiza.
					await _empresaManager.UpdateAsync(empresa);

					//Elimina los archivos de la empresa.
					await _archivoEmpresaManager.DeleteByEmpresaIdAsync(idEmpresa);
				}
				else
				{
					//De lo contrario, crea a la empresa y obtiene su id.
					idEmpresa = await _empresaManager.CreateAsync(empresa);
				}

				//Crea los archivos de la empresa.
				foreach (ArchivoModel? a in e.Archivos)
				{
					if (a != null)
					{
						await _archivoEmpresaManager.CreateAsync(
							new ArchivoEmpresa() { Archivo = (a.imgSrc ?? string.Empty).Length >= 1 ? Convert.FromBase64String(a.imgSrc ?? string.Empty) : Array.Empty<byte>(), EmpresaId = idEmpresa, Extension = a.extension ?? string.Empty, Nombre = a.nombre ?? string.Empty, TipoArchivoId = a.tipoArchivoId }
						);
					}
				}


				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await _db.Database.RollbackTransactionAsync();
				throw;
			}
		}

		public async Task<JsonResult> OnPostImportarEmpresas()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresasImportadasUnsuccessfully"]);
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
								if (result.Tables[0].Rows.IndexOf(row) == 0) { continue; }

								string vmsg = await CreateCompanyFromExcelRow(row);

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
									resp.Mensaje = _strLocalizer["EmpresasImportadasSuccessfully"];
								}
							}
						}
					}
				}

				if (!resp.TieneError)
				{
					
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> CreateCompanyFromExcelRow(DataRow row)
		{
			string validationMsg = string.Empty;

			EmpresaModel e = new EmpresaModel() {
				RazonSocial = row[0].ToString() ?? string.Empty,
				Origen = row[0].ToString() ?? string.Empty,
				Nivel = row[0].ToString() ?? string.Empty,
				RFC = row[20].ToString() ?? string.Empty,
				DomicilioFiscal = row[0].ToString() ?? string.Empty,
				Administrador = row[0].ToString() ?? string.Empty,
				Accionista = row[0].ToString() ?? string.Empty,
				CorreoGeneral = row[0].ToString() ?? string.Empty,
				CorreoBancos = row[0].ToString() ?? string.Empty,
				CorreoFiscal = row[0].ToString() ?? string.Empty,
				Telefono = row[0].ToString() ?? string.Empty,
			};

			List<ArchivoModel> archivos = new List<ArchivoModel>();
			//Crea los archivos de la empresa.
			foreach (Data.Entities.Empresas.FileTypes i in Enum.GetValues(typeof(Data.Entities.Empresas.FileTypes)))
			{
				archivos.Add(new ArchivoModel() { extension = "", imgSrc = "", nombre = "", tipoArchivoId = (int)i });
			}

			e.Archivos = archivos.ToArray();

			//Valida que no exista una empresa registrada con los mismos datos. En caso de haber, se deja el mensaje en resp.Mensajes para ser mostrado al usuario.
			validationMsg = await validarSiExisteEmpresa(e, true);

			//Si la longitud del mensaje de respuesta es menor o igual a cero, se considera que no hubo errores anteriores.
			if ((validationMsg ?? "").Length <= 0)
			{
				//Procede a crear o actualizar la empresa.
				await createOrUpdateCompany(e);
			}

			return validationMsg ?? "";
		}
	}
}