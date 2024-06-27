using ERPSEI.Data;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Usuarios;
using ERPSEI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ERPSEI.Areas.Catalogos.Pages
{
	[Authorize(Policy = "AccessPolicy")]
    public class RolesModel : PageModel
	{
		private readonly IAccesoModuloManager _accesoModuloManager;
		private readonly IModuloManager _moduloManager;
		private readonly AppRoleManager _roleManager;
		private readonly IStringLocalizer<RolesModel> _strLocalizer;
		private readonly ILogger<RolesModel> _logger;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public RolModel InputRol { get; set; }

		public class RolModel
		{
			public string Id { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "FullNameField")]
			public string NombreRol { get; set; } = string.Empty;

			public ModuloModel?[] Modulos { get; set; } = Array.Empty<ModuloModel>();
		}

		public class ModuloModel
		{
			public int? ModuloId { get; set; }
			public bool? PuedeTodo { get; set; } = false;
			public bool? PuedeConsultar { get; set; } = false;
			public bool? PuedeEditar { get; set; } = false;
			public bool? PuedeEliminar { get; set; } = false;
			public bool? PuedeAutorizar { get; set; } = false;
		}

		public RolesModel(
			IAccesoModuloManager accesoModuloManager,
			IModuloManager moduloManager,
			AppRoleManager roleManager,
			IStringLocalizer<RolesModel> stringLocalizer,
			ILogger<RolesModel> logger,
			ApplicationDbContext db
		)
		{ 
			_accesoModuloManager = accesoModuloManager;
			_moduloManager = moduloManager;
			_roleManager = roleManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
			_db = db;

			InputRol = new RolModel();
		}

		private async Task<string> getLista()
		{
			string jsonResponse;
			List<string> jsonModulos = new List<string>();
			List<string> jsonResultados = new List<string>();

			foreach (AppRole r in _roleManager.Roles)
			{
				switch (r.Name)
				{
					case ServicesConfiguration.RolMaster:
					case ServicesConfiguration.RolUsuario:
					case ServicesConfiguration.RolCandidato:
						//Los roles default no serán mostrados.
						break;
					default:
						foreach (Modulo m in await _moduloManager.GetAllAsync())
						{
							List<AccesoModulo> accesosRol = await _accesoModuloManager.GetByRolIdAsync(r.Id);
							AccesoModulo? acceso = accesosRol.Where(a => a.Modulo?.NombreNormalizado == m.NombreNormalizado).FirstOrDefault();
							jsonModulos.Add(
								"{" +
									$"\"nombre\": \"{m.NombreNormalizado}\"," +
									$"\"puedeTodo\": \"{acceso?.PuedeTodo == 1}\"," +
									$"\"puedeConsultar\": \"{acceso?.PuedeConsultar == 1}\"," +
									$"\"puedeEditar\": \"{acceso?.PuedeEditar == 1}\"," +
									$"\"puedeEliminar\": \"{acceso?.PuedeEliminar == 1}\"," +
									$"\"puedeAutorizar\": \"{acceso?.PuedeAutorizar == 1}\"" +
								"}"
							);
						}

						//El resto de roles se mostrará
						jsonResultados.Add(
							"{" +
								$"\"id\": \"{r.Id}\"," +
								$"\"rol\": \"{r.Name}\"," +
								$"\"modulos\": [" +
									string.Join(",", jsonModulos) +
								$"]" +
							"}"
						);
						break;
				}
			}

			jsonResponse = $"[{string.Join(",", jsonResultados)}]";

			return jsonResponse;
		}

		public async Task<JsonResult> OnPostFiltrar()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["FiltroUnsuccessfully"]);
			try
			{
				resp.Datos = await getLista();
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["FiltroSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		
		public async Task<JsonResult> OnPostSave()
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["SavedUnsuccessfully"]);

			if (!ModelState.IsValid)
			{
				resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				return new JsonResult(resp);
			}
			try
			{
				//Se busca si ya existe un registro con el mismo nombre.
				AppRole? rol = await _roleManager.GetByNameAsync(InputRol.NombreRol);

				//Si ya existe un registro con el mismo nombre y los Id's no coinciden
				if (rol != null && rol.Id != InputRol.Id)
				{
					//Ya existe un elemento con el mismo nombre.
					resp.Mensaje = _strLocalizer["ErrorExistente"];
				}
				else
				{
					//Crea o actualiza el registro
					await createOrUpdateRole(InputRol);

					resp.TieneError = false;
					resp.Mensaje = _strLocalizer["SavedSuccessfully"];
				}
			}
			catch (Exception ex)
			{
				await _db.Database.RollbackTransactionAsync();
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task createOrUpdateRole(RolModel e)
		{
			try
			{
				await _db.Database.BeginTransactionAsync();

				string idRol = "";
				//Busca el registro por Id
				AppRole? rol = await _roleManager.GetByIdAsync(e.Id);

				//Si se encontró rol, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
				if (rol != null) { idRol = rol.Id; } else { rol = new AppRole(); }

				rol.Name = e.NombreRol;

				if (idRol.Length >= 1)
				{
					//El registro ya existe, por lo que solo se actualiza.
					await _roleManager.UpdateAsync(rol);

					//Elimina los accesos del rol.
					await _accesoModuloManager.DeleteByRolIdAsync(idRol);
				}
				else
				{
					//De lo contrario, crea a la empresa y obtiene su id.
					IdentityResult result = await _roleManager.CreateAsync(rol);
					if(result.Succeeded)
					{
						idRol = rol?.Id??"";
					}
				}

				//Crea los accesos del rol
				foreach (ModuloModel? acceso in e.Modulos)
				{
					if (acceso != null)
					{
						await _accesoModuloManager.CreateAsync(
							new AccesoModulo() { 
								RolId = idRol,
								ModuloId = acceso.ModuloId, 
								PuedeTodo = acceso.PuedeTodo ?? false ? 1 : 0, 
								PuedeConsultar = acceso.PuedeConsultar ?? false ? 1 : 0, 
								PuedeEditar = acceso.PuedeEditar ?? false ? 1 : 0, 
								PuedeEliminar = acceso.PuedeEliminar ?? false ? 1 : 0, 
								PuedeAutorizar = acceso.PuedeAutorizar ?? false ? 1 : 0 
							}
						);
					}
				}

				await _db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await _db.Database.RollbackTransactionAsync();
				throw;
			}
		}
	}
}