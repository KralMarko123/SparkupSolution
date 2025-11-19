using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkUpSolution.Application.Helpers;
using LoginRequest = SparkUpSolution.Application.Requests.LoginRequest;

namespace SparkUpSolution.Controllers
{
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IJwtHelper jwtHelper;

        public AuthController(IJwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.Username != "sparkup" || request.Password != "sparkup")
            {
                return await Task.FromResult(Unauthorized());
            }

            var token = await jwtHelper.GenerateJwtToken(request.Username);

            return Ok(new { token });
        }
    }
}
