using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IArchivoEmpleadoManager
    {

        public Task CreateAsync(ArchivoEmpleado file);

        public Task UpdateAsync(ArchivoEmpleado file);

        public Task DeleteAsync(ArchivoEmpleado file);

        public Task DeleteByIdAsync(string fileId);

        public Task<List<ArchivoEmpleado>> GetFilesByEmpleadoIdAsync(int empleadoId);

        ArchivoEmpleado? GetFileById(string id);
    }
}