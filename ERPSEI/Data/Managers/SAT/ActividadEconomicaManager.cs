using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT
{
    public class ActividadEconomicaManager : IRWCatalogoManager<ActividadEconomica>
    {
        ApplicationDbContext db { get; set; }

        public ActividadEconomicaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        private async Task<int> getNextId()
        {
            List<ActividadEconomica> registros = await db.ActividadesEconomicas.ToListAsync();
            ActividadEconomica? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ActividadEconomica a)
        {
            a.Id = await getNextId();
            db.ActividadesEconomicas.Add(a);
            await db.SaveChangesAsync();
            return a.Id;
        }
        public async Task UpdateAsync(ActividadEconomica a)
        {
            ActividadEconomica? n = db.Find<ActividadEconomica>(a.Id);
            if (n != null)
            {
                n.Nombre = a.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ActividadEconomica a)
        {
            db.ActividadesEconomicas.Remove(a);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ActividadEconomica? a = await GetByIdAsync(id);
            if (a != null)
            {
                db.Remove(a);
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
                    ActividadEconomica? a = await GetByIdAsync(int.Parse(id));
                    if (a != null)
                    {
                        db.Remove(a);
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

        public async Task<List<ActividadEconomica>> GetAllAsync()
        {
            return await db.ActividadesEconomicas.ToListAsync();
        }

        public async Task<ActividadEconomica?> GetByIdAsync(int id)
        {
            return await db.ActividadesEconomicas.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ActividadEconomica?> GetByNameAsync(string name)
        {
            return await db.ActividadesEconomicas.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}
