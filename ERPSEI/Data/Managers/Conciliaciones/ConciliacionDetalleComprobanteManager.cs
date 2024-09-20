using ERPSEI.Data.Entities.Conciliaciones;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class ConciliacionDetalleComprobanteManager(ApplicationDbContext db) : IConciliacionDetalleComprobanteManager
    {
        private async Task<int> GetNextId()
        {
            List<ConciliacionDetalleComprobante> conciliacionDetalleComprobante = await db.ConciliacionesDetallesComprobantes.ToListAsync();
            ConciliacionDetalleComprobante? last = conciliacionDetalleComprobante.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ConciliacionDetalleComprobante conciliacionDetalleComprobante)
        {
            conciliacionDetalleComprobante.Id = await GetNextId();
            db.ConciliacionesDetallesComprobantes.Add(conciliacionDetalleComprobante);
            await db.SaveChangesAsync();
            return conciliacionDetalleComprobante.Id;
        }
        public async Task UpdateAsync(ConciliacionDetalleComprobante conciliacionDetalleComprobante)
        {
            ConciliacionDetalleComprobante? a = db.Find<ConciliacionDetalleComprobante>(conciliacionDetalleComprobante.Id);
            if (a != null)
            {
                a.ConciliacionDetalleId = conciliacionDetalleComprobante.ConciliacionDetalleId;
                a.ComprobanteId = conciliacionDetalleComprobante.ComprobanteId;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ConciliacionDetalleComprobante conciliacionDetalleComprobante)
        {
            db.ConciliacionesDetallesComprobantes.Remove(conciliacionDetalleComprobante);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ConciliacionDetalleComprobante? conciliacionDetalleComprobante = await GetByIdAsync(id);
            if (conciliacionDetalleComprobante != null)
            {
                db.Remove(conciliacionDetalleComprobante);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteMultipleByIdAsync(string[] ids)
        {
            //Inicia una transacción.
            await db.Database.BeginTransactionAsync();
            try
            {
                foreach (string id in ids)
                {
                    ConciliacionDetalleComprobante? conciliacionDetalleComprobante = await GetByIdAsync(int.Parse(id));
                    if (conciliacionDetalleComprobante != null)
                    {
                        db.Remove(conciliacionDetalleComprobante);
                        await db.SaveChangesAsync();
                    }
                }

                await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                throw;

            }
        }

        public async Task<List<ConciliacionDetalleComprobante>> GetAllAsync()
        {
            return await db.ConciliacionesDetallesComprobantes.ToListAsync();
        }

        public async Task<ConciliacionDetalleComprobante?> GetByIdAsync(int id)
        {
            return await db.ConciliacionesDetallesComprobantes.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public Task<ConciliacionDetalleComprobante?> GetByNameAsync(string name)
        {
            return Task.FromResult<ConciliacionDetalleComprobante?>(null);
        }
    }
}
