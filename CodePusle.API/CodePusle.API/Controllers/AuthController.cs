using CodePusle.API.Models.DTO;
using CodePusle.API.Repositories.Inteface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePusle.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly ITokenRespository tokenRespository;

		public AuthController(UserManager<IdentityUser> userManager,
			ITokenRespository tokenRespository)
        {
			this.userManager = userManager;
			this.tokenRespository = tokenRespository;
		}

		// POST: /api/Auth/login

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
		{
			var user = await userManager.FindByEmailAsync(requestDto.Email);

			if (user != null)
			{
				var checkPasswordResult = await userManager.CheckPasswordAsync(user, requestDto.Password);

				if (checkPasswordResult)
				{

					var roles = await userManager.GetRolesAsync(user);

					// Create a token and response

					var jwtToken = tokenRespository.CreateJWTToken(user, roles.ToList());

					var response = new LoginResponseDto()
					{
						Email = requestDto.Email,
						Roles = roles.ToList(),
						Token = jwtToken
					};

					return Ok(response);
				}
			}

			ModelState.AddModelError("", "Email or Password Incorrect.");

			return ValidationProblem(ModelState);
		}

        // POST: /api/Auth/Register

        [HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerRequestDto.Email?.Trim(),
				Email = registerRequestDto.Email?.Trim()
			};

			//Create User

			var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

			if (identityResult.Succeeded)
			{
				// Add roles to user (Reader role).
			
					identityResult = await userManager.AddToRoleAsync(identityUser, "Reader");

					if (identityResult.Succeeded)
					{
						return Ok();
					}
					else
					{
						if (identityResult.Errors.Any())
						{
							foreach (var error in identityResult.Errors)
							{
								ModelState.AddModelError("", error.Description);
							}
						}
					}
			}

			else
			{
				if(identityResult.Errors.Any())
				{
					foreach(var error in identityResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return BadRequest(ModelState);
		}
	}
}
