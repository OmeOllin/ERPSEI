using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using ERPSEI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
    [Authorize(Roles = $"{ServicesConfiguration.RolMaster}, {ServicesConfiguration.RolAdministrador}")]
    public class OficinasModel : PageModel
    {
		private readonly IRWCatalogoManager<Oficina> _oficinaManager;
		private readonly IStringLocalizer<OficinasModel> _strLocalizer;
		private readonly ILogger<OficinasModel> _logger;

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

		public OficinasModel(
			IRWCatalogoManager<Oficina> oficinaManage,
			IStringLocalizer<OficinasModel> stringLocalizer,
			ILogger<OficinasModel> logger
		)
		{
			Input = new InputModel();
			_oficinaManager = oficinaManage;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetOficinasList()
		{
			List<Oficina> oficinas = _oficinaManager.GetAllAsync().Result;

			return new JsonResult(oficinas);
		}

		public async Task<JsonResult> OnPostDeleteOficinas(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["OfficesDeletedUnsuccessfully"]);
			try
			{
				await _oficinaManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["OfficesDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSaveOficina()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["OfficeSavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Oficina? oficina = await _oficinaManager.GetByNameAsync(Input.Nombre);

					if (oficina != null && oficina.Id != Input.Id)
					{
						resp.Mensaje = _strLocalizer["ErrorOficinaExistente"];
					}
					else
					{
						int id = 0;
						//Se busca por id.
						oficina = await _oficinaManager.GetByIdAsync(Input.Id);

						if (oficina != null) { id = oficina.Id } else { oficina = new Oficina(); }

						oficina.Nombre = Input.Nombre;

						if(id >= 1)
						{
							await _oficinaManager.UpdateAsync(oficina);
						}
						else
						{
							await _oficinaManager.CreateAsync(oficina);

						}

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["OfficeSavedSuccessfully"];
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
