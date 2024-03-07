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
    public class PerfilesModel : PageModel
    {
		private readonly IRWCatalogoManager<Perfil> _catalogoManager;
		private readonly IStringLocalizer<PerfilesModel> _strLocalizer;
		private readonly ILogger<PerfilesModel> _logger;

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[StringLength(50, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[RegularExpression(RegularExpressions.AlphanumSpace, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;
		}

		public PerfilesModel(
			IRWCatalogoManager<Perfil> catalogoManager,
			IStringLocalizer<PerfilesModel> stringLocalizer,
			ILogger<PerfilesModel> logger
		)
		{
			Input = new InputModel();
			_catalogoManager = catalogoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetListAll()
        {
			List<Perfil> perfiles = _catalogoManager.GetAllAsync().Result;

			return new JsonResult(perfiles);
		}

		public async Task<JsonResult> OnPostDelete(string[] ids)
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

		public async Task<JsonResult> OnPostSave()
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
                    //Busca el registro por Id
                    Perfil? perfil = await _catalogoManager.GetByIdAsync(Input.Id);

					if (perfil != null)
					{
						//El registro ya existe, por lo que solo se actualiza.
						perfil.Nombre = Input.Nombre;
						await _catalogoManager.UpdateAsync(perfil);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SavedSuccessfully"];
					}
					else
					{
						//Se busca si ya existe un registro con el mismo nombre.
						perfil = await _catalogoManager.GetByNameAsync(Input.Nombre);
						if (perfil != null) {
							resp.Mensaje = _strLocalizer["ErrorExistente"];
						}
						else
						{
							await _catalogoManager.CreateAsync(new Perfil() { Nombre = Input.Nombre });

							resp.TieneError = false;
							resp.Mensaje = _strLocalizer["SavedSuccessfully"];
						}
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
