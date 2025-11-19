using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SparkUpSolution.Application.DTOs;
using SparkUpSolution.Application.Requests;
using SparkUpSolution.Application.Services;

namespace SparkUpSolution.Controllers
{
    [ApiController]
    [Route("api/bonus")]
    [Authorize] // should be authorized, task does not specify what kind so went with simple JWT auth
    public class BonusController : ControllerBase
    {
        private readonly IBonusService bonusService;

        public BonusController(IBonusService bonusService)
        {
            this.bonusService = bonusService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<PagedResult<BonusDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var result = await bonusService.GetAllAsync(pageNumber, pageSize, ct);

            return Ok(result);
        }

    }
}
