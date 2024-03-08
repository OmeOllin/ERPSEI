using ERPSEI.Data;
using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Managers;
using ERPSEI.Data.Managers.Empresas;
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
    public class PerfilesModel : PageModel
    {
		private readonly IRWCatalogoManager<ProductoServicio> _productosServiciosManager;
		private readonly IProductoServicioPerfilManager _productosServiciosPerfilManager;
		private readonly IRWCatalogoManager<Perfil> _catalogoManager;
		private readonly IStringLocalizer<PerfilesModel> _strLocalizer;
		private readonly ILogger<PerfilesModel> _logger;
		private readonly ApplicationDbContext _db;

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Display(Name = "Id")]
			public int Id { get; set; }

			[StringLength(100, ErrorMessage = "FieldLength", MinimumLength = 1)]
			[RegularExpression(RegularExpressions.AlphanumSpaceCommaDotParenthesis, ErrorMessage = "AlphanumSpace")]
			[Required(ErrorMessage = "Required")]
			[Display(Name = "NameField")]
			public string Nombre { get; set; } = string.Empty;

			[Display(Name = "SearchProductServiceField")]
			public int? ProductoServicioId { get; set; }
			public int?[] ProductosServicios { get; set; } = Array.Empty<int?>();
		}

		public PerfilesModel(
			IRWCatalogoManager<ProductoServicio> productosServiciosManager,
			IProductoServicioPerfilManager productosServiciosPerfilManager,
			IRWCatalogoManager<Perfil> catalogoManager,
			IStringLocalizer<PerfilesModel> stringLocalizer,
			ILogger<PerfilesModel> logger,
			ApplicationDbContext db
		)
		{
			Input = new InputModel();
			_productosServiciosManager = productosServiciosManager;
			_productosServiciosPerfilManager = productosServiciosPerfilManager;
			_catalogoManager = catalogoManager;
			_strLocalizer = stringLocalizer;
			_logger = logger;
			_db = db;
		}

		public JsonResult OnGetListAll()
        {

			string jsonResponse;
			List<string> jsonPerfiles = new List<string>();
			List<Perfil> perfiles = _catalogoManager.GetAllAsync().Result;

			foreach ( Perfil p in perfiles)
			{
				List<string> jsonProdServ = getListJsonProductosServicios(p.ProductosServiciosPerfil);
				jsonPerfiles.Add(
									"{" +
										$"\"id\": {p.Id}," +
										$"\"nombre\": \"{p.Nombre}\", " +
										$"\"productosServicios\": [{string.Join(",", jsonProdServ)}]" +
									"}"
								);
			}

			jsonResponse = $"[{string.Join(",", jsonPerfiles)}]";

			return new JsonResult(jsonResponse);
		}
		private List<string> getListJsonProductosServicios(ICollection<ProductoServicioPerfil>? productosServicios)
		{
			List<string> jsonProdServ = new List<string>();
			if (productosServicios != null)
			{
				foreach (ProductoServicioPerfil a in productosServicios)
				{
					if (a.ProductoServicio == null) { continue; }

					jsonProdServ.Add(
						"{" +
							$"\"id\": \"{a.ProductoServicio.Id}\"," +
							$"\"clave\": \"{a.ProductoServicio.Clave}\"," +
							$"\"descripcion\": \"{a.ProductoServicio.Descripcion}\"" +
						"}"
					);
				}
			}

			return jsonProdServ;
		}

		public async Task<JsonResult> OnPostDelete(string[] ids)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["DeletedUnsuccessfully"]);
			try
			{
				await _catalogoManager.DeleteMultipleByIdAsync(ids);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["DeletedSuccessfully"];
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

			try
			{
				if (!ModelState.IsValid)
				{
					resp.Errores = ModelState.Keys.SelectMany(k => ModelState[k].Errors).Select(m => m.ErrorMessage).ToArray();
				}
				else
				{
					//Se busca si ya existe un registro con el mismo nombre.
					Perfil? perfil = await _catalogoManager.GetByNameAsync(Input.Nombre);

					//Si ya existe un registro con el mismo nombre y los Id's no coinciden
					if (perfil != null && perfil.Id != Input.Id)
					{
						//Ya existe un elemento con el mismo nombre.
						resp.Mensaje = _strLocalizer["ErrorExistente"];
					}
					else
					{
						//Crea o actualiza el registro
						await createOrUpdateProfile(Input);

						resp.TieneError = false;
						resp.Mensaje = _strLocalizer["SavedSuccessfully"];
					}

				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task createOrUpdateProfile(InputModel p)
		{
			try
			{
				await _db.Database.BeginTransactionAsync();

				int idPerfil = 0;
				//Busca el registro por Id
				Perfil? perfil = await _catalogoManager.GetByIdAsync(p.Id);

				//Si se encontró perfil, obtiene su Id del registro existente. De lo contrario, se crea uno nuevo.
				if (perfil != null) { idPerfil = perfil.Id; } else { perfil = new Perfil(); }

				perfil.Nombre = p.Nombre;

				if (idPerfil >= 1)
				{
					//El registro ya existe, por lo que solo se actualiza.
					await _catalogoManager.UpdateAsync(perfil);

					//Elimina los productos y servicios del perfil.
					await _productosServiciosPerfilManager.DeleteByPerfilIdAsync(idPerfil);
				}
				else
				{
					//De lo contrario, crea a la empresa y obtiene su id.
					idPerfil = await _catalogoManager.CreateAsync(perfil);
				}

				//Crea los productos y servicios del perfil
				foreach (int? id in p.ProductosServicios)
				{
					if (id != null)
					{
						await _productosServiciosPerfilManager.CreateAsync(
							new ProductoServicioPerfil() { ProductoServicioId = id, PerfilId = idPerfil }
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

		public async Task<JsonResult> OnPostGetProductosServiciosSuggestion(string texto)
		{
			ServerResponse resp = new ServerResponse(true, _strLocalizer["ConsultadoUnsuccessfully"]);
			try
			{
				resp.Datos = await GetProductosServiciosSuggestion(texto);
				resp.TieneError = false;
				resp.Mensaje = _strLocalizer["ConsultadoSuccessfully"];
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new JsonResult(resp);
		}
		private async Task<string> GetProductosServiciosSuggestion(string texto)
		{
			string jsonResponse;
			List<string> jsons = new List<string>();

			List<ProductoServicio> prodserv = await _productosServiciosManager.GetAllAsync();
			prodserv = prodserv.Where(e => e.Descripcion.ToLowerInvariant().Contains(texto.ToLowerInvariant()) || e.Clave.ToLowerInvariant().Contains(texto.ToLowerInvariant())).Take(20).ToList();

			if (prodserv != null)
			{
				foreach (ProductoServicio a in prodserv)
				{
					jsons.Add($"{{" +
									$"\"id\": {a.Id}, " +
									$"\"value\": \"{a.Descripcion}\", " +
									$"\"label\": \"{a.Clave} - {a.Descripcion}\", " +
									$"\"clave\": \"{a.Clave}\"" +
								$"}}");
				}
			}

			jsonResponse = $"[{string.Join(",", jsons)}]";

			return jsonResponse;
		}
	}
}
