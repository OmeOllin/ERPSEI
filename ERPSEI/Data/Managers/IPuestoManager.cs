using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IPuestoManager
    {
        public Task CreateAsync(Puesto puesto);

        public Task UpdateAsync(Puesto puesto);

        public Task DeleteAsync(Puesto puesto);

        public Task DeleteByIdAsync(int id);
        public Task DeleteMultipleByIdAsync(string[] ids);
        public Task<List<Puesto>> GetAllAsync();

        Puesto? GetById(int id);
    }
}
