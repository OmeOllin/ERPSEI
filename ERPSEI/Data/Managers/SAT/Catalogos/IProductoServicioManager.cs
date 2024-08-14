using ERPSEI.Data.Entities.Empresas;
using ERPSEI.Data.Entities.SAT.Catalogos;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public interface IProductoServicioManager : IRWCatalogoManager<ProductoServicio>
    {
        public Task<List<ProductoServicioBuscado>> SearchProductService(string texto);

    }
}