using ERPSEI.Data.Entities.SAT;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class ImpuestoManager : IImpuestoManager
	{
        ApplicationDbContext db { get; set; }

        public ImpuestoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Impuesto>> GetAllAsync()
		{
			return await db.Impuestos.ToListAsync();
		}

		public async Task<Impuesto?> GetByIdAsync(int id)
        {
            return await db.Impuestos.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Impuesto?> GetByNameAsync(string name)
		{
			return await db.Impuestos.Where(i => i.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
