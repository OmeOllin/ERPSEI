using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class MetodoPagoManager : IMetodoPagoManager
	{
        ApplicationDbContext db { get; set; }

        public MetodoPagoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<MetodoPago>> GetAllAsync()
		{
			return await db.MetodosPago.ToListAsync();
		}

		public async Task<MetodoPago?> GetByIdAsync(int id)
        {
            return await db.MetodosPago.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

		public async Task<MetodoPago?> GetByNameAsync(string name)
		{
			return await db.MetodosPago.Where(m => m.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
