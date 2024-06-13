using ERPSEI.Data;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Empleados;
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
		private readonly AppUserManager _usuarioManager;
		private readonly AppRoleManager _roleManager;
		private readonly IStringLocalizer<UsuariosModel> _strLocalizer;
		private readonly ILogger<UsuariosModel> _logger;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public RolModel InputRol { get; set; }

		public class RolModel
		{
			public string Id { get; set; } = string.Empty;

			[Required(ErrorMessage = "Required")]
			[Display(Name = "RolField")]
			public string RolId { get; set; } = string.Empty;

			[Display(Name = "FullNameField")]
			public string NombreRol { get; set; } = string.Empty;
		}

		public RolesModel(
			IAccesoModuloManager accesoModuloManager,
			IModuloManager moduloManager,
			AppUserManager usuarioManager,
			AppRoleManager roleManager,
			IStringLocalizer<UsuariosModel> stringLocalizer,
			ILogger<UsuariosModel> logger,
			ApplicationDbContext db
		)
		{ 
			_accesoModuloManager = accesoModuloManager;
			_moduloManager = moduloManager;
			_usuarioManager = usuarioManager;
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
									$"\"puedeTodo\": \"{acceso?.PuedeConsultar == 1 && acceso?.PuedeEditar == 1 && acceso?.PuedeEliminar == 1 && acceso?.PuedeAutorizar == 1}\"," +
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
				await _db.Database.BeginTransactionAsync();

				//Procede a actualizar el usuario.
				await updateUser(InputRol);

				await _db.Database.CommitTransactionAsync();

				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["SavedSuccessfully"];
			}
			catch (Exception ex)
			{
				await _db.Database.RollbackTransactionAsync();
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task updateUser(RolModel e)
		{
			//Se busca usuario por id
			AppUser? usuario = await _usuarioManager.FindByIdAsync(e.Id);
			AppRole? rol = await _roleManager.FindByIdAsync(e.RolId);

			//Si se encontró usuario, obtiene su Id del registro existente.
			if (usuario != null && rol != null) {
				//Llena los datos del usuario.
				if (await _usuarioManager.IsInRoleAsync(usuario, ServicesConfiguration.RolAdministrador) && rol.Name != ServicesConfiguration.RolAdministrador)
				{
					await _usuarioManager.RemoveFromRoleAsync(usuario, ServicesConfiguration.RolAdministrador);
					await _usuarioManager.AddToRoleAsync(usuario, rol.Name ?? ServicesConfiguration.RolUsuario);
				}
				else if (await _usuarioManager.IsInRoleAsync(usuario, ServicesConfiguration.RolUsuario) && rol.Name != ServicesConfiguration.RolUsuario)
				{
					await _usuarioManager.RemoveFromRoleAsync(usuario, ServicesConfiguration.RolUsuario);
					await _usuarioManager.AddToRoleAsync(usuario, rol.Name ?? ServicesConfiguration.RolUsuario);
				}
				else
				{
					await _usuarioManager.AddToRoleAsync(usuario, rol.Name ?? ServicesConfiguration.RolUsuario);
				}
			}
		}
	}
}