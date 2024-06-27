using ERPSEI.Data.Entities.Usuarios;

namespace ERPSEI.Data.Managers.Usuarios
{
    public interface IAccesoModuloManager : IRWCatalogoManager<AccesoModulo>
    {
		public Task<List<AccesoModulo>> GetByRolIdAsync(string idRol);

		public Task DeleteByRolIdAsync(string idRol);
	}
}