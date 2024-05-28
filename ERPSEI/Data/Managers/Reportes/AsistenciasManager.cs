using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
	public class AsistenciasManager : IRCatalogoManager<Asistencias>
	{
		ApplicationDbContext db { get; set; }

		public AsistenciasManager(ApplicationDbContext _db)
		{
			db = _db;
		}

		public async Task<List<Asistencias>> GetAllAsync()
		{
			return await db.Asistencias.ToListAsync();
		}

		public async Task<Asistencias?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Asistencias?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
