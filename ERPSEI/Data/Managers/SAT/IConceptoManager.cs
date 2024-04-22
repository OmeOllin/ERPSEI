using ERPSEI.Data.Entities.SAT;

namespace ERPSEI.Data.Managers.SAT
{
    public interface IConceptoManager : IRWCatalogoManager<Concepto>
    {

        public Task DeleteByPrefacturaIdAsync(int id);

	}
}