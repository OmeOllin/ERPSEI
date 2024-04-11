using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class PeriodicidadManager : IPeriodicidadManager
	{
        ApplicationDbContext db { get; set; }

        public PeriodicidadManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Periodicidad>> GetAllAsync()
		{
			return await db.Periodicidades.ToListAsync();
		}

		public async Task<Periodicidad?> GetByIdAsync(int id)
        {
            return await db.Periodicidades.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Periodicidad?> GetByNameAsync(string name)
		{
			return await db.Periodicidades.Where(p => p.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
