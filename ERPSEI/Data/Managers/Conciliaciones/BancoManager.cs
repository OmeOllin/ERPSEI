using ERPSEI.Data.Entities.Conciliaciones;
using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class BancoManager(ApplicationDbContext db) : IBancoManager
    {
        public async Task<List<Banco>> GetAllAsync()
        {
            return await db.Bancos.ToListAsync();
        }

        public async Task<Banco?> GetByIdAsync(int id)
        {
            return await db.Bancos.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Banco?> GetByNameAsync(string name)
        {
            return await db.Bancos.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
    }
}
