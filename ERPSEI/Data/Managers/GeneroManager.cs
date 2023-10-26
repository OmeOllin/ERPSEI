using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class GeneroManager : IRCatalogoManager<Genero>
    {
        ApplicationDbContext db { get; set; }

        public GeneroManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Genero>> GetAllAsync()
		{
			return await db.Generos.ToListAsync();
		}

		public Genero? GetById(int id)
        {
            return db.Generos.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
