using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class TipoComprobanteManager : ITipoComprobanteManager
	{
        ApplicationDbContext db { get; set; }

        public TipoComprobanteManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<TipoComprobante>> GetAllAsync()
		{
			return await db.TiposComprobante.ToListAsync();
		}

		public async Task<TipoComprobante?> GetByIdAsync(int id)
        {
            return await db.TiposComprobante.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

		public async Task<TipoComprobante?> GetByNameAsync(string name)
		{
			return await db.TiposComprobante.Where(t => t.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
