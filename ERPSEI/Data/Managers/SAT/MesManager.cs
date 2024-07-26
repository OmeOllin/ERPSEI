using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class MesManager : IMesManager
	{
        ApplicationDbContext db { get; set; }

        public MesManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Mes>> GetAllAsync()
		{
			return await db.Meses.ToListAsync();
		}

		public async Task<Mes?> GetByIdAsync(int id)
        {
            return await db.Meses.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Mes?> GetByNameAsync(string name)
		{
			return await db.Meses.Where(m => m.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
