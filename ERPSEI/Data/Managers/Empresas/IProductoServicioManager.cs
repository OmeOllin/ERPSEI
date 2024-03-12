using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
    public interface IProductoServicioManager : IRWCatalogoManager<ProductoServicio>
    {
        public Task<List<ProductoServicioBuscado>> SearchProductService(string texto);

    }
}