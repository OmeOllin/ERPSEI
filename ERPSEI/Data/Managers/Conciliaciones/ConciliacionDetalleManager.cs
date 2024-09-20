using ERPSEI.Data.Entities.Conciliaciones;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class ConciliacionDetalleManager(ApplicationDbContext db) : IConciliacionDetalleManager
    {
        private async Task<int> GetNextId()
        {
            List<ConciliacionDetalle> ConciliacionDetalle = await db.ConciliacionesDetalles.ToListAsync();
            ConciliacionDetalle? last = ConciliacionDetalle.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ConciliacionDetalle conciliacionDetalle)
        {
            conciliacionDetalle.Id = await GetNextId();
            db.ConciliacionesDetalles.Add(conciliacionDetalle);
            await db.SaveChangesAsync();
            return conciliacionDetalle.Id;
        }
        public async Task UpdateAsync(ConciliacionDetalle conciliacionDetalle)
        {
            ConciliacionDetalle? a = db.Find<ConciliacionDetalle>(conciliacionDetalle.Id);
            if (a != null)
            {
                a.ConciliacionId = conciliacionDetalle.ConciliacionId;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ConciliacionDetalle conciliacionDetalle)
        {
            db.ConciliacionesDetalles.Remove(conciliacionDetalle);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ConciliacionDetalle? conciliacionDetalle = await GetByIdAsync(id);
            if (conciliacionDetalle != null)
            {
                db.Remove(conciliacionDetalle);
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
                    ConciliacionDetalle? conciliacionDetalle = await GetByIdAsync(int.Parse(id));
                    if (conciliacionDetalle != null)
                    {
                        db.Remove(conciliacionDetalle);
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

        public async Task<List<ConciliacionDetalle>> GetAllAsync()
        {
            return await db.ConciliacionesDetalles.ToListAsync();
        }

        public async Task<ConciliacionDetalle?> GetByIdAsync(int id)
        {
            return await db.ConciliacionesDetalles.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public Task<ConciliacionDetalle?> GetByNameAsync(string name)
        {
            return Task.FromResult<ConciliacionDetalle?>(null);
        }

    }
}
