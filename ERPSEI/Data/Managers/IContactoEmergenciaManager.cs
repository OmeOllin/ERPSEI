using ERPSEI.Data.Entities.Empleados;

namespace ERPSEI.Data.Managers
{
    public interface IContactoEmergenciaManager
    {

        public Task<int> CreateAsync(ContactoEmergencia contacto);

        public Task UpdateAsync(ContactoEmergencia contacto);

        public Task DeleteAsync(ContactoEmergencia contacto);

        public Task DeleteByIdAsync(int contactoId);

        public Task<ICollection<ContactoEmergencia>> GetContactosByEmpleadoIdAsync(int contactoId);

		ContactoEmergencia? GetById(int id);
    }
}