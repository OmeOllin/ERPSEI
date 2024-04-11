using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empleados
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

        public async Task<EstadoCivil?> GetByIdAsync(int id)
        {
            return await db.EstadosCiviles.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<EstadoCivil?> GetByNameAsync(string name)
        {
            return await db.EstadosCiviles.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
