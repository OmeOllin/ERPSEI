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
    public class OrigenesModel : PageModel
    {
		private readonly IRWCatalogoManager<Origen> _origenManager;
		private readonly IStringLocalizer<OrigenesModel> _strLocalizer;
		private readonly ILogger<OrigenesModel> _logger;

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
		}

		public OrigenesModel(
			IRWCatalogoManager<Origen> origenManager,
			IStringLocalizer<OrigenesModel> stringLocalizer,
			ILogger<OrigenesModel> logger
		)
		{
			Input = new InputModel();
			_origenManager = origenManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetOrigenesList()
        {
			List<Origen> origenes = _origenManager.GetAllAsync().Result;

			return new JsonResult(origenes);
		}

		public async Task<JsonResult> OnPostDeleteOrigenes(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["DeletedUnsuccessfully"]);
			try
			{
				await _origenManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["DeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveOrigen()
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
					//Busca el área por Id
					Origen? origen = await _origenManager.GetByIdAsync(Input.Id);

					if (origen != null)
					{
						//El origen ya existe, por lo que solo se actualiza.
						origen.Nombre = Input.Nombre;
						await _origenManager.UpdateAsync(origen);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SavedSuccessfully"];
					}
					else
					{
						//Se busca si ya existe un origen con el mismo nombre.
						origen = await _origenManager.GetByNameAsync(Input.Nombre);
						if (origen != null) {
							resp.Mensaje = _strLocalizer["ErrorExistente"];
						}
						else
						{
							await _origenManager.CreateAsync(new Origen() { Nombre = Input.Nombre });

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
