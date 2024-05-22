using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Data.Entities.Usuarios
{
	public class AppRole : IdentityRole
    {
		public ICollection<AccesoModulo> Accesos { get; set; } = new List<AccesoModulo>();
    }
}
