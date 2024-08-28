using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
    public class HorariosManager(ApplicationDbContext db) : IHorariosManager
	{
		private async Task<int> GetNextId()
		{
			List<Horario> registros = await db.Horarios.ToListAsync();
			Horario? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Horario horario)
		{
			horario.Id = await GetNextId();
			db.Horarios.Add(horario);
			await db.SaveChangesAsync();
			return horario.Id;
		}
		public async Task UpdateAsync(Horario horario)
		{
			Horario? a = db.Find<Horario>(horario.Id);
			if (a != null)
			{
				a.Descripcion = horario.Descripcion;
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Horario horario)
		{
			db.Horarios.Remove(horario);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Horario? horario = await GetByIdAsync(id);
			if (horario != null)
			{
				db.Remove(horario);
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
					Horario? horario = await GetByIdAsync(int.Parse(id));
					if (horario != null)
					{
						db.Remove(horario);
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

		public async Task<List<Horario>> GetAllAsync()
		{
			return await db.Horarios.Include(h => h.HorarioDetalles).ToListAsync();
		}

		public async Task<Horario?> GetByIdAsync(int id)
		{
			return await db.Horarios.Include(h => h.HorarioDetalles).Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Horario?> GetByNameAsync(string name)
		{
			return await db.Horarios.Include(h => h.HorarioDetalles).Where(a => a.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}
	}
}
