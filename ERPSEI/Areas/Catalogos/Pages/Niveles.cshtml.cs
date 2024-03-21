using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
    [Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
    public class NivelesModel : PageModel
    {
		private readonly IRWCatalogoManager<Nivel> _catalogoManager;
		private readonly IStringLocalizer<NivelesModel> _strLocalizer;
		private readonly ILogger<NivelesModel> _logger;

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[RegularExpression(RegularExpressions.AlphanumSpaceParenthesis, ErrorMessage = "AlphanumSpaceParenthesis")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "PuedeFacturarField")]
			public bool PuedeFacturar { get; set; } = true;
		}

		public NivelesModel(
			IRWCatalogoManager<Nivel> catalogoManager,
			IStringLocalizer<NivelesModel> stringLocalizer,
			ILogger<NivelesModel> logger
		)
		{
			Input = new InputModel();
			_catalogoManager = catalogoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetNivelesList()
        {
			List<Nivel> niveles = _catalogoManager.GetAllAsync().Result;

			return new JsonResult(niveles);
		}

		public async Task<JsonResult> OnPostDeleteNiveles(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["DeletedUnsuccessfully"]);
			try
			{
				await _catalogoManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["DeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveNivel()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["SavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					//Busca el nivel por nombre
					Nivel? nivel = await _catalogoManager.GetByNameAsync(Input.Nombre);

					if (nivel != null && nivel.Id != Input.Id)
					{
						resp.Mensaje = _strLocalizer["ErrorExistente"];
					}
					else
					{
						int id = 0;
						//Se busca por id.
						nivel = await _catalogoManager.GetByIdAsync(Input.Id);

						if (nivel != null) { id = nivel.Id; } else { nivel = new Nivel(); }

						//El registro ya existe, por lo que solo se actualiza.
						nivel.Nombre = Input.Nombre;
						nivel.PuedeFacturar = Input.PuedeFacturar;

						if (id >= 1)
						{
							await _catalogoManager.UpdateAsync(nivel);
						}
						else
						{
							await _catalogoManager.CreateAsync(nivel);

						}

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SavedSuccessfully"];
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
	}
}
