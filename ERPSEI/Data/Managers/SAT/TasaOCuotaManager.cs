using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class TasaOCuotaManager : ITasaOCuotaManager
	{
        ApplicationDbContext db { get; set; }

        public TasaOCuotaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<TasaOCuota>> GetAllAsync()
		{
			return await db.TasasOCuotas.ToListAsync();
		}

		public async Task<TasaOCuota?> GetByIdAsync(int id)
        {
            return await db.TasasOCuotas.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

		public async Task<TasaOCuota?> GetByNameAsync(string name)
		{
			return await db.TasasOCuotas.Where(t => t.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
