using ERPSEI.Data.Entities.Empresas;

namespace ERPSEI.Data.Managers.Empresas
{
    public interface IArchivoEmpresaManager
    {
        public Task<string> CreateAsync(ArchivoEmpresa file);

        public Task UpdateAsync(ArchivoEmpresa file);

        public Task DeleteAsync(ArchivoEmpresa file);

        public Task DeleteByIdAsync(string fileId);

        public Task DeleteByEmpresaIdAsync(int eId);

        public Task<List<SemiArchivoEmpresa>> GetFilesByEmpresaIdAsync(int eId);

		ArchivoEmpresa? GetFileById(string id);
    }
}