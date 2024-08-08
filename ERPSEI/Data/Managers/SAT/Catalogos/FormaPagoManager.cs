using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public class FormaPagoManager : IFormaPagoManager
    {
        ApplicationDbContext db { get; set; }

        public FormaPagoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<FormaPago>> GetAllAsync()
        {
            return await db.FormasPago.ToListAsync();
        }

        public async Task<FormaPago?> GetByIdAsync(int id)
        {
            return await db.FormasPago.Where(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<FormaPago?> GetByNameAsync(string name)
        {
            return await db.FormasPago.Where(f => f.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
