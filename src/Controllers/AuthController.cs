using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkUpSolution.Application.Helpers;
using LoginRequest = SparkUpSolution.Application.Requests.LoginRequest;

namespace SparkUpSolution.Controllers
{
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private const string SPARKUP_USERNAME = "sparkup";
        private const string SPARKUP_PASSWORD = "sparkup";
        private readonly IJwtHelper jwtHelper;

        public AuthController(IJwtHelper jwtHelper)
        {
            this.jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Returns a Jwt token for simple authentication
        /// </summary>
        /// <remarks>
        /// Use 'sparkup' for both username and password to obtain token
        /// </remarks>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.Username != SPARKUP_USERNAME || request.Password != SPARKUP_PASSWORD)
            {
                return await Task.FromResult(Unauthorized());
            }

            var token = await jwtHelper.GenerateJwtToken(request.Username);

            return Ok(new { token });
        }
    }
}
