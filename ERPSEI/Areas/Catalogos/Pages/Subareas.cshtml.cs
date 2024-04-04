using ERPSEI.Data;
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
	public class SubareasModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		private readonly IEmpleadoManager _empleadoManager;
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
			ApplicationDbContext db,
			IEmpleadoManager empleadoManager,
			IRWCatalogoManager<Subarea> subareaManager,
			IRWCatalogoManager<Area> areaManager,
			IStringLocalizer<SubareasModel> stringLocalizer,
			ILogger<SubareasModel> logger
		)
		{
			Input = new InputModel();
			_db = db;
			_empleadoManager = empleadoManager;
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
				await _db.Database.BeginTransactionAsync();

                foreach (string id in ids)
                {
					int sid = 0;
					if (!int.TryParse(id, out sid)) { sid = 0; }
					List<Empleado> empleados = await _empleadoManager.GetAllAsync(null, null, null, null, null, null, sid, null, true);
					foreach(Empleado e in empleados)
					{
						e.SubareaId = null;
						await _empleadoManager.UpdateAsync(e);
					}
					await _subareaManager.DeleteByIdAsync(sid);
                }

				await _db.Database.CommitTransactionAsync();

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["SubareasDeletedSuccessfully"];
			}
			catch (Exception ex)
			{
				await _db.Database.RollbackTransactionAsync();
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
					Subarea? subarea = await _subareaManager.GetByNameAsync(Input.Nombre);

					if (subarea != null && subarea.Id != Input.Id)
					{
						resp.Mensaje = _strLocalizer["ErrorSubareaExistente"];
					}
					else
					{
						int id = 0;
						//Se busca por id.
						subarea = await _subareaManager.GetByIdAsync(Input.Id);

						if (subarea != null) { id = subarea.Id; } else { subarea = new Subarea(); }

						subarea.Nombre = Input.Nombre;
						subarea.AreaId = Input.IdArea;

						if (id >= 1)
						{
							await _subareaManager.UpdateAsync(subarea);
						}
						else
						{
							await _subareaManager.CreateAsync(subarea);

						}

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
