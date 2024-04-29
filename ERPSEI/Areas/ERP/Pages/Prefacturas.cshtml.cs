using ERPSEI.Data;
using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Data.Managers.SAT;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using iText.Html2pdf;
using System;
using System.IO;

namespace ERPSEI.Areas.ERP.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class PrefacturasModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<AppUser> _userManager;
		private readonly IRWCatalogoManager<Perfil> _perfilManager;
		private readonly IUnidadMedidaManager _unidadMedidaManager;
		private readonly IProductoServicioManager _productosServiciosManager;
		private readonly IEmpresaManager _empresaManager;
		private readonly IPrefacturaManager _prefacturaManager;
		private readonly IConceptoManager _conceptoManager;
		private readonly IStringLocalizer<PrefacturasModel> _strLocalizer;
		private readonly ILogger<PrefacturasModel> _logger;

		[BindProperty]
		public FiltroModel InputFiltro { get; set; }

		public class FiltroModel
		{
			[Display(Name = "SerieField")]
			public string? Serie { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaInicioField")]
			public DateTime? FechaInicio { get; set; }

			[DataType(DataType.DateTime)]
			[Display(Name = "FechaFinField")]
			public DateTime? FechaFin { get; set; }

			[Display(Name = "MonedaField")]
			public int? MonedaId { get; set; }

			[Display(Name = "FormaPagoField")]
			public int? FormaPagoId { get; set; }

			[Display(Name = "MetodoPagoField")]
			public int? MetodoPagoId { get; set; }

			[Display(Name = "UsoCFDIField")]
			public int? UsoCFDIId { get; set; }
		}

		[BindProperty]
		public EmpresaModel InputEmpresa { get; set; }

		public class EmpresaModel()
		{
			[DisplayName("SearchCompanyField")]
			public string? TextoEmisor { get; set; } = string.Empty;

			[DisplayName("SearchCompanyField")]
			public string? TextoReceptor { get; set; } = string.Empty;
		}

		public ConceptoModel InputConceptos { get; set; }

		public class ConceptoModel
		{
			[Required(ErrorMessage = "Required")]
			[Display(Name = "CantidadField")]
			public int? Cantidad { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "UnidadField")]
			public int? UnidadId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "DescripcionField")]
			public string? Descripcion { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "UnitarioField")]
			public decimal? Unitario { get; set; }

			[Display(Name = "DescuentoField")]
			public decimal? Descuento { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "ObjetoImpuestoField")]
			public int? ObjetoImpuestoId { get; set; }

			public TrasladoModel Traslado { get; set; } = new TrasladoModel();

			public RetencionModel Retencion { get; set; } = new RetencionModel();

			[Display(Name = "SearchProductServiceField")]
			public int? ProductoServicioId { get; set; }
		}

		public TrasladoModel InputTraslado { get; set; }
		public class TrasladoModel
		{
			[Required(ErrorMessage = "Required")]
			[Display(Name = "TrasladoField")]
			public int? TasaOCuotaId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "TrasladoField")]
			public decimal? Valor { get; set; }
		}

		public RetencionModel InputRetencion { get; set; }
		public class RetencionModel
		{
			[Required(ErrorMessage = "Required")]
			[Display(Name = "RetencionField")]
			public int? TasaOCuotaId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "RetencionField")]
			public decimal? Valor { get; set; }
		}

		[BindProperty]
		public CFDIModel InputCFDI { get; set; }

		public class CFDIModel
		{
			public int Id { get; set; }

			public int EmisorId { get; set; }

			public int ReceptorId { get; set; }

			[Required(ErrorMessage = "Required")]
			[RegularExpression(RegularExpressions.Alphabetic, ErrorMessage = "Alphabetic")]
			[Display(Name = "SerieField")]
			public string Serie { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[RegularExpression(RegularExpressions.NumericNoRestriction, ErrorMessage = "NumericNoRestriction")]
			[Display(Name = "FolioField")]
			public string Folio { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "TipoComprobanteField")]
			public int TipoComprobanteId { get; set; }

			[Required(ErrorMessage = "Required")]
			[DataType(DataType.DateTime)]
			[Display(Name = "FechaField")]
			public DateTime Fecha { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "MonedaField")]
			public int MonedaId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "TipoCambioField")]
			public decimal TipoCambio { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FormaPagoField")]
			public int FormaPagoId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "MetodoPagoField")]
			public int MetodoPagoId { get; set; }

			[Required(ErrorMessage = "Required")]
			[Display(Name = "UsoCFDIField")]
			public int UsoCFDIId { get; set; }

			[Display(Name = "ExportacionField")]
			public int? ExportacionId { get; set; }

			[Display(Name = "NumeroOperacionField")]
			[RegularExpression(RegularExpressions.NumericFirstDigitNonZero, ErrorMessage = "Numeric")]
			public int? NumeroOperacion {  get; set; }

			public ConceptoModel?[] Conceptos { get; set; } = Array.Empty<ConceptoModel>();
		}

		public PrefacturasModel(
			ApplicationDbContext db,
			UserManager<AppUser> userManager,
			IRWCatalogoManager<Perfil> perfilManager,
			IUnidadMedidaManager unidadMedidaManager,
			IProductoServicioManager productosServiciosManager,
			IEmpresaManager empresaManager,
			IPrefacturaManager prefacturaManager,
			IConceptoManager conceptoManager,
            IStringLocalizer<PrefacturasModel> stringLocalizer,
            ILogger<PrefacturasModel> logger
        )
        {
			_db = db;
			_userManager = userManager;
			_perfilManager = perfilManager;
			_unidadMedidaManager = unidadMedidaManager;
			_productosServiciosManager = productosServiciosManager;
			_empresaManager = empresaManager;
			_prefacturaManager = prefacturaManager;
			_conceptoManager = conceptoManager;
            _strLocalizer = stringLocalizer;
            _logger = logger;

			InputFiltro = new FiltroModel();
            InputEmpresa = new EmpresaModel();
			InputConceptos = new ConceptoModel();
			InputTraslado = new TrasladoModel();
			InputRetencion = new RetencionModel();
			InputCFDI = new CFDIModel();
		}

		public void OnGet()
		{
		}

		public async Task<JsonResult> OnPostDatosAdicionales(int idEmpleado)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetDatosAdicionales(idEmpleado);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		public async Task<JsonResult> OnPostFiltrar()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetPrefacturasList(InputFiltro);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetDatosAdicionales(int idPrefactura)
		{
			string jsonResponse;
			Prefactura? p = await _prefacturaManager.GetByIdWithAdicionalesAsync(idPrefactura);

			if (p == null) { throw new Exception($"No se encontró información de la prefactura id {idPrefactura}"); }

			List<string> jsonConceptos;

			jsonConceptos = getListJsonConceptos(p.Conceptos);

			jsonResponse = $"{{" +
								$"\"conceptos\": [{string.Join(",", jsonConceptos)}], " +
							$"}}";

			return jsonResponse;
		}
		private async Task<string> GetPrefacturasList(FiltroModel? filtro = null)
		{
			string nombreTipo;
			string nombreMoneda;
			string nombreForma;
			string nombreMetodo;
			string nombreUsoCFDI;
			string nombreExportacion;
			
			string jsonResponse;
			List<string> jsonPrefacturas = new List<string>();
			List<Prefactura> prefacturas;

			if (filtro != null)
			{
				prefacturas = await _prefacturaManager.GetAllAsync(
					filtro.FechaInicio,
					filtro.FechaFin,
					filtro.Serie,
					filtro.MonedaId,
					filtro.FormaPagoId,
					filtro.MetodoPagoId,
					filtro.UsoCFDIId,
					false
				);
			}
			else
			{
				prefacturas = await _prefacturaManager.GetAllAsync();
			}

			foreach (Prefactura p in prefacturas)
			{
				nombreTipo = p.TipoComprobante != null ? p.TipoComprobante.Descripcion : "";
				nombreMoneda = p.Moneda != null ? p.Moneda.Descripcion : "";
				nombreForma = p.FormaPago != null ? p.FormaPago.Descripcion : "";
				nombreMetodo = p.MetodoPago != null ? p.MetodoPago.Descripcion : "";
				nombreUsoCFDI = p.UsoCFDI != null ? p.UsoCFDI.Descripcion : "";
				nombreExportacion = p.Exportacion != null ? p.Exportacion.Descripcion : "";

				DateTime? fecha = p.Fecha == DateTime.MinValue ? null : p.Fecha;

				jsonPrefacturas.Add(
					"{" +
						$"\"id\": {p.Id}," +
						$"\"serie\": \"{p.Serie}\", " +
						$"\"folio\": \"{p.Folio}\", " +
						$"\"tipoComprobante\": \"{nombreTipo}\", " +
						$"\"tipoComprobanteId\": {p.TipoComprobanteId}, " +
						$"\"fecha\": \"{fecha:dd/MM/yyyy}\", " +
						$"\"fechaJS\": \"{fecha:yyyy-MM-dd}\", " +
						$"\"moneda\": \"{nombreMoneda}\", " +
						$"\"monedaId\": {p.TipoComprobanteId}, " +
						$"\"formaPago\": \"{nombreForma}\", " +
						$"\"formaPagoId\": {p.TipoComprobanteId}, " +
						$"\"metodoPago\": \"{nombreMetodo}\", " +
						$"\"metodoPagoId\": {p.TipoComprobanteId}, " +
						$"\"usoCFDI\": \"{nombreUsoCFDI}\", " +
						$"\"usoCFDIId\": {p.TipoComprobanteId}, " +
						$"\"exportacion\": \"{nombreExportacion}\", " +
						$"\"exportacionId\": {p.ExportacionId}, " +
						$"\"numeroOperacion\": \"{p.NumeroOperacion}\", " +
						$"\"usuarioCreadorId\": \"{p.UsuarioCreadorId}\", " +
                        $"\"usuarioAutorizadorId\": \"{p.UsuarioAutorizadorId}\", " +
                        $"\"usuarioFinalizadorId\": \"{p.UsuarioFinalizadorId}\", " +
                        $"\"conceptos\": [] " +
					"}"
				);
			}

			jsonResponse = $"[{string.Join(",", jsonPrefacturas)}]";

			return jsonResponse;
		}
		private List<string> getListJsonConceptos(ICollection<Concepto>? conceptos)
		{
			string nombreProdServ;
			string nombreUnidad;
			string nombreObjetoImpuesto;
			List<string> jsonConceptos = new List<string>();
			if (conceptos != null)
			{
				List<Concepto> concepts = (from c in conceptos
											orderby c.Id ascending
											select c).ToList();
				foreach (Concepto c in concepts)
				{
					nombreProdServ = c.ProductoServicio != null ? c.ProductoServicio.Descripcion : string.Empty;
					nombreUnidad = c.UnidadMedida != null ? c.UnidadMedida.Descripcion : string.Empty;
					nombreObjetoImpuesto = c.ObjetoImpuesto != null ? c.ObjetoImpuesto.Descripcion : string.Empty;
					jsonConceptos.Add($"{{" +
										$"\"productoServicioId\": {c.ProductoServicioId}, " +
										$"\"productoServicio\": \"{nombreProdServ}\", " +
										$"\"cantidad\": {c.Cantidad}, " +
										$"\"unitario\": {c.PrecioUnitario}, " +
										$"\"descuento\": {c.Descuento}, " +
										$"\"unidadId\": {c.UnidadMedidaId}, " +
										$"\"unidad\": \"{nombreUnidad}\", " +
										$"\"descripcion\": \"{c.Descripcion}\", " +
										$"\"objetoImpuestoId\": {c.ObjetoImpuestoId}, " +
										$"\"objetoImpuesto\": \"{nombreObjetoImpuesto}\", " +
                                        $"\"tasaTraslado\": {c.TasaTraslado}, " +
                                        $"\"tasaRetencion\":{c.TasaRetencion}" +
                                        $"\"traslado\": {c.Traslado}, " +
										$"\"retencion\":{c.Retencion}" +	
									  $"}}");
				}
			}
			return jsonConceptos;
		}

		public async Task<JsonResult> OnPostSave() {
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PrefacturaSavedUnsuccessfully"]);

			if (!ModelState.IsValid)
			{
				resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				return new JsonResult(resp);
			}
			try
			{
				string validacion = await validarSerieFolio(InputCFDI);

				//Si la longitud del mensaje de respuesta es menor o igual a cero, se considera que no hubo errores anteriores.
				if ((validacion ?? string.Empty).Length <= 0)
				{
					//Procede a crear o actualizar la prefactura.
					await createOrUpdatePrefactura(InputCFDI);

					resp.TieneError = false;
					resp.Mensaje = _strLocalizer["PrefacturaSavedSuccessfully"];
				}
				else
				{
					resp.Mensaje = validacion;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> validarSerieFolio(CFDIModel prefactura)
		{
			List<Prefactura> coincidences = new List<Prefactura>();
			List<Prefactura> prefs = await _prefacturaManager.GetAllAsync();

			//Excluyo al empleado con el Id actual.
			prefs = prefs.Where(e => e.Id != prefactura.Id).ToList();

			//Valido que no exista prefactura con misma serie y folio.
			coincidences = prefs.Where(e => e.Deshabilitado == 0 && ($"{e.Serie}{e.Folio}" == $"{prefactura.Serie}{prefactura.Folio}")).ToList();
			if (coincidences.Count() >= 1) { return $"{_strLocalizer["ErrorPrefacturaExistenteA"]} {prefactura.Serie}{prefactura.Folio}. {_strLocalizer["ErrorPrefacturaExistenteB"]}."; }

			return string.Empty;
		}
		private async Task createOrUpdatePrefactura(CFDIModel prefacturaModel)
		{
			try
			{
				await _db.Database.BeginTransactionAsync();

				int idPrefactura = 0;
				int idEstatusCreada = 1;

				//Se busca prefactura por id
				Prefactura? prefactura = await _prefacturaManager.GetByIdAsync(prefacturaModel.Id);

				//Si se encontró prefactura, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
				if (prefactura != null) { idPrefactura = prefactura.Id; } else { prefactura = new Prefactura(); }

				//Llena los datos de la prefactura.
				prefactura.EmisorId = prefacturaModel.EmisorId;
				prefactura.ReceptorId = prefacturaModel.ReceptorId;
				prefactura.Fecha = prefacturaModel.Fecha;
				prefactura.Serie = prefacturaModel.Serie;
				prefactura.Folio = prefacturaModel.Folio;
				prefactura.TipoCambio = prefacturaModel.TipoCambio;
				prefactura.ExportacionId = prefacturaModel.ExportacionId;
				prefactura.FormaPagoId = prefacturaModel.FormaPagoId;
				prefactura.MetodoPagoId = prefacturaModel.MetodoPagoId;
				prefactura.UsoCFDIId = prefacturaModel.UsoCFDIId;
				prefactura.MonedaId = prefacturaModel.MonedaId;
				prefactura.NumeroOperacion = prefacturaModel.NumeroOperacion;
				prefactura.TipoCambio = prefacturaModel.TipoCambio;
				prefactura.TipoComprobanteId = prefacturaModel.TipoComprobanteId;

				if (idPrefactura >= 1)
				{
					//Si la prefactura ya existía, la actualiza.
					await _prefacturaManager.UpdateAsync(prefactura);

					//Elimina los conceptos de la prefactura.
					await _conceptoManager.DeleteByPrefacturaIdAsync(idPrefactura);
				}
				else
				{
                    //De lo contrario...

                    //Se busca al usuario logeado
                    AppUser? u = _userManager.FindByNameAsync(User.Identity?.Name ?? "").Result;

                    //Se establece al usuario que creó la prefactura.
                    prefactura.UsuarioCreadorId = u?.Id;
					//Se establece el estatus de la prefactura en Creada
					prefactura.EstatusId = idEstatusCreada;

                    //Crea al empleado y obtiene su id.
                    idPrefactura = await _prefacturaManager.CreateAsync(prefactura);
				}

				//Crea los conceptos de la prefactura
				foreach (ConceptoModel? c in prefacturaModel.Conceptos)
				{
					if (c == null) { continue; }

					int cantidad = c.Cantidad ?? 0;

					decimal precioUnitario = c.Unitario ?? 0,
						subtotal = cantidad * precioUnitario,
						tasaTraslado = c.Traslado?.Valor ?? 0,
						tasaRetencion = c.Retencion?.Valor ?? 0,
						traslado = subtotal * tasaTraslado,
						retencion = subtotal * tasaRetencion;

					//Se usa la info para guardar el concepto.
					await _conceptoManager.CreateAsync(
						new Concepto()
						{
							ProductoServicioId = c.ProductoServicioId ?? 0,
							Cantidad = cantidad,
							PrecioUnitario = precioUnitario,
							Descuento = c.Descuento ?? 0,
							UnidadMedidaId = c.UnidadId ?? 0,
							Descripcion = c.Descripcion ?? string.Empty,
							ObjetoImpuestoId = c.ObjetoImpuestoId ?? 0,
							TasaTraslado = tasaTraslado,
							TasaRetencion = tasaRetencion,
                            Traslado = traslado,
                            Retencion = retencion,
                            PrefacturaId = idPrefactura
						}
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

		public async Task<JsonResult> OnPostGetEmpresaSuggestion(string texto, string idempresa)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				int idEmp = 0;

				if (!int.TryParse(idempresa, out idEmp)) { idEmp = 0; }

				resp.Datos = await GetEmpresasSuggestion(texto, idEmp);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetEmpresasSuggestion(string texto, int idempresa)
		{
			string jsonResponse;
			List<string> jsonEmpresas = new List<string>();

			List<EmpresaBuscada> empresas = await _empresaManager.SearchEmpresas(texto);

			if (empresas != null)
			{
				foreach (EmpresaBuscada e in empresas)
				{
					Empresa? emp = await _empresaManager.GetByIdWithAdicionalesAsync(e.Id);
					List<Perfil> perfiles = await _perfilManager.GetAllAsync();
					perfiles = perfiles.Where(p => p.Id == emp?.PerfilId).ToList();
					Perfil? perfilEmpresa = perfiles != null && perfiles.Count >= 1 ? perfiles.First() : null;

					List<ProductoServicioPerfil> prodServEmpresa = new List<ProductoServicioPerfil>();
					if (perfilEmpresa != null) { prodServEmpresa = perfilEmpresa.ProductosServiciosPerfil.ToList();  }

					//Si viene establecido el id empresa, omite el elemento con ese id.
					if (idempresa >= 1 && e.Id == idempresa) { continue; }

					e.ObjetoSocial = jsonEscape(e.ObjetoSocial ?? string.Empty);
					List<string> jsonActividades = getListJsonActividades(emp?.ActividadesEconomicasEmpresa);
					List<string> jsonProductosServicios = getListJsonProductosServicios(prodServEmpresa);
					string serie = e.RFC != null ? e.RFC.Substring(0, 3) : string.Empty;
					List<Prefactura> prefacturas = await _prefacturaManager.GetAllAsync(null, null, serie, null, null, null, null);
					prefacturas = prefacturas.OrderByDescending(p => p.Id).ToList();
					int proximoFolio = 0; 
					int.TryParse(prefacturas.FirstOrDefault()?.Folio, out proximoFolio);
					proximoFolio += 1;

					jsonEmpresas.Add($"{{" +
										$"\"id\": {e.Id}, " +
										$"\"value\": \"{e.RazonSocial}\", " +
										$"\"label\": \"{e.RFC} - {e.RazonSocial}\", " +
										$"\"rfc\": \"{e.RFC}\", " +
										$"\"proximoFolio\": \"{proximoFolio}\"," +
										$"\"razonSocial\": \"{e.RazonSocial}\", " +
										$"\"actividadesEconomicas\": [{string.Join(",", jsonActividades)}], " +
										$"\"objetoSocial\": \"{e.ObjetoSocial}\", " +
										$"\"origen\": \"{e.Origen}\", " +
										$"\"nivel\": {{" +
														$"\"nombre\": \"{e.Nivel}\", " +
														$"\"ordinal\": \"{e.Ordinal}\", " +
														$"\"puedeFacturar\": \"{e.PuedeFacturar}\"" +
													$"}}, " +
										$"\"perfil\": \"{e.Perfil}\", " +
										$"\"domicilioFiscal\": \"{e.DomicilioFiscal}\", " +
										$"\"productosServicios\": [{string.Join(",", jsonProductosServicios)}] " +
									$"}}");
				}
			}

			jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

			return jsonResponse;
		}
		private List<string> getListJsonProductosServicios(ICollection<ProductoServicioPerfil>? psp)
		{
			List<string> jsonProdServ = new List<string>();
			if (psp != null)
			{
				foreach (ProductoServicioPerfil a in psp)
				{
					if (a.ProductoServicio == null) { continue; }

					jsonProdServ.Add(
						"{" +
							$"\"id\": \"{a.ProductoServicio.Id}\"," +
							$"\"clave\": \"{a.ProductoServicio.Clave}\"" +
						"}"
					);
				}
			}

			return jsonProdServ;
		}
		private List<string> getListJsonActividades(ICollection<ActividadEconomicaEmpresa>? actividades)
		{
			List<string> jsonActividades = new List<string>();
			if (actividades != null)
			{
				foreach (ActividadEconomicaEmpresa a in actividades)
				{
					if (a.ActividadEconomica == null) { continue; }

					jsonActividades.Add(
						"{" +
							$"\"id\": \"{a.ActividadEconomica.Id}\"," +
							$"\"clave\": \"{a.ActividadEconomica.Clave}\"," +
							$"\"nombre\": \"{a.ActividadEconomica.Nombre}\"" +
						"}"
					);
				}
			}

			return jsonActividades;
		}
		private string jsonEscape(string str)
		{
			return str.Replace("\n", "<br />").Replace("\r", "<br />").Replace("\t", "<br />");
		}

		public async Task<JsonResult> OnPostGetProductosServiciosSuggestion(string texto)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetProductosServiciosSuggestion(texto);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetProductosServiciosSuggestion(string texto)
		{
			string jsonResponse;
			List<string> jsons = new List<string>();

			List<ProductoServicioBuscado> prodserv = await _productosServiciosManager.SearchProductService(texto);

			if (prodserv != null)
			{
				foreach (ProductoServicioBuscado a in prodserv)
				{
					string additional = a.PalabrasSimilares.Length >= 1 ? $" ({a.PalabrasSimilares})" : string.Empty;
					jsons.Add($"{{" +
									$"\"id\": {a.Id}, " +
									$"\"value\": \"{a.Descripcion}\", " +
									$"\"label\": \"{a.Clave} - {a.Descripcion}{additional}\", " +
									$"\"clave\": \"{a.Clave}\"" +
								$"}}");
				}
			}

			jsonResponse = $"[{string.Join(",", jsons)}]";

			return jsonResponse;
		}

		public async Task<JsonResult> OnPostGetUnidadesSuggestion(string texto)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetUnidadesSuggestion(texto);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetUnidadesSuggestion(string texto)
		{
			string jsonResponse;
			List<string> jsonUnidades = new List<string>();

			List<UnidadMedida> unidades = await _unidadMedidaManager.SearchUnidades(texto);

			if (unidades != null)
			{
				foreach (UnidadMedida u in unidades)
				{
					if(u.Simbolo.Trim().Length >= 1)
					{
                        jsonUnidades.Add($"{{" +
                                            $"\"id\": {u.Id}, " +
                                            $"\"value\": \"{u.Clave} - ({u.Simbolo}) {u.Nombre}\", " +
                                            $"\"label\": \"{u.Clave} - ({u.Simbolo}) {u.Nombre}\"" +
                                        $"}}");
                    }
					else
					{
						jsonUnidades.Add($"{{" +
											$"\"id\": {u.Id}, " +
											$"\"value\": \"{u.Clave} - {u.Nombre}\", " +
											$"\"label\": \"{u.Clave} - {u.Nombre}\"" +
										$"}}");
					}
				}
			}

			jsonResponse = $"[{string.Join(",", jsonUnidades)}]";

			return jsonResponse;
		}
	}
}
