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
	[Authorize]
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
					Puesto? puesto = await _puestoManager.GetByIdAsync(Input.Id);

					if (puesto != null)
					{
						puesto.Nombre = Input.Nombre;
						await _puestoManager.UpdateAsync(puesto);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
					}
					else
					{
						//Se busca si ya existe un puesto con el mismo nombre.
						puesto = await _puestoManager.GetByNameAsync(Input.Nombre);
						if (puesto != null)
						{
							resp.Mensaje = _strLocalizer["ErrorPuestoExistente"];
						}
						else
						{
							await _puestoManager.CreateAsync(new Puesto() { Nombre = Input.Nombre });

							resp.TieneError = false;
							resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
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
