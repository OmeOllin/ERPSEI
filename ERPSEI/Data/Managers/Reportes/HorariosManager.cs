using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
    public class HorariosManager(ApplicationDbContext db) : IHorariosManager
	{
		private async Task<int> GetNextId()
		{
			List<Horarios> registros = await db.Horarios.ToListAsync();
			Horarios? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Horarios horario)
		{
			horario.Id = await GetNextId();
			db.Horarios.Add(horario);
			await db.SaveChangesAsync();
			return horario.Id;
		}
		public async Task UpdateAsync(Horarios horario)
		{
			Horarios? a = db.Find<Horarios>(horario.Id);
			if (a != null)
			{
				a.Salida = horario.Salida;
				a.Entrada = horario.Entrada;
				a.ToleranciaEntrada = horario.ToleranciaEntrada;
				a.ToleranciaSalida = horario.ToleranciaSalida;
				a.ToleranciaFalta = horario.ToleranciaFalta;
				a.NombreHorario = horario.NombreHorario;
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Horarios horario)
		{
			db.Horarios.Remove(horario);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Horarios? horario = await GetByIdAsync(id);
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
					Horarios? horario = await GetByIdAsync(int.Parse(id));
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

		public async Task<List<Horarios>> GetAllAsync()
		{
			return await db.Horarios.ToListAsync();
		}

		public async Task<Horarios?> GetByIdAsync(int id)
		{
			return await db.Horarios.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Horarios?> GetByNameAsync(string name)
		{
			return await db.Horarios.Where(a => a.NombreHorario.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}
	}
}
