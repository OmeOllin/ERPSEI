using ERPSEI.Data.Entities.Empleados;
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
    public class AreasModel : PageModel
    {
		private readonly IRWCatalogoManager<Area> _areaManager;
		private readonly IStringLocalizer<AreasModel> _strLocalizer;
		private readonly ILogger<AreasModel> _logger;

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

		public AreasModel(
			IRWCatalogoManager<Area> areaManager,
			IStringLocalizer<AreasModel> stringLocalizer,
			ILogger<AreasModel> logger
		)
		{
			Input = new InputModel();
			_areaManager = areaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetAreasList()
        {
			List<Area> areas = _areaManager.GetAllAsync().Result;

			return new JsonResult(areas);
		}

		public async Task<JsonResult> OnPostDeleteAreas(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["AreasDeletedUnsuccessfully"]);
			try
			{
				await _areaManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["AreasDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveArea()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["AreaSavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					//Busca el área por Id
					Area? area = await _areaManager.GetByIdAsync(Input.Id);

					if (area != null)
					{
						//El área ya existe, por lo que solo se actualiza.
						area.Nombre = Input.Nombre;
						await _areaManager.UpdateAsync(area);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["AreaSavedSuccessfully"];
					}
					else
					{
						//Se busca si ya existe un área con el mismo nombre.
						area = await _areaManager.GetByNameAsync(Input.Nombre);
						if (area != null) {
							resp.Mensaje = _strLocalizer["ErrorAreaExistente"];
						}
						else
						{
							await _areaManager.CreateAsync(new Area() { Nombre = Input.Nombre });

							resp.TieneError = false;
							resp.Mensaje = _strLocalizer["AreaSavedSuccessfully"];
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
