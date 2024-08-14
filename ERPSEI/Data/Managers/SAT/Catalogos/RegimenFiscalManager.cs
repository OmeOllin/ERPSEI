using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public class RegimenFiscalManager : IRegimenFiscalManager
    {
        ApplicationDbContext db { get; set; }

        public RegimenFiscalManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<RegimenFiscal>> GetAllAsync()
        {
            return await db.RegimenesFiscales.ToListAsync();
        }

        public async Task<RegimenFiscal?> GetByIdAsync(int id)
        {
            return await db.RegimenesFiscales.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<RegimenFiscal?> GetByNameAsync(string name)
        {
            return await db.RegimenesFiscales.Where(r => r.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
