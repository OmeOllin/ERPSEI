using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class EstadoCivilManager : IRCatalogoManager<EstadoCivil>
    {
        ApplicationDbContext db { get; set; }

        public EstadoCivilManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<EstadoCivil>> GetAllAsync()
		{
			return await db.EstadosCiviles.ToListAsync();
		}

		public EstadoCivil? GetById(int id)
        {
            return db.EstadosCiviles.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
