using Microsoft.AspNetCore.Identity;

namespace CodePusle.API.Repositories.Inteface
{
	public interface ITokenRespository
	{
		string CreateJWTToken(IdentityUser user, List<string> roles);
	}
}
