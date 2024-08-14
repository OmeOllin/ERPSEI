using ERPSEI.Data.Entities.SAT.Catalogos;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public interface IUnidadMedidaManager : IRCatalogoManager<UnidadMedida>
    {

        public Task<List<UnidadMedida>> SearchUnidades(string texto);

    }
}