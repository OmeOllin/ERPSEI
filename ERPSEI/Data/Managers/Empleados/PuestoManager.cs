﻿using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empleados
{
    public class PuestoManager : IRWCatalogoManager<Puesto>
    {
        ApplicationDbContext db { get; set; }

        public PuestoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        private async Task<int> getNextId()
        {
            List<Puesto> registros = await db.Puestos.ToListAsync();
            Puesto? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }
        public async Task<int> CreateAsync(Puesto puesto)
        {
            puesto.Id = await getNextId();
            db.Puestos.Add(puesto);
            await db.SaveChangesAsync();
            return puesto.Id;
        }
        public async Task UpdateAsync(Puesto puesto)
        {
            Puesto? p = db.Find<Puesto>(puesto.Id);
            if (p != null)
            {
                p.Nombre = puesto.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Puesto puesto)
        {
            db.Puestos.Remove(puesto);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Puesto? puesto = await GetByIdAsync(id);
            if (puesto != null)
            {
                db.Remove(puesto);
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
                    Puesto? puesto = await GetByIdAsync(int.Parse(id));
                    if (puesto != null)
                    {
                        db.Remove(puesto);
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

        public async Task<List<Puesto>> GetAllAsync()
        {
            return await db.Puestos.ToListAsync();
        }

        public async Task<Puesto?> GetByIdAsync(int id)
        {
            return await db.Puestos.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Puesto?> GetByNameAsync(string name)
        {
            return await db.Puestos.Where(a => a.Nombre.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

    }
}