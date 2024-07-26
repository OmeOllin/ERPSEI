using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class TipoRelacionManager : ITipoRelacionManager
	{
        ApplicationDbContext db { get; set; }

        public TipoRelacionManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<TipoRelacion>> GetAllAsync()
		{
			return await db.TiposRelacion.ToListAsync();
		}

		public async Task<TipoRelacion?> GetByIdAsync(int id)
        {
            return await db.TiposRelacion.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

		public async Task<TipoRelacion?> GetByNameAsync(string name)
		{
			return await db.TiposRelacion.Where(t => t.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
