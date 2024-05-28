using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Data.Entities.Usuarios
{
	public class AppRole : IdentityRole
    {
		public AppRole():base() { }

		public AppRole(string roleName):base(roleName) { }

		public ICollection<AccesoModulo> Accesos { get; set; } = new List<AccesoModulo>();
    }
}
