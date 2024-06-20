using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers.Empleados
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
            int? oficinaId = null,
            bool deshabilitado = false
        );

		/*public Task<List<Asistencia>> GetAllAsistenciaAsync(
			DateTime? fechaInicio = null,
			DateTime? fechaFin = null,
            string? nombreEmpleado = null,

			int? asistenciaId = null,
			string? nombre = null,
			DateOnly? fecha = null,
			TimeOnly? horaEntrada = null,
			TimeOnly? horaSalida = null,
			int? retardo = null,
			int? total = null,
			int? falta = null
			//bool deshabilitado = false
		);*/

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