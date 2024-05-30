using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers.Empleados
{
	public interface IAsistenciaManager: IRWCatalogoManager<Asistencia>
	{
		public Task<Asistencia?> GetByIdAsync(int id);
	}
}
