using ERPSEI.Data.Entities.Usuarios;
using ERPSEI.Data.Managers.Usuarios;
using Microsoft.AspNetCore.Authorization;

namespace ERPSEI.Authorization
{
	public class AccessHandler : AuthorizationHandler<AccessRequirement>
	{
		readonly IModuloManager _moduloManager;

		public AccessHandler(IModuloManager moduloManager) 
		{ 
			_moduloManager = moduloManager;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirement requirement)
		{
			DefaultHttpContext? httpContext = (DefaultHttpContext)context.Resource ?? null;
			string moduleName = httpContext?.GetEndpoint()?.DisplayName ?? string.Empty;
			string[] moduleNameParts = moduleName.Split("/", StringSplitOptions.RemoveEmptyEntries);
			if (moduleNameParts.Length >= 1){ moduleName = moduleNameParts[0]; }

			Modulo? m = await _moduloManager.GetByNameAsync(moduleName??string.Empty);

            if (m != null)
			{
				bool succeeded = false;

				foreach (AccesoModulo acceso in m.Accesos)
				{
					if (context.User.IsInRole(acceso.Rol?.Name ?? ""))
					{
						succeeded = acceso.PuedeConsultar == 1;
						break;
					}
				}

				if (succeeded) { context.Succeed(requirement); }
			}
		}
	}
}
