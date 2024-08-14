using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public class UsoCFDIManager : IUsoCFDIManager
    {
        ApplicationDbContext db { get; set; }

        public UsoCFDIManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<UsoCFDI>> GetAllAsync()
        {
            return await db.UsosCFDI.ToListAsync();
        }

        public async Task<UsoCFDI?> GetByIdAsync(int id)
        {
            return await db.UsosCFDI.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UsoCFDI?> GetByNameAsync(string name)
        {
            return await db.UsosCFDI.Where(u => u.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
