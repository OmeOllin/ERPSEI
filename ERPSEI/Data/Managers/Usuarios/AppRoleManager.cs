using ERPSEI.Data.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Usuarios
{
	public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(
            IRoleStore<AppRole> store,
            IEnumerable<IRoleValidator<AppRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<AppRole>> logger) :
            base(store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
        }

        public async Task<List<AppRole>> GetAllAsync()
        {

            return await Roles.ToListAsync();
        }
    }
}
