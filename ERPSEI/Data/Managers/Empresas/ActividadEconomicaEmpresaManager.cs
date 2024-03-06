using ERPSEI.Data.Entities.Empresas;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empresas
{
	public class ActividadEconomicaEmpresaManager : IActividadEconomicaEmpresaManager
    {
        ApplicationDbContext db { get; set; }

        public ActividadEconomicaEmpresaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<ActividadEconomicaEmpresa> registros = await db.ActividadesEconomicasEmpresa.ToListAsync();
			ActividadEconomicaEmpresa? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(ActividadEconomicaEmpresa a)
        {
            a.Id = await getNextId();
            db.ActividadesEconomicasEmpresa.Add(a);
            await db.SaveChangesAsync();
            return a.Id;
        }
        public async Task UpdateAsync(ActividadEconomicaEmpresa a)
        {
			ActividadEconomicaEmpresa? n = db.Find<ActividadEconomicaEmpresa>(a.Id);
            if (n != null)
            {
                n.EmpresaId = a.EmpresaId;
				n.ActividadId = a.ActividadId;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ActividadEconomicaEmpresa a)
        {
            db.ActividadesEconomicasEmpresa.Remove(a);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
			ActividadEconomicaEmpresa? a = await GetByIdAsync(id);
            if (a != null)
            {
                db.Remove(a);
                await db.SaveChangesAsync();
            }
        }

		public async Task DeleteByEmpresaIdAsync(int id)
		{
			List<ActividadEconomicaEmpresa> actividades = await db.ActividadesEconomicasEmpresa.Where(a => a.EmpresaId == id).ToListAsync();
			if (actividades != null && actividades.Count >= 1) { db.ActividadesEconomicasEmpresa.RemoveRange(actividades); }
		}

		public async Task DeleteMultipleByIdAsync(string[] ids)
		{
			//Inicia una transacción.
			await db.Database.BeginTransactionAsync();
			try
			{
				foreach (string id in ids)
				{
					ActividadEconomicaEmpresa? a = await GetByIdAsync(int.Parse(id));
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

		public async Task<List<ActividadEconomicaEmpresa>> GetAllAsync()
		{
			return await db.ActividadesEconomicasEmpresa.ToListAsync();
		}

		public async Task<ActividadEconomicaEmpresa?> GetByIdAsync(int id)
        {
            return await db.ActividadesEconomicasEmpresa.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

		public async Task<ActividadEconomicaEmpresa?> GetByNameAsync(string name)
		{
			return await db.ActividadesEconomicasEmpresa
				.Include(a => a.ActividadEconomica)
				.Where(a => a.ActividadEconomica != null && a.ActividadEconomica.Nombre == name).FirstOrDefaultAsync();
		}

	}
}
