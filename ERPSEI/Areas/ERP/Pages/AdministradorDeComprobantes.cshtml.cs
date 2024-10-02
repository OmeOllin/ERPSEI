using ERPSEI.Data;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using ERPSEI.Data.Entities.SAT.Catalogos;
using ERPSEI.Data.Entities.SAT.cfdiv40;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Data.Managers.SAT;
using ERPSEI.Data.Managers.SAT.Catalogos;
using ERPSEI.Data.Managers.Usuarios;
using ERPSEI.Pages.Shared;
using ERPSEI.Requests;
using ERPSEI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NuGet.Packaging;
using ServicioEDICOM;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using XSDToXML.Utils;

namespace ERPSEI.Areas.ERP.Pages
{
	[Authorize(Policy = "AccessPolicy")]
	public class AdministradorDeComprobantesModel(
			CFDi clienteEDICOM,
			ApplicationDbContext db,
			AppUserManager userManager,
			IRWCatalogoManager<Perfil> perfilManager,
			IEmpresaManager empresaManager,
			IPrefacturaManager prefacturaManager,
			ITasaOCuotaManager tasaOCuotaManager,
			IStringLocalizer<PrefacturasModel> localizer,
			ILogger<PrefacturasModel> logger,
			IConfiguration configuration,
			IArchivoEmpresaManager archivoEmpresaManager,
			IEncriptacionAES encriptacionAES
		) : ERPPageModel
	{

		[BindProperty]
		public FiltroModel InputFiltro { get; set; } = new FiltroModel();

		public class FiltroModel
		{
			[DataType(DataType.Text)]
			[Display(Name = "PeriodoField")]
			public string? Periodo { get; set; }

			[Display(Name = "EstatusField")]
			public int? EstatusId { get; set; }

			[Display(Name = "TipoField")]
			public int? TipoId { get; set; }

			[Display(Name = "FormaPagoField")]
			public int? FormaPagoId { get; set; }

			[Display(Name = "MetodoPagoField")]
			public int? MetodoPagoId { get; set; }

			[Display(Name = "UsoCFDIField")]
			public int? UsoCFDIId { get; set; }

			[Display(Name = "EmisorField")]
			public int? EmisorId { get; set; }

			[Display(Name = "ReceptorField")]
			public int? ReceptorId { get; set; }
		}

		public void OnGet()
		{
		}

		public async Task<JsonResult> OnPostFiltrar()
		{
			ServerResponse resp = new(true, localizer["ConsultadoUnsuccessfully"]);
			try
			{
				if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
				{
					resp.Datos = await GetComprobantesList(InputFiltro);
					resp.TieneError = false;
					resp.Mensaje = localizer["ConsultadoSuccessfully"];
				}
				else
				{
					resp.Mensaje = localizer["AccesoDenegado"];
				}
			}
			catch (Exception ex)
			{
				logger.LogError("{message}", ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetComprobantesList(FiltroModel? filtro = null)
		{
			string nombreTipo;
			string nombreMoneda;
			string nombreForma;
			string nombreMetodo;
			string nombreUsoCFDI;
			string nombreExportacion;

			string jsonResponse;
			List<string> jsonPrefacturas = [];
			List<Prefactura> comprobantes = [];

			if (filtro != null)
			{
				comprobantes = await prefacturaManager.GetAllAsync(
					filtro.Fecha,
					filtro.Fecha,
					filtro.Serie,
					filtro.MonedaId,
					filtro.FormaPagoId,
					filtro.MetodoPagoId,
					filtro.UsoCFDIId,
					filtro.UsuarioCreadorId,
					filtro.UsuarioTimbradorId,
					false
				);
			}
			else
			{
				comprobantes = await prefacturaManager.GetAllAsync();
			}

			foreach (Prefactura p in comprobantes)
			{
				nombreTipo = p.TipoComprobante != null ? p.TipoComprobante.Descripcion : "";
				nombreMoneda = p.Moneda != null ? p.Moneda.Descripcion : "";
				nombreForma = p.FormaPago != null ? p.FormaPago.Descripcion : "";
				nombreMetodo = p.MetodoPago != null ? p.MetodoPago.Descripcion : "";
				nombreUsoCFDI = p.UsoCFDI != null ? p.UsoCFDI.Descripcion : "";
				nombreExportacion = p.Exportacion != null ? p.Exportacion.Descripcion : "";

				DateTime? fecha = p.Fecha == DateTime.MinValue ? null : p.Fecha;

				AppUser? usr = userManager.GetUserAsync(User).Result;
				string safeL = string.Empty;
				if (usr != null)
				{
					safeL = $"userId={usr.Id}&id={p.Id}&module=prefacturas";
					safeL = encriptacionAES.PlainTextToBase64AES(safeL);
				}

				jsonPrefacturas.Add(
					"{" +
						$"\"id\": {p.Id}," +
						$"\"safeL\": \"{safeL}\"," +
						$"\"serie\": \"{p.Serie}\", " +
						$"\"folio\": \"{p.Folio}\", " +
						$"\"emisor\": \"{p.Emisor?.RazonSocial}\", " +
						$"\"emisorId\": {p.Emisor?.Id}, " +
						$"\"receptor\": \"{p.Receptor?.RazonSocial}\", " +
						$"\"receptorId\": {p.Receptor?.Id}, " +
						$"\"tipoComprobante\": \"{nombreTipo}\", " +
						$"\"tipoComprobanteId\": {p.TipoComprobanteId}, " +
						$"\"fecha\": \"{fecha:dd/MM/yyyy HH:mm:ss}\", " +
						$"\"fechaJS\": \"{fecha:yyyy-MM-dd HH:mm:ss}\", " +
						$"\"tipoCambio\": {p.TipoCambio}, " +
						$"\"moneda\": \"{nombreMoneda}\", " +
						$"\"monedaId\": {p.MonedaId}, " +
						$"\"formaPago\": \"{nombreForma}\", " +
						$"\"formaPagoId\": {p.FormaPagoId}, " +
						$"\"metodoPago\": \"{nombreMetodo}\", " +
						$"\"metodoPagoId\": {p.MetodoPagoId}, " +
						$"\"usoCFDI\": \"{nombreUsoCFDI}\", " +
						$"\"usoCFDIId\": {p.UsoCFDIId}, " +
						$"\"exportacion\": \"{nombreExportacion}\", " +
						$"\"exportacionId\": {p.ExportacionId}, " +
						$"\"numeroOperacion\": \"{p.NumeroOperacion}\", " +
						$"\"usuarioCreadorId\": \"{p.UsuarioCreadorId}\", " +
						$"\"usuarioTimbradorId\": \"{p.UsuarioTimbradorId}\", " +
						$"\"requiereAutorizacion\": \"{p.RequiereAutorizacion}\", " +
						$"\"estatus\": \"{p.Estatus?.Descripcion}\", " +
						$"\"estatusId\": \"{p.Estatus?.Id}\", " +
						$"\"conceptos\": [] " +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonPrefacturas)}]";

			return jsonResponse;
		}

		public async Task<JsonResult> OnPostGetEmpresaSuggestion(string texto, string idempresa)
		{
			ServerResponse resp = new(true, localizer["ConsultadoUnsuccessfully"]);
			try
			{
				if (PuedeTodo || PuedeConsultar || PuedeEditar || PuedeEliminar)
				{
					if (!int.TryParse(idempresa, out int idEmp)) { idEmp = 0; }

					resp.Datos = await GetEmpresasSuggestion(texto, idEmp);
					resp.TieneError = false;
					resp.Mensaje = localizer["ConsultadoSuccessfully"];
				}
				else
				{
					resp.Mensaje = localizer["AccesoDenegado"];
				}
			}
			catch (Exception ex)
			{
				logger.LogError(message: ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetEmpresasSuggestion(string texto, int idempresa)
		{
			string jsonResponse;
			List<string> jsonEmpresas = [];

			List<EmpresaBuscada> empresas = await empresaManager.SearchEmpresas(texto);

			if (empresas != null)
			{
				foreach (EmpresaBuscada e in empresas)
				{
					Empresa? emp = await empresaManager.GetByIdWithAdicionalesAsync(e.Id);
					List<Perfil> perfiles = await perfilManager.GetAllAsync();
					perfiles = perfiles.Where(p => p.Id == emp?.PerfilId).ToList();
					Perfil? perfilEmpresa = perfiles != null && perfiles.Count >= 1 ? perfiles.First() : null;

					List<ProductoServicioPerfil> prodServEmpresa = [];
					if (perfilEmpresa != null) { prodServEmpresa = [.. perfilEmpresa.ProductosServiciosPerfil]; }

					//Si viene establecido el id empresa, omite el elemento con ese id.
					if (idempresa >= 1 && e.Id == idempresa) { continue; }

					e.ObjetoSocial = JsonEscape(e.ObjetoSocial ?? string.Empty);
					string serie = e.RFC != null ? e.RFC[..3] : string.Empty;
					List<Prefactura> prefacturas = await prefacturaManager.GetAllAsync(null, null, serie, null, null, null, null);
					prefacturas = [.. prefacturas.OrderByDescending(p => p.Id)];
					_ = int.TryParse(prefacturas.FirstOrDefault()?.Folio, out int proximoFolio);
					proximoFolio += 1;

					jsonEmpresas.Add($"{{" +
										$"\"id\": {e.Id}, " +
										$"\"value\": \"{e.RazonSocial}\", " +
										$"\"label\": \"{e.RFC} - {e.RazonSocial}\", " +
										$"\"rfc\": \"{e.RFC}\", " +
										$"\"proximoFolio\": \"{proximoFolio}\"," +
										$"\"razonSocial\": \"{e.RazonSocial}\", " +
										$"\"objetoSocial\": \"{e.ObjetoSocial}\", " +
										$"\"origen\": \"{e.Origen}\", " +
										$"\"nivel\": {{" +
														$"\"nombre\": \"{e.Nivel}\", " +
														$"\"ordinal\": \"{e.Ordinal}\", " +
														$"\"puedeFacturar\": \"{e.PuedeFacturar}\"" +
													$"}}, " +
										$"\"perfil\": \"{e.Perfil}\", " +
										$"\"domicilioFiscal\": \"{e.DomicilioFiscal}\"" +
									$"}}");
				}
			}

			jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

			return jsonResponse;
		}
		private static string JsonEscape(string str)
		{
			return str.Replace("\n", "<br />").Replace("\r", "<br />").Replace("\t", "<br />");
		}

		public async Task<JsonResult> OnPostExportExcel(string[] ids)
		{
			ServerResponse resp = new(true, localizer["PrefacturasExportedUnsuccessfully"]);

			if (PuedeTodo || PuedeConsultar)
			{
				try
				{
					await db.Database.BeginTransactionAsync();

						//El llenado de datos comienza en la fila 1 del archivo ya que la fila 0 es el encabezado que se crea junto con el excel.
						int rowIndex = 1;
						List<TasaOCuota> impuestos = await tasaOCuotaManager.GetAllAsync();
						List<TasaOCuota> impuestosIEPS = impuestos.Where(t => t.ImpuestoId == 3).ToList();
						List<TasaOCuota> impuestosIVA = impuestos.Where(t => t.ImpuestoId == 2).ToList();

						//Crea el archivo Excel
						using (HSSFWorkbook wb = await CreateExcel())
						{
							//Obtiene la primer hoja del archivo
							ISheet sheet = wb.GetSheetAt(0);
							//Crea el estilo de las celdas.
							HSSFCellStyle cellStyle = (HSSFCellStyle)wb.CreateCellStyle();

							foreach (string id in ids)
							{
								int intId = Convert.ToInt32(id);
								Prefactura? p = await prefacturaManager.GetByIdAsync(intId);
								if (p != null)
								{
									string clave = p.Serie + p.Folio.PadLeft(6, '0');
									foreach (Concepto c in p.Conceptos)
									{
										IRow row = sheet.CreateRow(rowIndex);
										//Clave
										CreateCell(row, 0, clave, cellStyle);
										//Cliente
										CreateCell(row, 1, p.ReceptorId.ToString(), cellStyle);
										//Fecha de elaboración
										CreateCell(row, 2, p.Fecha.ToString("dd/mm/yyyy"), cellStyle);
										//Su pedido
										CreateCell(row, 3, string.Empty, cellStyle);
										//Clave del artículo
										CreateCell(row, 4, c.ProductoServicio?.Clave ?? string.Empty, cellStyle);
										//Cantidad
										CreateCell(row, 5, c.Cantidad.ToString(), cellStyle);
										//Precio
										CreateCell(row, 6, c.PrecioUnitario.ToString(), cellStyle);
										//Desc. 1
										CreateCell(row, 7, string.Empty, cellStyle);
										//Desc. 2
										CreateCell(row, 8, string.Empty, cellStyle);
										//Desc. 3
										CreateCell(row, 9, string.Empty, cellStyle);
										//Clave de vendedor
										CreateCell(row, 10, string.Empty, cellStyle);
										//Comisión
										CreateCell(row, 11, string.Empty, cellStyle);
										//Clave de esquema de impuestos
										CreateCell(row, 12, string.Empty, cellStyle);
										//I.E.P.S.
										CreateCell(row, 13, GetIEPSConcepto(c, impuestosIEPS).ToString(), cellStyle);
										//Impuesto 2
										CreateCell(row, 14, string.Empty, cellStyle);
										//Impuesto 3
										CreateCell(row, 15, string.Empty, cellStyle);
										//I.V.A.
										CreateCell(row, 16, GetIVAConcepto(c, impuestosIVA).ToString(), cellStyle);
										//Impuesto 5
										CreateCell(row, 17, string.Empty, cellStyle);
										//Impuesto 6
										CreateCell(row, 18, string.Empty, cellStyle);
										//Impuesto 7
										CreateCell(row, 19, string.Empty, cellStyle);
										//Impuesto 8
										CreateCell(row, 20, string.Empty, cellStyle);
										//Método de pago
										CreateCell(row, 21, p.MetodoPago?.Clave ?? string.Empty, cellStyle);
										//Forma de Pago SAT
										CreateCell(row, 22, p.FormaPago?.Clave ?? string.Empty, cellStyle);
										//Uso CFDI
										CreateCell(row, 23, p.UsoCFDI?.Clave ?? string.Empty, cellStyle);
										//Clave SAT
										CreateCell(row, 24, c.ProductoServicio?.Clave ?? string.Empty, cellStyle);
										//Unidad SAT
										CreateCell(row, 25, c.UnidadMedida?.Clave ?? string.Empty, cellStyle);
										//Observaciones
										CreateCell(row, 26, c.Descripcion ?? string.Empty, cellStyle);
										//Observaciones de partida
										CreateCell(row, 27, string.Empty, cellStyle);
										//Fecha de entrega
										CreateCell(row, 28, string.Empty, cellStyle);
										//Fecha de vencimiento
										CreateCell(row, 29, string.Empty, cellStyle);
										//Descripcion
										CreateCell(row, 30, c.Descripcion ?? string.Empty, cellStyle);

										rowIndex++;
									}
								}
							}

							//Crea el archivo excel y lo exporta al usuario.
							using (var fileData = new FileStream("wwwroot/templates/Prefacturas.xls", FileMode.OpenOrCreate))
							{
								wb.Write(fileData);
							}

							wb.Close();
						}

						await db.Database.CommitTransactionAsync();

						resp.TieneError = false;
						resp.Mensaje = localizer["PrefacturasExportedSuccessfully"];
				}
				catch (Exception ex)
				{
					logger.LogError(message: ex.Message);
					resp.Mensaje = ex.Message;
					await db.Database.RollbackTransactionAsync();
				}
			}
			else
			{
				resp.Mensaje = localizer["AccesoDenegado"];
			}

			return new JsonResult(resp);
		}
		private static Task<HSSFWorkbook> CreateExcel()
		{
			HSSFWorkbook workbook = new();
			HSSFFont myFont = (HSSFFont)workbook.CreateFont();
			myFont.FontHeightInPoints = 11;
			myFont.FontName = "Tahoma";

			// Define un borde
			HSSFCellStyle borderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
			borderedCellStyle.SetFont(myFont);
			borderedCellStyle.BorderLeft = BorderStyle.Medium;
			borderedCellStyle.BorderTop = BorderStyle.Medium;
			borderedCellStyle.BorderRight = BorderStyle.Medium;
			borderedCellStyle.BorderBottom = BorderStyle.Medium;
			borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

			ISheet Sheet = workbook.CreateSheet("Prefacturas");
			//Creat The Headers of the excel
			IRow HeaderRow = Sheet.CreateRow(0);

			//Create The Actual Cells
			CreateCell(HeaderRow, 0, "Clave", borderedCellStyle);
			CreateCell(HeaderRow, 1, "Cliente", borderedCellStyle);
			CreateCell(HeaderRow, 2, "Fecha de elaboración", borderedCellStyle);
			CreateCell(HeaderRow, 3, "Su pedido", borderedCellStyle);
			CreateCell(HeaderRow, 4, "Clave del artículo", borderedCellStyle);
			CreateCell(HeaderRow, 5, "Cantidad", borderedCellStyle);
			CreateCell(HeaderRow, 6, "Precio", borderedCellStyle);
			CreateCell(HeaderRow, 7, "Desc. 1", borderedCellStyle);
			CreateCell(HeaderRow, 8, "Desc. 2", borderedCellStyle);
			CreateCell(HeaderRow, 9, "Desc. 3", borderedCellStyle);
			CreateCell(HeaderRow, 10, "Clave de vendedor", borderedCellStyle);
			CreateCell(HeaderRow, 11, "Comisión", borderedCellStyle);
			CreateCell(HeaderRow, 12, "Clave de esquema de impuestos", borderedCellStyle);
			CreateCell(HeaderRow, 13, "I.E.P.S.", borderedCellStyle);
			CreateCell(HeaderRow, 14, "Impuesto 2", borderedCellStyle);
			CreateCell(HeaderRow, 15, "Impuesto 3", borderedCellStyle);
			CreateCell(HeaderRow, 16, "I.V.A.", borderedCellStyle);
			CreateCell(HeaderRow, 17, "Impuesto 5", borderedCellStyle);
			CreateCell(HeaderRow, 18, "Impuesto 6", borderedCellStyle);
			CreateCell(HeaderRow, 19, "Impuesto 7", borderedCellStyle);
			CreateCell(HeaderRow, 20, "Impuesto 8", borderedCellStyle);
			CreateCell(HeaderRow, 21, "Método de pago", borderedCellStyle);
			CreateCell(HeaderRow, 22, "Forma de Pago SAT", borderedCellStyle);
			CreateCell(HeaderRow, 23, "Uso CFDI", borderedCellStyle);
			CreateCell(HeaderRow, 24, "Clave SAT", borderedCellStyle);
			CreateCell(HeaderRow, 25, "Unidad SAT", borderedCellStyle);
			CreateCell(HeaderRow, 26, "Observaciones", borderedCellStyle);
			CreateCell(HeaderRow, 27, "Observaciones de partida", borderedCellStyle);
			CreateCell(HeaderRow, 28, "Fecha de entrega", borderedCellStyle);
			CreateCell(HeaderRow, 29, "Fecha de vencimiento", borderedCellStyle);
			CreateCell(HeaderRow, 30, "Descripcion", borderedCellStyle);

			return Task.FromResult(workbook);
		}
		private static void CreateCell(IRow CurrentRow, int CellIndex, string Value, HSSFCellStyle Style)
		{
			ICell Cell = CurrentRow.CreateCell(CellIndex);
			Cell.SetCellValue(Value);
			Cell.CellStyle = Style;
		}
		private static decimal GetIEPSConcepto(Concepto c, List<TasaOCuota> impuestos)
		{
			decimal total = 0;
			if (c.ObjetoImpuestoId >= 2)
			{
				//Obtiene el valor total del IEPS
				foreach (TasaOCuota t in impuestos)
				{
					if (c.TasaTraslado == (decimal)t.ValorMaximo)
					{
						total += c.Traslado;
						break;
					}
					else if (c.TasaRetencion == (decimal)t.ValorMaximo)
					{
						total += c.Retencion;
						break;
					}
				}
			}
			return total;
		}
		private static decimal GetIVAConcepto(Concepto c, List<TasaOCuota> impuestos)
		{
			decimal total = 0;
			if (c.ObjetoImpuestoId >= 2)
			{
				//Obtiene el valor total del IVA
				foreach (TasaOCuota t in impuestos)
				{
					if (c.TasaTraslado == (decimal)t.ValorMaximo)
					{
						total += c.Traslado;
						break;
					}
					else if (c.TasaRetencion == (decimal)t.ValorMaximo)
					{
						total += c.Retencion;
						break;
					}
				}
			}
			return total;
		}

		public ActionResult OnGetDownloadExcel()
		{
			if (PuedeTodo || PuedeConsultar)
			{
				return File("/templates/Prefacturas.xls", MediaTypeNames.Application.Octet, "Prefacturas.xls");
			}
			else
			{
				return new EmptyResult();
			}
		}
		
		public async Task<JsonResult> OnPostTimbrarMultiple(string[] ids)
		{
			ServerResponse resp = new(true, localizer["PrefacturasStampedUnsuccessfully"]);

			if (PuedeTodo || PuedeEditar)
			{
				try
				{
					await db.Database.BeginTransactionAsync();

					foreach (string id in ids)
					{
						_ = int.TryParse(id, out int idPrefactura);

						//Se timbra la prefactura
						ServerResponse respTimbre = new(true, localizer["FailedToStamp"] + $" {idPrefactura}");
						if (idPrefactura >= 1){ 
							respTimbre = await TimbrarPrefactura(idPrefactura);
							if (respTimbre.TieneError) {
								//TODO: Refactorizar método para que notifique errores por prefactura.
								resp.Errores.AddRange(respTimbre.Errores);
							}
						}
					}

					await db.Database.CommitTransactionAsync();

					resp.TieneError = false;
					resp.Mensaje = localizer["PrefacturasExportedSuccessfully"];
				}
				catch (Exception ex)
				{
					logger.LogError(message: ex.Message);
					resp.Mensaje = ex.Message;
					await db.Database.RollbackTransactionAsync();
				}
			}
			else
			{
				resp.Mensaje = localizer["AccesoDenegado"];
			}

			return new JsonResult(resp);
		}
		public async Task<JsonResult> OnPostTimbrar(int idPrefactura)
		{
			ServerResponse resp = new(true, localizer["PrefacturaStampedUnsuccessfully"]);
			try
			{
				if(PuedeTodo || PuedeEditar)
				{
					//Se timbra la prefactura
					resp = await TimbrarPrefactura(idPrefactura);
				}
				else
				{
					resp.Mensaje = localizer["AccesoDenegado"];
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				logger.LogError("{message}", message);
			}

			return new JsonResult(resp);
		}
		private async Task<ServerResponse> TimbrarPrefactura(int idPrefactura)
		{
			ServerResponse resp = new(true, localizer["PrefacturaStampedUnsuccessfully"]);
			try
			{
				//Obtiene los datos de la prefactura
				Prefactura? p = await prefacturaManager.GetByIdAsync(idPrefactura);

				if (p != null)
				{
					int anio = DateTime.Now.Year;
					int mes = DateTime.Now.Month;
					int dia = DateTime.Now.Day;
					string guid = $"pref_{idPrefactura}{DateTime.Now:yyyyMMddHHmmssfffffff}";

					string pathXML = $"wwwroot/cfdiv40/xml/{anio}/{mes}/{dia}/sin_timbre/",
						XMLFilePath = $"{pathXML}{guid}.xml",
						pathZip = $"wwwroot/cfdiv40/xml/{anio}/{mes}/{dia}/con_timbre/",
						ZipFilePath = $"{pathZip}{guid}.zip",
						pathXSLT = "Utils/cadenaoriginal_4_0.xslt",
						cadenaOriginal = string.Empty;

					SelloDigital sello = new();
					Comprobante cfdi;

					if (!Path.Exists(pathXML)) { Directory.CreateDirectory(pathXML); }
					if (!Path.Exists(pathZip)) { Directory.CreateDirectory(pathZip); }

					//Se obtienen los archivos del Emisor
					List <SemiArchivoEmpresa> archivos = await archivoEmpresaManager.GetFilesByEmpresaIdAsync(p.EmisorId);

					SemiArchivoEmpresa? saCER = archivos.Where(a => a.TipoArchivoId == (int)FileTypes.CER).FirstOrDefault(),
										saKEY = archivos.Where(a => a.TipoArchivoId == (int)FileTypes.KEY).FirstOrDefault();

                    if (saCER == null || saCER.FileSize <= 0) { throw new Exception(localizer["ArchivoCERMissing"]); }
                    if (saKEY == null || saKEY.FileSize <= 0) { throw new Exception(localizer["ArchivoKEYMissing"]); }

                    byte[] fileCER = archivoEmpresaManager.GetFileById(saCER.Id)?.Archivo ?? [],
						   fileKEY = archivoEmpresaManager.GetFileById(saKEY.Id)?.Archivo ?? [];

					if (fileCER == null || fileCER.Length <= 0) { throw new Exception(localizer["ArchivoCERMissing"]); }
					if (fileKEY == null || fileKEY.Length <= 0) { throw new Exception(localizer["ArchivoKEYMissing"]); }

					//Crea el CFDI a partir de los datos de la prefactura.
					cfdi = CrearCFDIFromPrefactura(p);
					if(cfdi == null) { throw new Exception(localizer["ErrorEstructuraCFDI"]); }

					//Establece el número del certificado, obtenido a partir del archivo CER
					cfdi.NoCertificado = ObtenerNumeroCertificado(fileCER);
					if (cfdi.NoCertificado.Length <= 0) { throw new Exception(localizer["ErrorNumeroCertificado"]); }

					//Crea el XML a partir del CFDI creado y lo guarda en una ruta física del servidor.
					await CrearXMLFromCFDIAsync(cfdi, XMLFilePath);
					if (!System.IO.File.Exists(XMLFilePath)) { throw new Exception(localizer["ErrorEstructuraXML"]); }

					//Se obtiene la cadena original a partir del xml creado y usando el archivo xslt del sat.
					cadenaOriginal = ObtenerCadenaOriginal(pathXSLT, XMLFilePath);
					if (cadenaOriginal.Length <= 0) { throw new Exception(localizer["ErrorCadenaOriginal"]); }

					//Se elimina el archivo XML sin sellar
					System.IO.File.Delete(XMLFilePath);

					//Se obtiene el certificado a partir del archivo CER
					cfdi.Certificado = sello.Certificado(fileCER);
					if (cfdi.Certificado.Length <= 0) { throw new Exception(localizer["ErrorCertificado"]); }

					//Se obtiene el sello a partir del archivo KEY usando la cadena original y la clave privada
					cfdi.Sello = sello.Sellar(cadenaOriginal, fileKEY, encriptacionAES.Base64AESToPlainText(p.Emisor?.PFESAT ?? string.Empty));
					if (cfdi.Sello.Length <= 0) { throw new Exception(localizer["ErrorSello"]); }

					//Se vuelve a crear el XML sellado.
					await CrearXMLFromCFDIAsync(cfdi, XMLFilePath);
					if (!System.IO.File.Exists(XMLFilePath)) { throw new Exception(localizer["ErrorEstructuraXML"]); }

					//Se obtienen los bytes de la cadena base64 generada a partir del XML
					byte[] fileCFDI = await CrearBytesXMLAsync(XMLFilePath);
					if (fileCFDI == null || fileCFDI.Length <= 0) { throw new Exception(localizer["ErrorBytesXMLCFDI"]); }

					//Se realiza el timbrado del comprobante
					byte[] zipComprobanteTimbrado = await TimbrarComprobanteEnEDICOMAsync(configuration["ERPSEI_EDICOM_USR"] ?? string.Empty, configuration["ERPSEI_EDICOM_PWD"] ?? string.Empty, fileCFDI);
					if (zipComprobanteTimbrado == null || zipComprobanteTimbrado.Length <= 0) { throw new Exception(localizer["ErrorTimbradoCFDI"]); }

					//Se guarda el comprobante timbrado
					await GuardarZipComprobante(ZipFilePath, zipComprobanteTimbrado);
					if (!System.IO.File.Exists(ZipFilePath)) { throw new Exception(localizer["ErrorGuardadoCFDI"]); }

					//Devuelve mensaje correcto de timbrado.
					resp.TieneError = false;
					resp.Errores = [];
					resp.Mensaje = localizer["PrefacturaStampedSuccessfully"];
				}
			}
			catch (Exception ex)
			{
				//Devuelve el error en el timbrado.
				resp.Errores = [..resp.Errores.Append(ex.Message)];
			}

			return resp;
		}
		private async Task GuardarZipComprobante(string pathZip, byte[] zipComprobanteTimbrado)
		{
			string b64Zip = Convert.ToBase64String(zipComprobanteTimbrado);
			byte[] zip = Convert.FromBase64String(b64Zip);
			using (FileStream f = new(pathZip, FileMode.OpenOrCreate))
			{
				await f.WriteAsync(zip);
				System.IO.Compression.ZipFile.ExtractToDirectory(f, Path.GetDirectoryName(pathZip) ?? string.Empty);
			}
		}
		private async Task<byte[]> TimbrarComprobanteEnEDICOMAsync(string user, string password, byte[] fileB64)
		{
			byte[] zipComprobanteTimbrado;

			getCfdiTestResponse resp = new();
			getCfdiTestRequest req = new() { user = user, password = password, file = fileB64 };
			resp = await clienteEDICOM.getCfdiTestAsync(req);
			zipComprobanteTimbrado = resp.getCfdiTestReturn;

			return zipComprobanteTimbrado;
		}
		private async Task<byte[]> CrearBytesXMLAsync(string pathXML)
		{
			byte[] fileCFDI;
			using (FileStream f = new(pathXML, FileMode.Open))
			{
				fileCFDI = new byte[f.Length];
				await f.ReadAsync(fileCFDI);
			}
			return fileCFDI;
		}
		private async Task CrearXMLFromCFDIAsync(Comprobante cfdi, string pathXML)
		{
			XmlSerializerNamespaces xmlsn = new();
			xmlsn.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
			xmlsn.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
			xmlsn.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

			XmlSerializer xmlSerializer = new(typeof(Comprobante));

			string xml = string.Empty;

			using (StringWriterCustomEncoding sw = new(Encoding.UTF8))
			{
				using (XmlWriter xmlw = XmlWriter.Create(sw))
				{
					xmlSerializer.Serialize(xmlw, cfdi, xmlsn);
					xml = sw.ToString();
				}
			}

			await System.IO.File.WriteAllTextAsync(pathXML, xml);
		}
		private string ObtenerCadenaOriginal(string pathXSLT, string pathXML)
		{
			string cadenaOriginal = string.Empty;
			XslCompiledTransform transformador = new();
			XsltSettings settings = new(true, true);
			XmlUrlResolver resolver = new();
			transformador.Load(pathXSLT, settings, resolver);

			using (StringWriter sw = new())
			using (XmlWriter xmlw = XmlWriter.Create(sw, transformador.OutputSettings))
			{
				transformador.Transform(pathXML, xmlw);
				cadenaOriginal = sw.ToString();
			}

			return cadenaOriginal;
		}
		private string ObtenerNumeroCertificado(byte[] fileCER)
		{
			//Obtiene el número de certificado
			string noCertificado, x, y, z;
			SelloDigital.leerCER(fileCER, out x, out y, out z, out noCertificado);

			return noCertificado;
		}
		private string ObtenerCodigoPostal(string direccion)
		{
			return Regex.Match(direccion, "[0-9]{5}").Value;
		}

        private List<ComprobanteConcepto> CrearConceptosFromPrefactura(ICollection<Concepto> conceptos)
		{
			List<ComprobanteConcepto> lc = [];

			foreach (Concepto c in conceptos ?? [])
			{
                string umn = c.UnidadMedida?.Nombre ?? "0";
                lc.Add(new ComprobanteConcepto()
				{
					Impuestos = new()
					{
						Traslados = [new() {
							Base = 0.000001m,
							Importe = 0,
							ImporteSpecified = true,
							Impuesto = "002",
							TasaOCuota = c.TasaTraslado,
							TasaOCuotaSpecified = true,
							TipoFactor = "Tasa"
                        }],
						Retenciones = [new() {
                            Base = 0.000001m,
                            Importe = 0,
                            Impuesto = "002",
                            TasaOCuota = c.TasaRetencion,
                            TipoFactor = "Tasa"
                        }]
					},
					ClaveProdServ = c.ProductoServicio?.Clave ?? string.Empty,
					NoIdentificacion = c.ProductoServicio?.Clave ?? string.Empty,
					Cantidad = c.Cantidad,
					ClaveUnidad = c.UnidadMedida?.Clave ?? string.Empty,
					Descripcion = c.Descripcion ?? string.Empty,
					ValorUnitario = c.PrecioUnitario,
					Importe = c.Cantidad * c.PrecioUnitario,
					Descuento = c.Descuento,
					DescuentoSpecified = true,
					ObjetoImp = c.ObjetoImpuesto?.Clave ?? string.Empty,
				});
			}

			return lc;
		}
		private Comprobante CrearCFDIFromPrefactura(Prefactura p)
		{
			//Obtiene los datos de los conceptos
			List<ComprobanteConcepto> lc = CrearConceptosFromPrefactura(p.Conceptos);
			List<ComprobanteImpuestosTraslado> cit = [];
            List<ComprobanteImpuestosRetencion> cir = [];
			decimal totalTraslados = 0m;
            decimal totalRetenciones = 0m;
			decimal totalImportes = 0m;
			decimal totalDescuentos = 0m;
			string moneda = p.Moneda?.Clave ?? "MXN";

            foreach (ComprobanteConcepto cc in lc)
			{
				foreach(ComprobanteConceptoImpuestosTraslado ccit in cc.Impuestos?.Traslados ?? [])
				{
					cit.Add(new() { 
						Base = Math.Round(ccit.Base, 2),
						Importe = ccit.Importe,
						ImporteSpecified = ccit.ImporteSpecified,
						Impuesto = ccit.Impuesto,
						TasaOCuota = ccit.TasaOCuota,
						TasaOCuotaSpecified = ccit.TasaOCuotaSpecified,
						TipoFactor = ccit.TipoFactor
					});
					totalTraslados += ccit.Importe;
				}
				foreach(ComprobanteConceptoImpuestosRetencion ccir in cc.Impuestos?.Retenciones ?? [])
				{
					cir.Add(new() { 
						Importe = ccir.Importe,
						Impuesto = ccir.Impuesto
					});
					totalRetenciones += ccir.Importe;
                }

				totalImportes += cc.Importe;
				totalDescuentos += cc.Descuento;
            }

            return new()
			{
				Emisor = new()
				{
					Rfc = p.Emisor?.RFC ?? string.Empty,
					Nombre = p.Emisor?.RazonSocial ?? string.Empty,
					RegimenFiscal = p.Emisor?.RegimenFiscal?.Clave ?? string.Empty
				},
				Receptor = new()
				{
					Rfc = p.Receptor?.RFC ?? string.Empty,
					Nombre = p.Receptor?.RazonSocial ?? string.Empty,
					DomicilioFiscalReceptor = ObtenerCodigoPostal(p.Receptor?.DomicilioFiscal ?? string.Empty),
					RegimenFiscalReceptor = p.Receptor?.RegimenFiscal?.Clave ?? string.Empty,
					UsoCFDI = p.UsoCFDI?.Clave ?? string.Empty
                },
				Conceptos = [.. lc],
				Impuestos = new()
				{
					Retenciones = [.. cir],
					Traslados = [.. cit],
					TotalImpuestosRetenidos = totalRetenciones,
					TotalImpuestosRetenidosSpecified = true,
					TotalImpuestosTrasladados = totalTraslados,
					TotalImpuestosTrasladadosSpecified = true
				},
				Version = "4.0",
				Serie = p.Serie ?? string.Empty,
				Folio = p.Folio ?? string.Empty,
				Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
				Sello = string.Empty,
				FormaPago = p.FormaPago?.Clave ?? string.Empty,
				FormaPagoSpecified = true,
				NoCertificado = string.Empty,
				Certificado = string.Empty,
				SubTotal = Math.Round(totalImportes, p.Moneda?.Decimales ?? 2),
				Descuento = Math.Round(totalDescuentos, p.Moneda?.Decimales ?? 2),
                DescuentoSpecified = true,
				Moneda = moneda,
				TipoCambio = moneda == "MXN" ? 1 : p.TipoCambio,
				TipoCambioSpecified = true,
				Total = Math.Round(totalImportes - totalDescuentos + totalTraslados - totalRetenciones, p.Moneda?.Decimales ?? 2),
                TipoDeComprobante = p.TipoComprobante?.Clave ?? string.Empty,
				Exportacion = p.Exportacion?.Clave ?? string.Empty,
				MetodoPago = p.MetodoPago?.Clave ?? string.Empty,
				MetodoPagoSpecified = true,
				LugarExpedicion = ObtenerCodigoPostal(p.Emisor?.DomicilioFiscal ?? string.Empty)
            };
		}
	}
}
