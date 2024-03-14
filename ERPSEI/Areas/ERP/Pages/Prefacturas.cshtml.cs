using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers.Empresas;
using ERPSEI.Requests;
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
			public string Texto {  get; set; } = string.Empty;
        }
        
        public PrefacturasModel(
			IEmpresaManager empresaManager,
            IStringLocalizer<PrefacturasModel> stringLocalizer,
            ILogger<PrefacturasModel> logger
        )
        {
			_empresaManager = empresaManager;
            _strLocalizer = stringLocalizer;
            _logger = logger;

            InputEmpresa = new EmpresaModel();
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

					//Si viene establecido el id empresa, omite el elemento con ese id.
					if (idempresa >= 1 && e.Id == idempresa) { continue; }

					e.ObjetoSocial = jsonEscape(e.ObjetoSocial ?? string.Empty);
					List<string> jsonActividades = getListJsonActividades(emp?.ActividadesEconomicasEmpresa);

					jsonEmpresas.Add($"{{" +
										$"\"id\": {e.Id}, " +
										$"\"value\": \"{e.RazonSocial}\", " +
										$"\"label\": \"{e.RFC} - {e.RazonSocial}\", " +
										$"\"rfc\": \"{e.RFC}\", " +
										$"\"razonSocial\": \"{e.RazonSocial}\", " +
										$"\"actividadesEconomicas\": [{string.Join(",", jsonActividades)}], " +
										$"\"objetoSocial\": \"{e.ObjetoSocial}\", " +
										$"\"origen\": \"{e.Origen}\", " +
										$"\"nivel\": \"{e.Nivel}\", " +
										$"\"perfil\": \"{e.Perfil}\", " +
										$"\"domicilioFiscal\": \"{e.DomicilioFiscal}\"" +
									$"}}");
				}
			}

			jsonResponse = $"[{string.Join(",", jsonEmpresas)}]";

			return jsonResponse;
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
	}
}
