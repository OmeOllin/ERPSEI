using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ERPSEI.Areas.Catalogos.Pages
{
    public class PuestosModel : PageModel
    {
		private readonly IPuestoManager _puestoManager;
		private readonly IStringLocalizer<PuestosModel> _strLocalizer;
		private readonly ILogger<PuestosModel> _logger;

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[Display(Name = "Nombre")]
			public string Nombre { get; set; } = string.Empty;
		}

		public PuestosModel(
			IPuestoManager puestoManager,
			IStringLocalizer<PuestosModel> stringLocalizer,
			ILogger<PuestosModel> logger
		)
		{
			Input = new InputModel();
			_puestoManager = puestoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
		}

		public void OnGet()
        {
        }

		public JsonResult OnGetPuestosList()
		{
			List<Puesto> puestos = _puestoManager.GetAllAsync().Result;

			return new JsonResult(puestos);
		}

		public async Task<JsonResult> OnPostSavePuesto(string id, string nombre)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["PositionSavedUnsuccessfully"]);

			try
			{
				int intId = id == "Nuevo" ? 0 : int.Parse(id);

				Puesto? puesto = _puestoManager.GetById(intId);

				if(puesto != null)
				{
					puesto.Nombre = nombre;
					await _puestoManager.UpdateAsync(puesto);

					resp.Error = false;
					resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
				}
				else
				{
					await _puestoManager.CreateAsync(new Puesto() { Nombre = nombre });

					resp.Error = false;
					resp.Mensaje = _strLocalizer["PositionSavedSuccessfully"];
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
