using ERPSEI.Data.Entities.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPSEI.Data.Managers.Usuarios
{
	public class AppRoleManager : RoleManager<AppRole>
    {
		ApplicationDbContext _db { get; set; }

		public AppRoleManager(
            IRoleStore<AppRole> store,
            IEnumerable<IRoleValidator<AppRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<AppRole>> logger,
            ApplicationDbContext db) :
            base(store,
                roleValidators,
                keyNormalizer,
                errors,
                logger)
        {
            _db = db;
        }

        public async Task<List<AppRole>> GetAllAsync()
        {

            return await _db.Roles
                .Include(r => r.Accesos)
                .ThenInclude(a => a.Modulo)
                .ToListAsync();
        }

        public async Task<AppRole?> GetByNameAsync(string name)
        {
            return await _db.Roles
                .Where(r => r.Name == name)
                .Include(r => r.Accesos)
                .ThenInclude(a => a.Modulo)
                .FirstOrDefaultAsync();
        }

		public async Task<AppRole?> GetByIdAsync(string id)
		{
			return await _db.Roles
				.Where(r => r.Id == id)
                .Include(r => r.Accesos)
                .ThenInclude(a => a.Modulo)
                .FirstOrDefaultAsync();
		}
	}
}
