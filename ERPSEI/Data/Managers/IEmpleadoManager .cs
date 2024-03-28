using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IEmpleadoManager
    {

        public Task<int> CreateAsync(Empleado contacto);

        public Task UpdateAsync(Empleado contacto);

        public Task DeleteAsync(Empleado contacto);

		public Task DisableByIdAsync(int id);

		public Task DeleteByIdAsync(int contactoId);

        public Task DeleteMultipleByIdAsync(string[] ids);

        public Task<List<Empleado>> GetAllAsync(
			DateTime? fechaIngresoInicio = null,
			DateTime? fechaIngresoFin = null,
			DateTime? fechaNacimientoInicio = null,
			DateTime? fechaNacimientoFin = null,
			int? puestoId = null,
			int? areaId = null,
			int? subareaId = null,
			int? oficinaId = null
		);

		public Task<Empleado?> GetEmpleadoLoginAsync(int id);

        public Task<Empleado?> GetEmpleadoOrganigramaAsync(int id);

		public Task<List<Empleado>> GetEmpleadosOrganigramaAsync(int? jefeId, int? puestoId, int? areaId, int? subareaId);

		public Task<Empleado?> GetByIdWithAdicionalesAsync(int id);

		public Task<Empleado?> GetByIdAsync(int id);

        public Task<Empleado?> GetByCURPAsync(string curp);

        public Task<Empleado?> GetByNameAsync(string name);

		public Task<Empleado?> GetByEmailAsync(string email);

	}
}