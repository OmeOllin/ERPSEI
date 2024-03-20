using ERPSEI.Data.Entities.Empleados;
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
					//Se busca si ya existe un registro con el mismo nombre.
					Area? area = await _areaManager.GetByNameAsync(Input.Nombre);

					//Si ya existe un registro con el mismo nombre y los Id's no coinciden
					if (area != null && area.Id != Input.Id)
					{
						//Ya existe un elemento con el mismo nombre.
						resp.Mensaje = _strLocalizer["ErrorAreaExistente"];
					}
					else
					{
						int id = 0;
						//Busca el registro por Id
						area = await _areaManager.GetByIdAsync(Input.Id);

						//Si se encontró área, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
						if (area != null) { id = area.Id; } else { area = new Area(); }

						area.Nombre = Input.Nombre;

						//Crea o actualiza el registro
						if(id >= 1)
						{
							await _areaManager.UpdateAsync(area);
						}
						else
						{
							await _areaManager.CreateAsync(area);
						}

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["AreaSavedSuccessfully"];
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
