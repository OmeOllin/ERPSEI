using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers.Empleados
{
	public interface IAsistenciaManager: IRCatalogoManager<Asistencia>
	{
		public Task<Asistencia?> GetByIdAsync(string id);
	}
}
