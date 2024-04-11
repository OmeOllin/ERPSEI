using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empleados
{
    public class ContactoEmergenciaManager : IContactoEmergenciaManager
    {
        ApplicationDbContext db { get; set; }

        public ContactoEmergenciaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        private async Task<int> getNextId()
        {
            List<ContactoEmergencia> registros = await db.ContactosEmergencia.ToListAsync();
            ContactoEmergencia? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(ContactoEmergencia contacto)
        {
            contacto.Id = await getNextId();
            db.ContactosEmergencia.Add(contacto);
            await db.SaveChangesAsync();
            return contacto.Id;
        }

        public async Task UpdateAsync(ContactoEmergencia contacto)
        {
            ContactoEmergencia? a = db.Find<ContactoEmergencia>(contacto.Id);
            if (a != null)
            {
                //a.Nombre = empleado.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(ContactoEmergencia contacto)
        {
            db.ContactosEmergencia.Remove(contacto);
            await db.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            ContactoEmergencia? contacto = GetById(id);
            if (contacto != null)
            {
                db.Remove(contacto);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteByEmpleadoIdAsync(int empleadoId)
        {
            List<ContactoEmergencia> contactos = await db.ContactosEmergencia.Where(c => c.EmpleadoId == empleadoId).ToListAsync();
            if (contactos != null && contactos.Count >= 1) { db.ContactosEmergencia.RemoveRange(contactos); }
            await db.SaveChangesAsync();
        }

        public async Task<ICollection<ContactoEmergencia>> GetContactosByEmpleadoIdAsync(int contactoId)
        {
            return await db.ContactosEmergencia.Where(c => c.EmpleadoId == contactoId).ToListAsync();
        }

        public ContactoEmergencia? GetById(int id)
        {
            return db.ContactosEmergencia.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
