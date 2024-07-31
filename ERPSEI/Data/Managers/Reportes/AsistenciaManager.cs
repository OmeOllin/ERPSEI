using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
	public class AsistenciaManager : IAsistenciaManager
	{
		ApplicationDbContext db { get; set; }

		public AsistenciaManager(ApplicationDbContext _db)
		{
			db = _db;
		}

		public async Task<List<Asistencia>> GetAllAsync()
		{
			return await db.Asistencias.ToListAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => a.NombreEmpleado.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(string id)
		{
			return await GetByIdAsync(id.ToString());
		}
	}
}
