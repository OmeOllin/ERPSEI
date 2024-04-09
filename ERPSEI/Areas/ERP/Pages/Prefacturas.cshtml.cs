using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.ERP.Pages
{
	[Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
	public class PrefacturasModel : PageModel
	{
		private readonly IRWCatalogoManager<Perfil> _perfilManager;
		private readonly IProductoServicioManager _productosServiciosManager;
		private readonly IEmpresaManager _empresaManager;
		private readonly IStringLocalizer<PrefacturasModel> _strLocalizer;
		private readonly ILogger<PrefacturasModel> _logger;

		[BindProperty]
		public EmpresaModel InputEmpresa { get; set; }

		public class EmpresaModel()
		{

			[DataType(DataType.Text)]
			[DisplayName("SearchCompanyField")]
			[StringLength(30, ErrorMessage = "FieldLength", MinimumLength = 2)]
			public string Texto { get; set; } = string.Empty;
		}

		[BindProperty]
		public ConceptoModel InputConceptos { get; set; }

		public class ConceptoModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesis, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[Display(Name = "SearchProductServiceField")]
			public int? ProductoServicioId { get; set; }
			public int?[] ProductosServicios { get; set; } = Array.Empty<int?>();
		}

		[BindProperty]
		public CFDIModel InputCFDI { get; set; }

		public class CFDIModel
		{
			[Display(Name = "SerieField")]
			public string Serie {  get; set; } = String.Empty;
			[Display(Name = "FolioField")]
			public string Folio { get; set; } = String.Empty;
			[Display(Name = "FechaField")]
			public DateTime Fecha { get; set; }
			[Display(Name = "MonedaField")]
			public int MonedaId { get; set; }
			[Display(Name = "FormaPagoField")]
			public int FormaPagoId { get; set; }
			[Display(Name = "MetodoPagoField")]
			public int MetodoPagoId { get; set; }
			[Display(Name = "UsoCFDIField")]
			public int UsoCFDIId { get; set; }
			[Display(Name = "ExportacionField")]
			public int ExportacionId { get; set; }
		}

		public PrefacturasModel(
			IRWCatalogoManager<Perfil> perfilManager,
			IProductoServicioManager productosServiciosManager,
			IEmpresaManager empresaManager,
            IStringLocalizer<PrefacturasModel> stringLocalizer,
            ILogger<PrefacturasModel> logger
        )
        {
			_perfilManager = perfilManager;
			_productosServiciosManager = productosServiciosManager;
			_empresaManager = empresaManager;
            _strLocalizer = stringLocalizer;
            _logger = logger;

            InputEmpresa = new EmpresaModel();
			InputConceptos = new ConceptoModel();
			InputCFDI = new CFDIModel();
        }

        public void OnGet()
        {
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

					jsonEmpresas.Add($"{{" +
										$"\"id\": {e.Id}, " +
										$"\"value\": \"{e.RazonSocial}\", " +
										$"\"label\": \"{e.RFC} - {e.RazonSocial}\", " +
										$"\"rfc\": \"{e.RFC}\", " +
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
	}
}
