using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
    public interface IBancoEmpresaManager
    {
        public Task<int> CreateAsync(BancoEmpresa banco);

        public Task UpdateAsync(BancoEmpresa banco);

        public Task DeleteAsync(BancoEmpresa banco);

        public Task DeleteByIdAsync(int id);

        public Task DeleteByEmpresaIdAsync(int id);

        public Task<List<BancoEmpresa>> GetBancosByEmpresaIdAsync(int id);

		BancoEmpresa? GetBancoById(int id);
    }
}