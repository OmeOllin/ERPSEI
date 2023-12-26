using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
    public interface IEmpresaManager
    {

        public Task<int> CreateAsync(Empresa e);

        public Task UpdateAsync(Empresa e);

        public Task DeleteAsync(Empresa e);

        public Task DeleteByIdAsync(int eId);

        public Task DeleteMultipleByIdAsync(string[] ids);

        public Task<List<Empresa>> GetAllAsync();

        public Task<Empresa?> GetByIdAsync(int id);

        public Task<Empresa?> GetByRFCAsync(string rfc);

        public Task<Empresa?> GetByNameAsync(string name);


    }
}