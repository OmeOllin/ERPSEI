using ERPSEI.Data;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Requests;
using ERPSEI.Resources;
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
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
    public class EmpresasModel : PageModel
	{
		private readonly IEmpresaManager _empresaManager;
		private readonly IBancoEmpresaManager _bancoEmpresaManager;
		private readonly IArchivoEmpresaManager _archivoEmpresaManager;
        private readonly IRWCatalogoManager<Origen> _origenManager;
        private readonly IRWCatalogoManager<Nivel> _nivelManager;
        private readonly IRWCatalogoManager<ActividadEconomica> _actividadEconomicaManager;
        private readonly IStringLocalizer<EmpresasModel> _strLocalizer;
		private readonly ILogger<EmpresasModel> _logger;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }

		public class FiltroModel
		{
			[Display(Name = "OrigenField")]
			public int? OrigenId { get; set; }

			[Display(Name = "NivelField")]
			public int? NivelId { get; set; }

            [Display(Name = "ActividadEconomicaField")]
            public int? ActividadEconomicaId { get; set; }
        }

		[BindProperty]
		public EmpresaModel InputEmpresa { get; set; }

		public class EmpresaModel
		{
			public int Id { get; set; }

			[DataType(DataType.Text)]
			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 2)]
			[RegularExpression(RegularExpressions.CompanyName, ErrorMessage = "CompanyName")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "RazonSocialField")]
			public string RazonSocial { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "OrigenField")]
			public int? OrigenId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "NivelField")]
			public int? NivelId { get; set; }

            [DataType(DataType.DateTime)]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "FechaConstitucionField")]
            public DateTime FechaConstitucion { get; set; }

            [DataType(DataType.DateTime)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "FechaInicioOperacionField")]
			public DateTime FechaInicioOperacion { get; set; }

            [DataType(DataType.DateTime)]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "FechaInicioFacturacionField")]
            public DateTime FechaInicioFacturacion { get; set; }

            [DataType(DataType.DateTime)]
            [Required(ErrorMessage = "Required")]
            [Display(Name = "FechaInicioAsimiladosField")]
            public DateTime FechaInicioAsimilados { get; set; }

            [DataType(DataType.Text)]
			[StringLength(13, ErrorMessage = "FieldLength", MinimumLength = 12)]
			[RegularExpression(RegularExpressions.AlphanumNoSpaceNoUnderscore, ErrorMessage = "AlphanumNoSpaceNoUnderscore")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "RFCField")]
			public string RFC { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[DataType(DataType.MultilineText)]
			[StringLength(300, ErrorMessage = "FieldLength", MinimumLength = 1)]
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
			[StringLength(55, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoGeneralField")]
			public string CorreoGeneral { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[StringLength(55, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoBancosField")]
			public string CorreoBancos { get; set; } = string.Empty;

			[EmailAddress(ErrorMessage = "EmailFormat")]
			[DataType(DataType.EmailAddress)]
			[StringLength(55, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CorreoFiscalField")]
			public string CorreoFiscal { get; set; } = string.Empty;

            [EmailAddress(ErrorMessage = "EmailFormat")]
            [DataType(DataType.EmailAddress)]
			[StringLength(55, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
            [Display(Name = "CorreoFacturacionField")]
            public string CorreoFacturacion { get; set; } = string.Empty;

            [Phone(ErrorMessage = "PhoneFormat")]
			[StringLength(10, ErrorMessage = "FieldLength", MinimumLength = 10)]
			[DataType(DataType.PhoneNumber)]
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "PhoneNumberField")]
			public string Telefono { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "ActividadEconomicaField")]
            public int? ActividadEconomicaId {  get; set; }

			[DataType(DataType.MultilineText)]
			[StringLength(5000, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[Required(ErrorMessage = "Required")]
            [Display(Name = "ObjetoSocialField")]
            public string ObjetoSocial {  get; set; } = string.Empty;

            public BancoModel?[] Bancos { get; set; } = Array.Empty<BancoModel>();

            public ArchivoModel?[] Archivos { get; set; } = Array.Empty<ArchivoModel>();
		}

		public class BancoModel
		{
            public string? Banco { get; set; } = string.Empty;
            public string? Responsable { get; set; } = string.Empty;
            public string? Firmante { get; set; } = string.Empty;
        }

		public class ArchivoModel
		{
			public string? Id { get; set; } = string.Empty;
			public string? Nombre { get; set; } = string.Empty;
			public int? TipoArchivoId { get; set; }
			public string? Extension { get; set; } = string.Empty;
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
			IBancoEmpresaManager bancoEmpresaManager,
			IArchivoEmpresaManager archivoEmpresaManager,
			IRWCatalogoManager<Origen> origenManager,
			IRWCatalogoManager<Nivel> nivelManager,
			IRWCatalogoManager<ActividadEconomica> actividadEconomicaManager,
			IStringLocalizer<EmpresasModel> stringLocalizer,
			ILogger<EmpresasModel> logger,
			ApplicationDbContext db
		)
		{
			_empresaManager = empresaManager;
			_bancoEmpresaManager = bancoEmpresaManager;
			_archivoEmpresaManager = archivoEmpresaManager;
			_origenManager = origenManager;
			_nivelManager = nivelManager;
			_actividadEconomicaManager	= actividadEconomicaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
			_db = db;

			InputFiltro = new FiltroModel();
			InputEmpresa = new EmpresaModel();
			InputImportar = new ImportarModel();
		}

        private async Task<string> GetDatosAdicionalesEmpresa(int idEmpresa)
        {
            string jsonResponse = string.Empty;
            Empresa? e = await _empresaManager.GetByIdWithAdicionalesAsync(idEmpresa);

            if (e == null) { return jsonResponse; }

			List<string> jsonBancos;
			jsonBancos = getListJsonBancos(e.BancosEmpresa);

            List<string> jsonArchivos;
            List<SemiArchivoEmpresa> archivos = await _archivoEmpresaManager.GetFilesByEmpresaIdAsync(idEmpresa);
            jsonArchivos = getListJsonArchivos(archivos);

            jsonResponse = $"{{" +
								$"\"bancos\": [{string.Join(",", jsonBancos)}], " +
                                $"\"archivos\": [{string.Join(",", jsonArchivos)}] " +
                            $"}}";

            return jsonResponse;
        }
        private async Task<string> getListaEmpresas(FiltroModel? filtro = null)
		{
			string nombreOrigen;
			string nombreNivel;
			string nombreActividadEconomica;
			string jsonResponse;
			List<string> jsonEmpresas = new List<string>();
			List<Empresa> empresas;

			if(filtro != null)
			{
				empresas = await _empresaManager.GetAllAsync(
					filtro.OrigenId,
					filtro.NivelId,
					filtro.ActividadEconomicaId
				);
            }
			else
			{
				empresas = await _empresaManager.GetAllAsync();
			}

			foreach (Empresa e in empresas)
			{
				nombreOrigen = e.Origen != null ? e.Origen.Nombre : string.Empty;
				nombreNivel = e.Nivel != null ? e.Nivel.Nombre : string.Empty;
				nombreActividadEconomica = e.ActividadEconomica != null ? e.ActividadEconomica.Nombre : string.Empty;

				e.ObjetoSocial = jsonEscape(e.ObjetoSocial);

				jsonEmpresas.Add(
					"{" +
						$"\"id\": {e.Id}," +
						$"\"razonSocial\": \"{e.RazonSocial}\", " +
						$"\"origenId\": \"{e.OrigenId}\", " +
						$"\"origen\": \"{nombreOrigen}\", " +
						$"\"nivelId\": \"{e.NivelId}\", " +
						$"\"nivel\": \"{nombreNivel}\", " +
						$"\"fechaConstitucion\": \"{e.FechaConstitucion:dd/MM/yyyy}\", " +
						$"\"fechaConstitucionJS\": \"{e.FechaConstitucion:yyyy-MM-dd}\", " +
						$"\"fechaInicioOperacion\": \"{e.FechaInicioOperacion:dd/MM/yyyy}\", " +
						$"\"fechaInicioOperacionJS\": \"{e.FechaInicioOperacion:yyyy-MM-dd}\", " +
						$"\"fechaInicioFacturacion\": \"{e.FechaInicioFacturacion:dd/MM/yyyy}\", " +
						$"\"fechaInicioFacturacionJS\": \"{e.FechaInicioFacturacion:yyyy-MM-dd}\", " +
						$"\"fechaInicioAsimilados\": \"{e.FechaInicioAsimilados:dd/MM/yyyy}\", " +
						$"\"fechaInicioAsimiladosJS\": \"{e.FechaInicioAsimilados:yyyy-MM-dd}\", " +
						$"\"rfc\": \"{e.RFC}\", " +
						$"\"domicilioFiscal\": \"{e.DomicilioFiscal}\", " +
                        $"\"administrador\": \"{e.Administrador}\", " +
						$"\"accionista\": \"{e.Accionista}\", " +
                        $"\"correoGeneral\": \"{e.CorreoGeneral}\", " +
						$"\"correoBancos\": \"{e.CorreoBancos}\", " +
						$"\"correoFiscal\": \"{e.CorreoFiscal}\", " +
						$"\"correoFacturacion\": \"{e.CorreoFacturacion}\", " +
						$"\"telefono\": \"{e.Telefono}\", " +
                        $"\"actividadEconomicaId\": \"{e.ActividadEconomicaId}\", " +
						$"\"actividadEconomica\": \"{nombreActividadEconomica}\", " +
						$"\"objetoSocial\": \"{e.ObjetoSocial}\"" +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

			return jsonResponse;
		}
		private string jsonEscape(string str)
		{
			return str.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
		}
		private List<string> getListJsonBancos(ICollection<BancoEmpresa>? bancos)
        {
            List<string> jsonBancos = new List<string>();
            if (bancos != null)
            {
                foreach (BancoEmpresa b in bancos)
                {
                    string id = Guid.NewGuid().ToString();

                    jsonBancos.Add(
                        "{" +
                            $"\"id\": \"{b.Id}\"," +
                            $"\"banco\": \"{b.Banco}\"," +
                            $"\"responsable\": \"{b.Responsable}\"," +
                            $"\"firmante\": \"{b.Firmante}\"" +
                        "}"
                    );
                }
            }

            return jsonBancos;
        }
        private List<string> getListJsonArchivos(ICollection<SemiArchivoEmpresa>? archivos)
		{
			List<string> jsonArchivos = new List<string>();
			if (archivos != null)
			{
				//Si la empresa ya tiene archivos, se llena el arreglo de datos a partir de ellos.					
				List<SemiArchivoEmpresa> empresaFiles = (from empresaFile in archivos
												   orderby empresaFile.TipoArchivoId ascending
												   select empresaFile).ToList();

				foreach (SemiArchivoEmpresa a in empresaFiles)
				{
					string htmlContainer = string.Empty;
					string imgSrc = string.Empty;
					string id = Guid.NewGuid().ToString();
					//Si el archivo tiene contenido
					if (a.FileSize >= 1)
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
							$"\"htmlContainer\": \"{htmlContainer}\"," +
                            $"\"fileSize\": \"{a.FileSize}\"" +
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
							$"\"htmlContainer\": \"{htmlContainer}\"," +
                            $"\"fileSize\": \"0\"" +
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

        public async Task<JsonResult> OnPostDatosAdicionalesEmpresa(int idEmpresa)
        {
            ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresaConsultadaUnsuccessfully"]);
            try
            {
                resp.Datos = await GetDatosAdicionalesEmpresa(idEmpresa);
                resp.TieneError = false;
                resp.Mensaje = _strLocalizer["EmpresaConsultadaSuccessfully"];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new JsonResult(resp);
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
		
		public async Task<JsonResult> OnPostDisableEmpresas(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["EmpresasDeletedUnsuccessfully"]);
			await _db.Database.BeginTransactionAsync();
			try
			{
                foreach (string id in ids)
                {
					int intId = Convert.ToInt32(id);
					await _empresaManager.DisableByIdAsync(intId);
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
			coincidences = emps.Where(e => (e.RazonSocial ?? "").Length >= 1 && e.RazonSocial == emp.RazonSocial).ToList();
            if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpresaExistenteA"]} {_strLocalizer["RazonSocial"]} {emp.RazonSocial}. {_strLocalizer["ErrorEmpresaExistenteB"]}."; }

			coincidences = emps.Where(e => (e.RFC ?? "").Length >= 1 && e.RFC == emp.RFC).ToList();
			if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorEmpresaExistenteA"]} {_strLocalizer["RFC"]} {emp.RFC}. {_strLocalizer["ErrorEmpresaExistenteB"]}."; }

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
				empresa.OrigenId = e.OrigenId;
				empresa.NivelId = e.NivelId;
				empresa.FechaConstitucion = e.FechaConstitucion;
				empresa.FechaInicioOperacion = e.FechaInicioOperacion;
				empresa.FechaInicioFacturacion = e.FechaInicioFacturacion;
				empresa.FechaInicioAsimilados = e.FechaInicioAsimilados;
				empresa.RFC = e.RFC ?? string.Empty;
				empresa.DomicilioFiscal = e.DomicilioFiscal ?? string.Empty;
				empresa.Administrador = e.Administrador ?? string.Empty;
				empresa.Accionista = e.Accionista ?? string.Empty;
				empresa.CorreoGeneral = e.CorreoGeneral;
				empresa.CorreoBancos = e.CorreoBancos;
				empresa.CorreoFiscal = e.CorreoFiscal;
				empresa.CorreoFacturacion = e.CorreoFacturacion;
				empresa.Telefono = e.Telefono;
				empresa.ActividadEconomicaId = e.ActividadEconomicaId;
				empresa.ObjetoSocial = e.ObjetoSocial;

				//Los archivos actualizables serán aquellos que traigan imgSrc, pues significa que el usuario añadió el archivo en la vista.
				List<ArchivoModel?> archivosActualizables = e.Archivos.Where(a => a?.imgSrc?.Length >= 1).ToList();

                if (idEmpresa >= 1)
				{
					//Si la empresa ya existía, la actualiza.
					await _empresaManager.UpdateAsync(empresa);

					//Elimina los bancos de la empresa.
					await _bancoEmpresaManager.DeleteByEmpresaIdAsync(idEmpresa);

					//Elimina los archivos de la empresa.
					foreach(ArchivoModel? a in archivosActualizables)
					{
						if (a == null) { continue; }

						await _archivoEmpresaManager.DeleteByIdAsync(a.Id ?? string.Empty);
					}
				}
				else
				{
					//De lo contrario, crea a la empresa y obtiene su id.
					idEmpresa = await _empresaManager.CreateAsync(empresa);
				}

				//Crea los bancos de la empresa
				foreach (BancoModel? b in e.Bancos)
				{
					if(b != null)
					{
						await _bancoEmpresaManager.CreateAsync(
							new BancoEmpresa() { Banco = b.Banco??string.Empty, Responsable = b.Responsable??string.Empty, Firmante = b.Firmante??string.Empty, EmpresaId = idEmpresa }
						);
					}
				}

				//Crea los archivos de la empresa.
				foreach (ArchivoModel? a in archivosActualizables)
				{
					if (a == null) { continue; }

					await _archivoEmpresaManager.CreateAsync(
						new ArchivoEmpresa() { Archivo = (a.imgSrc ?? string.Empty).Length >= 1 ? Convert.FromBase64String(a.imgSrc ?? string.Empty) : Array.Empty<byte>(), EmpresaId = idEmpresa, Extension = a.Extension ?? string.Empty, Nombre = a.Nombre ?? string.Empty, TipoArchivoId = a.TipoArchivoId }
					);
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
								if (result.Tables[0].Rows.IndexOf(row) == 0) {
									resp.TieneError = false;
									resp.Mensaje = _strLocalizer["EmpresasImportadasSuccessfully"];
									continue; 
								}

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
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				resp.TieneError = true;
				resp.Mensaje = _strLocalizer["EmpresasImportadasUnsuccessfully"];
			}

			return new JsonResult(resp);
		}
		private async Task<string> CreateCompanyFromExcelRow(DataRow row)
        {
            string validationMsg = string.Empty;

            Origen? origen = await _origenManager.GetByNameAsync(row[1].ToString()?.Trim() ?? string.Empty);
            Nivel? nivel = await _nivelManager.GetByNameAsync(row[2].ToString()?.Trim() ?? string.Empty);
            ActividadEconomica? actividadEconomica = await _actividadEconomicaManager.GetByNameAsync(row[16].ToString()?.Trim() ?? string.Empty);

			DateTime fc;
			DateTime.TryParse(row[3].ToString()?.Trim(), out fc);
			DateTime fio;
            DateTime.TryParse(row[4].ToString()?.Trim(), out fio);
			DateTime fif;
			DateTime.TryParse(row[5].ToString()?.Trim(), out fif);
			DateTime fia;
			DateTime.TryParse(row[6].ToString()?.Trim(), out fia);

			EmpresaModel e = new EmpresaModel() {
				RazonSocial = row[0].ToString()?.Trim() ?? string.Empty,
				OrigenId = origen != null ? origen.Id : null,
				NivelId = nivel != null ? nivel.Id : null,
				FechaConstitucion = fc,
				FechaInicioOperacion = fio,
				FechaInicioFacturacion = fif,
				FechaInicioAsimilados = fia,
				RFC = row[7].ToString()?.Trim() ?? string.Empty,
				DomicilioFiscal = row[8].ToString()?.Trim() ?? string.Empty,
				Administrador = row[9].ToString()?.Trim() ?? string.Empty,
				Accionista = row[10].ToString()?.Trim() ?? string.Empty,
				CorreoGeneral = row[11].ToString()?.Trim() ?? string.Empty,
				CorreoBancos = row[12].ToString()?.Trim() ?? string.Empty,
				CorreoFiscal = row[13].ToString()?.Trim() ?? string.Empty,
				CorreoFacturacion = row[14].ToString()?.Trim() ?? string.Empty,
				Telefono = row[15].ToString()?.Trim() ?? string.Empty,
				ActividadEconomicaId = actividadEconomica != null ? actividadEconomica.Id : null,
				ObjetoSocial = row[17].ToString()?.Trim() ?? string.Empty
			};

			e.ObjetoSocial = jsonEscape(e.ObjetoSocial);

			List<BancoModel> bancos = new List<BancoModel>();
			//Si existe banco 1, agrega uno al listado.
			if (row[18].ToString()?.Length >= 1) { bancos.Add(new BancoModel() { Banco = row[18].ToString()?.Trim() ?? string.Empty, Responsable = row[19].ToString()?.Trim() ?? string.Empty, Firmante = row[20].ToString()?.Trim() ?? string.Empty }); }
			//Si existe banco 2, agrega uno al listado.
			if (row[21].ToString()?.Length >= 1) { bancos.Add(new BancoModel() { Banco = row[21].ToString()?.Trim() ?? string.Empty, Responsable = row[22].ToString()?.Trim() ?? string.Empty, Firmante = row[23].ToString()?.Trim() ?? string.Empty }); }
			//Si existe banco 3, agrega uno al listado.
			if (row[24].ToString()?.Length >= 1) { bancos.Add(new BancoModel() { Banco = row[24].ToString()?.Trim() ?? string.Empty, Responsable = row[25].ToString()?.Trim() ?? string.Empty, Firmante = row[26].ToString()?.Trim() ?? string.Empty }); }
			//Si existe banco 4, agrega uno al listado.
			if (row[27].ToString()?.Length >= 1) { bancos.Add(new BancoModel() { Banco = row[27].ToString()?.Trim() ?? string.Empty, Responsable = row[28].ToString()?.Trim() ?? string.Empty, Firmante = row[29].ToString()?.Trim() ?? string.Empty }); }
			//Si existe banco 5, agrega uno al listado.
			if (row[30].ToString()?.Length >= 1) { bancos.Add(new BancoModel() { Banco = row[30].ToString()?.Trim() ?? string.Empty, Responsable = row[31].ToString()?.Trim() ?? string.Empty, Firmante = row[32].ToString()?.Trim() ?? string.Empty }); }

			e.Bancos = bancos.ToArray();

			List<ArchivoModel> archivos = new List<ArchivoModel>();
			//Crea los archivos de la empresa.
			foreach (FileTypes i in Enum.GetValues(typeof(FileTypes)))
			{
				archivos.Add(new ArchivoModel() { Extension = "", imgSrc = "", Nombre = "", TipoArchivoId = (int)i });
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

		public async Task<JsonResult> OnPostGetActividadesEconomicasSuggestion(string texto)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetActividadesEconomicasSuggestion(texto);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetActividadesEconomicasSuggestion(string texto)
		{
			string jsonResponse;
			List<string> jsons = new List<string>();

			List<ActividadEconomica> actividades = await _actividadEconomicaManager.GetAllAsync();
			actividades = actividades.Where(e => e.Nombre.ToLowerInvariant().Contains(texto.ToLowerInvariant()) || e.Clave.ToLowerInvariant().Contains(texto.ToLowerInvariant())).ToList();

			if (actividades != null)
			{
				int counter = 0;
				int limit = 20;
				foreach (ActividadEconomica a in actividades)
				{
					//Solo se muestran los primeros 20 resultados
					if (counter >= limit) { break; }

					jsons.Add($"{{" +
										$"\"id\": {a.Id}, " +
										$"\"value\": \"{a.Nombre}\", " +
										$"\"label\": \"{a.Clave} - {a.Nombre}\", " +
										$"\"clave\": \"{a.Clave}\"" +
									$"}}");
					counter++;
				}
			}

			jsonResponse = $"[{string.Join(",", jsons)}]";

			return jsonResponse;
		}
	}
}