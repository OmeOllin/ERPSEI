using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
	public interface IActividadEconomicaEmpresaManager : IRWCatalogoManager<ActividadEconomicaEmpresa>
	{

		public Task DeleteByEmpresaIdAsync(int id);

	}
}