using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class TipoFactorManager : ITipoFactorManager
	{
        ApplicationDbContext db { get; set; }

        public TipoFactorManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<TipoFactor>> GetAllAsync()
		{
			return await db.TiposFactor.ToListAsync();
		}

		public async Task<TipoFactor?> GetByIdAsync(int id)
        {
            return await db.TiposFactor.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

		public async Task<TipoFactor?> GetByNameAsync(string name)
		{
			return await db.TiposFactor.Where(t => t.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
