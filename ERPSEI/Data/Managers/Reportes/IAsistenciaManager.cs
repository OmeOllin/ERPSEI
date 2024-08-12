using ERPSEI.Data.Entities.Reportes;
using NPOI.SS.Formula.Functions;

namespace ERPSEI.Data.Managers.Reportes
{
    public interface IAsistenciaManager : IRWCatalogoManager<Asistencia>
    {
		public Task<List<Asistencia>> GetAllAsync(
			string? nombreEmpleado = null,
			DateTime? fechaIngresoInicio = null,
			DateTime? fechaIngresoFin = null);
	}
}
