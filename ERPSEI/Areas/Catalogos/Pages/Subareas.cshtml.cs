using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
    public class SubareasModel : PageModel
    {
		private readonly IRWCatalogoManager<Subarea> _subareaManager;
		private readonly IStringLocalizer<SubareasModel> _strLocalizer;
		private readonly ILogger<SubareasModel> _logger;

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

			[Required(ErrorMessage = "Required")]
			[Display(Name = "AreaField")]
			public int IdArea { get; set; }
		}

		public SubareasModel(
			IRWCatalogoManager<Subarea> subareaManager,
			IStringLocalizer<SubareasModel> stringLocalizer,
			ILogger<SubareasModel> logger
		)
		{
			Input = new InputModel();
			_subareaManager = subareaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetSubareasList()
		{
			List<Subarea> subareas = _subareaManager.GetAllAsync().Result;

			return new JsonResult(subareas);
		}

		public async Task<JsonResult> OnPostDeleteSubareas(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["SubareasDeletedUnsuccessfully"]);
			try
			{
				await _subareaManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["SubareasDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveSubarea()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["SubareaSavedUnsuccessfully"]);

			try
			{
				if(Input.IdArea <= 0){ ModelState.AddModelError("IdArea", _strLocalizer["IdAreaIsRequired"]); }

				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Subarea? subarea = _subareaManager.GetById(Input.Id);

					if (subarea != null)
					{
						subarea.Nombre = Input.Nombre;
						subarea.AreaId = Input.IdArea;
						await _subareaManager.UpdateAsync(subarea);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SubareaSavedSuccessfully"];
					}
					else
					{
						await _subareaManager.CreateAsync(new Subarea() { Nombre = Input.Nombre, AreaId = Input.IdArea });

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SubareaSavedSuccessfully"];
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
