﻿using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Managers.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Reportes
{
	public class AsistenciasManager : IAsistenciaManager
	{
		ApplicationDbContext db { get; set; }

		public AsistenciasManager(ApplicationDbContext _db)
		{
			db = _db;
		}

		public async Task<List<Asistencia>> GetAllAsync()
		{
			return await db.Asistencias.ToListAsync();
		}

		public async Task<Asistencia?> GetByIdAsync(int id)
		{
			return await db.Asistencias.Where(a => a.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Asistencia?> GetByNameAsync(string name)
		{
			return await db.Asistencias.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
		}

	}
}