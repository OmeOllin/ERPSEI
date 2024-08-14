using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public class ObjetoImpuestoManager : IObjetoImpuestoManager
    {
        ApplicationDbContext db { get; set; }

        public ObjetoImpuestoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<ObjetoImpuesto>> GetAllAsync()
        {
            return await db.ObjetosImpuesto.ToListAsync();
        }

        public async Task<ObjetoImpuesto?> GetByIdAsync(int id)
        {
            return await db.ObjetosImpuesto.Where(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ObjetoImpuesto?> GetByNameAsync(string name)
        {
            return await db.ObjetosImpuesto.Where(o => o.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
