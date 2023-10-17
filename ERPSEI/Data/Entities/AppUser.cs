using ERPSEI.Data.Entities.Empleados;
using Microsoft.AspNetCore.Identity;

namespace ERPSEI.Data.Entities
{
    public class AppUser : IdentityUser
    {
        public bool IsBanned { get; set; }
        public bool PasswordResetNeeded { get; set; }
        public bool IsPreregisterAuthorized {  get; set; }

        public int? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

    }
}
