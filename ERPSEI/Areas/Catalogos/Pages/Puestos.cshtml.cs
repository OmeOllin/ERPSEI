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
    public class PuestosModel : PageModel
    {
		private readonly IRWCatalogoManager<Puesto> _puestoManager;
		private readonly IStringLocalizer<PuestosModel> _strLocalizer;
		private readonly ILogger<PuestosModel> _logger;

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;
		}

		public PuestosModel(
			IRWCatalogoManager<Puesto> puestoManager,
			IStringLocalizer<PuestosModel> stringLocalizer,
			ILogger<PuestosModel> logger
		)
		{
			Input = new InputModel();
			_puestoManager = puestoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public JsonResult OnGetPuestosList()
		{
			List<Puesto> puestos = _puestoManager.GetAllAsync().Result;

			return new JsonResult(puestos);
		}

		public async Task<JsonResult> OnPostDeletePuestos(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PositionsDeletedUnsuccessfully"]);
			try
			{
				await _puestoManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["PositionsDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}

		public async Task<JsonResult> OnPostSavePuesto()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PositionSavedUnsuccessfully"]);

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					Puesto? puesto = await _puestoManager.GetByNameAsync(Input.Nombre);

					if (puesto != null && puesto.Id != Input.Id)
					{
						resp.Mensaje = _strLocalizer["ErrorPuestoExistente"];
					}
					else
					{
						int id = 0;
						puesto = await _puestoManager.GetByIdAsync(Input.Id);

						if (puesto != null) { id = puesto.Id; } else { puesto = new Puesto(); }

						puesto.Nombre = Input.Nombre;

						if(id >= 1)
						{
							await _puestoManager.UpdateAsync(puesto);
						}
						else
						{
							await _puestoManager.CreateAsync(puesto);

						}

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
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
