using Microsoft.AspNetCore.Authorization;

namespace ERPSEI.Authorization
{
	public class AccessRequirement : IAuthorizationRequirement
	{
		public string? ModuleName { get; }

		public AccessRequirement(string moduleName)
		{
			ModuleName = moduleName;
		}
	}
}
