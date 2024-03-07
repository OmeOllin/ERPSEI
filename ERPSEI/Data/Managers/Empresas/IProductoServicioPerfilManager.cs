using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
	public interface IProductoServicioPerfilManager : IRWCatalogoManager<ProductoServicioPerfil>
	{

		public Task DeleteByPerfilIdAsync(int id);

	}
}