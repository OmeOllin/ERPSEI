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
	[Authorize(Roles = $"{ServicesConfiguration.Master}, {ServicesConfiguration.Administrador}")]
	public class SubareasModel : PageModel
    {
		private readonly IRWCatalogoManager<Subarea> _subareaManager;
		private readonly IRWCatalogoManager<Area> _areaManager;
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
			IRWCatalogoManager<Area> areaManager,
			IStringLocalizer<SubareasModel> stringLocalizer,
			ILogger<SubareasModel> logger
		)
		{
			Input = new InputModel();
			_subareaManager = subareaManager;
			_areaManager = areaManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public async Task<JsonResult> OnGetSubareasList()
		{
			string nombreArea;
			string jsonResponse;
			List<string> jsonAreas = new List<string>();
			List<Subarea> subareas = _subareaManager.GetAllAsync().Result;
			foreach (Subarea sa in subareas)
			{
				sa.Area = await _areaManager.GetByIdAsync(sa.AreaId ?? 0);
				nombreArea = sa.Area != null ? sa.Area.Nombre : "";
				jsonAreas.Add("{\"id\": " + sa.Id + ", \"nombre\": \"" + sa.Nombre + "\", \"area\": \"" + nombreArea + "\", \"idArea\": " + sa.AreaId + "}");
			}

			jsonResponse = $"[{String.Join(",", jsonAreas)}]";
			return new JsonResult(jsonResponse);
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
					Subarea? subarea = await _subareaManager.GetByIdAsync(Input.Id);

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
						//Se busca si ya existe una subarea con el mismo nombre.
						subarea = await _subareaManager.GetByNameAsync(Input.Nombre);
						if (subarea != null)
						{
							resp.Mensaje = _strLocalizer["ErrorSubareaExistente"];
						}
						else
						{
							await _subareaManager.CreateAsync(new Subarea() { Nombre = Input.Nombre, AreaId = Input.IdArea });

							resp.TieneError = false;
							resp.Mensaje = _strLocalizer["SubareaSavedSuccessfully"];
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
