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

		public async Task<Genero?> GetByIdAsync(int id)
        {
            return await db.Generos.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Genero?> GetByNameAsync(string name)
		{
			return await db.Generos.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}
	}
}
