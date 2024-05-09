using ERPSEI.Data.Entities.Empleados;
using ERPSEI.Data.Entities.SAT;
using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Data.Entities.Usuarios
{
    public class AppUser : IdentityUser
    {
        public bool IsBanned { get; set; }
        public bool PasswordResetNeeded { get; set; }
        public bool IsPreregisterAuthorized { get; set; }

        public int? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

        public ICollection<Prefactura> PrefacturasCreadas { get; set; } = new List<Prefactura>();
        public ICollection<Prefactura> PrefacturasAutorizadas { get; set; } = new List<Prefactura>();
        public ICollection<Prefactura> PrefacturasFinalizadas { get; set; } = new List<Prefactura>();

        public ICollection<AccesoModulo> Accesos { get; set; } = new List<AccesoModulo>();
    }
}
