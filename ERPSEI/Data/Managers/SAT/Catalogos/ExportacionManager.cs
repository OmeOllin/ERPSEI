﻿using ERPSEI.Data.Entities.SAT.Catalogos;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.SAT.Catalogos
{
    public class ExportacionManager : IExportacionManager
    {
        ApplicationDbContext db { get; set; }

        public ExportacionManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        public async Task<List<Exportacion>> GetAllAsync()
        {
            return await db.Exportaciones.ToListAsync();
        }

        public async Task<Exportacion?> GetByIdAsync(int id)
        {
            return await db.Exportaciones.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Exportacion?> GetByNameAsync(string name)
        {
            return await db.Exportaciones.Where(e => e.Descripcion.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}