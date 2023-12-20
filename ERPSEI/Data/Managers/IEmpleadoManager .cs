using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IEmpleadoManager
    {

        public Task<int> CreateAsync(Empleado contacto);

        public Task UpdateAsync(Empleado contacto);

        public Task DeleteAsync(Empleado contacto);

        public Task DeleteByIdAsync(int contactoId);

        public Task DeleteMultipleByIdAsync(string[] ids);

        public Task<List<Empleado>> GetAllAsync();

        public Task<Empleado?> GetEmpleadoOrganigramaAsync(int id);

		public Task<List<Empleado>> GetEmpleadosOrganigramaAsync(int? jefeId, int? areaId, int? subareaId);

        public Task<Empleado?> GetByIdAsync(int id);

        public Task<Empleado?> GetByCURPAsync(string curp);

        public Task<Empleado?> GetByNameAsync(string name);


    }
}