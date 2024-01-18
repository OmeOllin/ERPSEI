using ERPSEI.Data.Entities.Empleados;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers
{
    public class RoleManager : RoleManager<IdentityRole>
    {
		ApplicationDbContext db { get; set; }

		public RoleManager(
            IRoleStore<IdentityRole> store,
            IEnumerable<IRoleValidator<IdentityRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<IdentityRole>> logger,
			ApplicationDbContext _db) :
            base(store,
				roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
			db = _db;
		}

		public async Task<List<IdentityRole>> GetAllAsync()
		{

			return await db.Roles.ToListAsync();
		}

	}
}
