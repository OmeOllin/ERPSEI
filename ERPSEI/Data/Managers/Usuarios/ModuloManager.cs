using ERPSEI.Data.Entities.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Usuarios
{
    public class ModuloManager : IModuloManager
	{
        ApplicationDbContext db { get; set; }

        public ModuloManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		public async Task<List<Modulo>> GetAllAsync()
		{
			return await db.Modulos.Include(m => m.Accesos).ThenInclude(a => a.Rol).ToListAsync();
		}

		public async Task<Modulo?> GetByIdAsync(int id)
        {
            return await db.Modulos.Include(m => m.Accesos).ThenInclude(a => a.Rol).Where(e => e.Id == id).FirstOrDefaultAsync();
        }

		public async Task<Modulo?> GetByNameAsync(string name)
		{
			return await db.Modulos.Include(m => m.Accesos).ThenInclude(a => a.Rol).Where(c => c.NombreNormalizado.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}
