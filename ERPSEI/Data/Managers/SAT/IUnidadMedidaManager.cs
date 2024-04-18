using ERPSEI.Data.Entities.SAT;

namespace ERPSEI.Data.Managers.SAT
{
    public interface IUnidadMedidaManager : IRCatalogoManager<UnidadMedida>
    {

		public Task<List<UnidadMedida>> SearchUnidades(string texto);

	}
}