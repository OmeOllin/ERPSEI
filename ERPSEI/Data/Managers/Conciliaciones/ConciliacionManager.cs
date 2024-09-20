﻿using ERPSEI.Data.Entities.Conciliaciones;
using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.Reportes;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ERPSEI.Data.Managers.Conciliaciones
{
    public class ConciliacionManager(ApplicationDbContext db) : IConciliacionManager
    {
        private async Task<int> GetNextId()
        {
            List<Conciliacion> registros = await db.Conciliaciones.ToListAsync();
            Conciliacion? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(Conciliacion conciliacion)
        {
            conciliacion.Id = await GetNextId();
            db.Conciliaciones.Add(conciliacion);
            await db.SaveChangesAsync();
            return conciliacion.Id;
        }
        public async Task UpdateAsync(Conciliacion conciliacion)
        {
            Conciliacion? a = db.Find<Conciliacion>(conciliacion.Id);
            if (a != null)
            {
                a.Fecha = conciliacion.Fecha;
                a.Descripcion = conciliacion.Descripcion;
                a.Total = conciliacion.Total;
                a.BancoId = conciliacion.BancoId;
                a.ClienteId = conciliacion.ClienteId;
                a.EmpresaId = conciliacion.EmpresaId;
                a.UsuarioCreadorId = conciliacion.UsuarioCreadorId;
                a.UsuarioModificadorId = conciliacion.UsuarioModificadorId;
                a.Deshabilitado = conciliacion.Deshabilitado;

                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Conciliacion conciliacion)
        {
            db.Conciliaciones.Remove(conciliacion);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Conciliacion? conciliacion = await GetByIdAsync(id);
            if (conciliacion != null)
            {
                db.Remove(conciliacion);
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
                    Conciliacion? conciliacion = await GetByIdAsync(int.Parse(id));
                    if (conciliacion != null)
                    {
                        db.Remove(conciliacion);
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

        public async Task<List<Conciliacion>> GetAllAsync()
        {
            return await db.Conciliaciones.ToListAsync();
        }

        public async Task<Conciliacion?> GetByIdAsync(int id)
        {
            return await db.Conciliaciones.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Conciliacion?> GetByNameAsync(string desc)
        {
            return await db.Conciliaciones.Where(a => a.Descripcion.ToLower() == desc.ToLower()).FirstOrDefaultAsync();
        }
    }
}
