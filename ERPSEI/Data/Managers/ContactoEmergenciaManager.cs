using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class ContactoEmergenciaManager : IRWCatalogoManager<ContactoEmergencia>
    {
        ApplicationDbContext db { get; set; }

        public ContactoEmergenciaManager(ApplicationDbContext _db)
        {
            db = _db;
        }

		private async Task<int> getNextId()
		{
			List<ContactoEmergencia> registros = await GetAllAsync();
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

		public async Task DeleteMultipleByIdAsync(string[] ids)
		{
			//Inicia una transacción.
			await db.Database.BeginTransactionAsync();
			try
			{
				foreach (string id in ids)
				{
					ContactoEmergencia? contacto = GetById(int.Parse(id));
					if (contacto != null)
					{
						db.Remove(contacto);
						await db.SaveChangesAsync();
					}
				}

				await db.Database.CommitTransactionAsync();
			}
			catch (Exception)
			{
				await db.Database.RollbackTransactionAsync();

			}
		}

		public async Task<List<ContactoEmergencia>> GetAllAsync()
		{
			return await db.ContactosEmergencia.ToListAsync();
		}

		public ContactoEmergencia? GetById(int id)
        {
            return db.ContactosEmergencia.Where(a => a.Id == id).FirstOrDefault();
        }

    }
}
