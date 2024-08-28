using ERPSEI.Data.Entities.Reportes;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
	public class AsistenciaManager(ApplicationDbContext db) : IAsistenciaManager
	{
		private async Task<int> GetNextId()
		{
			List<Asistencia> registros = await db.Asistencias.ToListAsync();
			Asistencia? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
			int lastId = last != null ? last.Id : 0;
			lastId += 1;

			return lastId;
		}

		public async Task<int> CreateAsync(Asistencia asistencia)
		{
			asistencia.Id = await GetNextId();
			db.Asistencias.Add(asistencia);
			await db.SaveChangesAsync();
			return asistencia.Id;
		}
		public async Task UpdateAsync(Asistencia asistencia)
		{
			Asistencia? a = db.Find<Asistencia>(asistencia.Id);
			if (a != null)
			{
				a.EmpleadoId = asistencia.EmpleadoId;
				a.Salida = asistencia.Salida;
				a.Entrada = asistencia.Entrada;
				a.ResultadoE = asistencia.ResultadoE;
				a.ResultadoS = asistencia.ResultadoS;
				a.Dia = asistencia.Dia;
				a.Fecha = asistencia.Fecha;
				await db.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Asistencia asistencia)
		{
			db.Asistencias.Remove(asistencia);
			await db.SaveChangesAsync();
		}

		public async Task DeleteByIdAsync(int id)
		{
			Asistencia? asistencia = await GetByIdAsync(id);
			if (asistencia != null)
			{
				db.Remove(asistencia);
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
					Asistencia? asistencia = await GetByIdAsync(int.Parse(id));
					if (asistencia != null)
					{
						db.Remove(asistencia);
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

		public async Task<List<Asistencia>> GetAllAsync()
		{
			return await db.Asistencias.Include(e => e.Empleado).ToListAsync();
		}
		public async Task<List<Asistencia>> GetAllAsync(string? nombreEmpleado = null, DateTime? fechaIngresoInicio = null, DateTime? fechaIngresoFin = null)
		{
			DateOnly? fechaInicio = fechaIngresoInicio.HasValue ? DateOnly.FromDateTime(fechaIngresoInicio.Value) : (DateOnly?)null;
			DateOnly? fechaFin = fechaIngresoFin.HasValue ? DateOnly.FromDateTime(fechaIngresoFin.Value) : (DateOnly?)null;

			return await db.Asistencias
				.Include(e => e.Empleado)
				.Where(e => nombreEmpleado == null || e.Empleado.NombreCompleto == nombreEmpleado)
				.Where(e => !fechaInicio.HasValue || (fechaFin.HasValue ? e.Fecha >= fechaInicio.Value && e.Fecha <= fechaFin.Value : e.Fecha == fechaInicio.Value))
				.ToListAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => false).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(string id)
		{
			return await GetByIdAsync(id.ToString());
		}
	}
}
