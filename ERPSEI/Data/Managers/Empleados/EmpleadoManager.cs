using ERPSEI.Data.Entities;
using ERPSEI.Data.Entities.Empleados;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Empleados
{
    public class EmpleadoManager : IEmpleadoManager
    {
        ApplicationDbContext db { get; set; }

        public EmpleadoManager(ApplicationDbContext _db)
        {
            db = _db;
        }

        private async Task<int> getNextId()
        {
            List<Empleado> registros = await db.Empleados.ToListAsync();
            Empleado? last = registros.OrderByDescending(r => r.Id).FirstOrDefault();
            int lastId = last != null ? last.Id : 0;
            lastId += 1;

            return lastId;
        }

        public async Task<int> CreateAsync(Empleado empleado)
        {
            empleado.Id = await getNextId();
            db.Empleados.Add(empleado);
            await db.SaveChangesAsync();
            return empleado.Id;
        }
        public async Task UpdateAsync(Empleado empleado)
        {
            Empleado? a = db.Find<Empleado>(empleado.Id);
            if (a != null)
            {
                //a.Nombre = empleado.Nombre;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Empleado empleado)
        {
            db.Empleados.Remove(empleado);
            await db.SaveChangesAsync();
        }

        public async Task DisableByIdAsync(int id)
        {
            Empleado? empleado = await GetByIdAsync(id);
            AppUser? appUser = db.Users.Where(u => u.EmpleadoId == id).FirstOrDefault();

            if (empleado != null)
            {
                //Deshabilita el empleado
                empleado.Deshabilitado = 1;
                if (appUser != null)
                {
                    appUser.IsBanned = true;
                    db.Update(appUser);
                }

                db.Update(empleado);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            Empleado? empleado = await GetByIdAsync(id);
            if (empleado != null)
            {
                db.Remove(empleado);
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
                    Empleado? empleado = await GetByIdAsync(int.Parse(id));
                    if (empleado != null)
                    {
                        db.Remove(empleado);
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

        public async Task<List<Empleado>> GetAllAsync(
            DateTime? fechaIngresoInicio = null,
            DateTime? fechaIngresoFin = null,
            DateTime? fechaNacimientoInicio = null,
            DateTime? fechaNacimientoFin = null,
            int? puestoId = null,
            int? areaId = null,
            int? subareaId = null,
            int? oficinaId = null,
            bool deshabilitado = false
        )
        {

            return await db.Empleados
                .Where(e => deshabilitado ? true : e.Deshabilitado == 0)
                .Where(e => fechaIngresoInicio != null ? e.FechaIngreso >= fechaIngresoInicio : true)
                .Where(e => fechaIngresoFin != null ? e.FechaIngreso <= fechaIngresoFin : true)
                .Where(e => fechaNacimientoInicio != null ? e.FechaNacimiento >= fechaNacimientoInicio : true)
                .Where(e => fechaNacimientoFin != null ? e.FechaNacimiento <= fechaNacimientoFin : true)
                .Where(e => puestoId != null ? e.PuestoId == puestoId : true)
                .Where(e => areaId != null ? e.AreaId == areaId : true)
                .Where(e => subareaId != null ? e.SubareaId == subareaId : true)
                .Where(e => oficinaId != null ? e.OficinaId == oficinaId : true)
                .Include(e => e.Oficina)
                .Include(e => e.Puesto)
                .Include(e => e.Area)
                .Include(e => e.Subarea)
                .Include(e => e.Genero)
                .Include(e => e.EstadoCivil)
                .ToListAsync();
        }

        public async Task<Empleado?> GetEmpleadoLoginAsync(int id)
        {
            return await db.Empleados
                .Where(e => e.Id == id)
                .Where(e => e.Deshabilitado == 0)
                .FirstOrDefaultAsync();
        }

        public async Task<Empleado?> GetEmpleadoOrganigramaAsync(int id)
        {
            return await db.Empleados
                .Where(e => e.Deshabilitado == 0)
                .Where(e => e.Id == id)
                .Include(e => e.Oficina)
                .Include(e => e.Puesto)
                .Include(e => e.Area)
                .Include(e => e.Subarea)
                .Include(e => e.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil))
                .FirstOrDefaultAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosOrganigramaAsync(int? jefeId, int? puestoId, int? areaId, int? subareaId)
        {
            if (jefeId.HasValue)
            {
                return await db.Empleados
                .Where(e => e.Deshabilitado == 0)
                .Where(e => jefeId.HasValue ? e.JefeId == jefeId : true)
                .Include(e => e.Oficina)
                .Include(e => e.Puesto)
                .Include(e => e.Area)
                .Include(e => e.Subarea)
                .Include(e => e.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil))
                .ToListAsync();
            }
            else
            {
                return await db.Empleados
                .Where(e => e.Deshabilitado == 0)
                .Where(e => puestoId.HasValue ? e.PuestoId == puestoId : true)
                .Where(e => areaId.HasValue ? e.AreaId == areaId : true)
                .Where(e => subareaId.HasValue ? e.SubareaId == subareaId : true)
                .Include(e => e.Oficina)
                .Include(e => e.Puesto)
                .Include(e => e.Area)
                .Include(e => e.Subarea)
                .Include(e => e.ArchivosEmpleado.Where(a => a.TipoArchivoId == (int)FileTypes.ImagenPerfil))
                .ToListAsync();
            }

        }

        public async Task<Empleado?> GetByIdWithAdicionalesAsync(int id)
        {
            return await db.Empleados
                .Where(e => e.Deshabilitado == 0)
                .Where(e => e.Id == id)
                .Include(e => e.ContactosEmergencia)
                .FirstOrDefaultAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await db.Empleados.Where(a => a.Deshabilitado == 0 && a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Empleado?> GetByCURPAsync(string curp)
        {
            return await db.Empleados.Where(e => e.Deshabilitado == 0 && e.CURP == curp).FirstOrDefaultAsync();
        }

        public async Task<Empleado?> GetByNameAsync(string name)
        {
            return await db.Empleados.Where(e => e.Deshabilitado == 0 && e.NombreCompleto.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<Empleado?> GetByEmailAsync(string email)
        {
            return await db.Empleados.Where(e => e.Deshabilitado == 0 && e.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

    }
}
