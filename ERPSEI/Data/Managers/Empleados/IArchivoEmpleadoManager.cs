using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers.Empleados
{
    public interface IArchivoEmpleadoManager
    {

        public Task<string> CreateAsync(ArchivoEmpleado file);

        public Task UpdateAsync(ArchivoEmpleado file);

        public Task DeleteAsync(ArchivoEmpleado file);

        public Task DeleteByIdAsync(string fileId);

        public Task DeleteByEmpleadoIdAsync(int empleadoId);

        public Task<ProfilePicture?> GetProfilePicByEmpleadoId(int empleadoId);

        public Task<List<SemiArchivoEmpleado>> GetFilesByEmpleadoIdAsync(int empleadoId);

        ArchivoEmpleado? GetFileById(string id);
    }
}