using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class MonedaManager : IMonedaManager
	{
        ApplicationDbContext db { get; set; }

        public MonedaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Moneda>> GetAllAsync()
		{
			return await db.Monedas.ToListAsync();
		}

		public async Task<Moneda?> GetByIdAsync(int id)
        {
            return await db.Monedas.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Moneda?> GetByNameAsync(string name)
		{
			return await db.Monedas.Where(m => m.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
