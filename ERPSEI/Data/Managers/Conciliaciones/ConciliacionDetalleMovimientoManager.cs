using ERPSEI.Data.Entities.Conciliaciones;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class ConciliacionDetalleMovimientoManager(ApplicationDbContext db) : IConciliacionDetalleMovimientoManager
    {
        private async Task<int> GetNextId()
        {
            List<ConciliacionDetalleMovimiento> conciliacionDetalleMovimientos = await db.ConciliacionesDetallesMovimientos.ToListAsync();
            ConciliacionDetalleMovimiento? last = conciliacionDetalleMovimientos.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ConciliacionDetalleMovimiento conciliacionDetalleMovimiento)
        {
            conciliacionDetalleMovimiento.Id = await GetNextId();
            db.ConciliacionesDetallesMovimientos.Add(conciliacionDetalleMovimiento);
            await db.SaveChangesAsync();
            return conciliacionDetalleMovimiento.Id;
        }
        public async Task UpdateAsync(ConciliacionDetalleMovimiento conciliacionDetalleMovimiento)
        {
            ConciliacionDetalleMovimiento? a = db.Find<ConciliacionDetalleMovimiento>(conciliacionDetalleMovimiento.Id);
            if (a != null)
            {
                a.ConciliacionDetalleId = conciliacionDetalleMovimiento.ConciliacionDetalleId;
                a.MovimientoBancarioId = conciliacionDetalleMovimiento.MovimientoBancarioId;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ConciliacionDetalleMovimiento conciliacionDetalleMovimiento)
        {
            db.ConciliacionesDetallesMovimientos.Remove(conciliacionDetalleMovimiento);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ConciliacionDetalleMovimiento? conciliacionDetalleMovimiento = await GetByIdAsync(id);
            if (conciliacionDetalleMovimiento != null)
            {
                db.Remove(conciliacionDetalleMovimiento);
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
                    ConciliacionDetalleMovimiento? conciliacionDetalleMovimiento = await GetByIdAsync(int.Parse(id));
                    if (conciliacionDetalleMovimiento != null)
                    {
                        db.Remove(conciliacionDetalleMovimiento);
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

        public async Task<List<ConciliacionDetalleMovimiento>> GetAllAsync()
        {
            return await db.ConciliacionesDetallesMovimientos.ToListAsync();
        }

        public async Task<ConciliacionDetalleMovimiento?> GetByIdAsync(int id)
        {
            return await db.ConciliacionesDetallesMovimientos.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        //Verificar por el nombre que no existe en la entidad
        public Task<ConciliacionDetalleMovimiento?> GetByNameAsync(string name)
        {
            return Task.FromResult<ConciliacionDetalleMovimiento?>(null);
        }
    }
}
