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
        public async Task<ActionResult<PagedResult<BonusDTO>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await bonusService.GetAllAsync(pageNumber, pageSize, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<BonusDTO>> Create([FromBody] CreateBonusRequest request, CancellationToken cancellationToken = default)
        {
            var created = await bonusService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<ActionResult<BonusDTO>> Update(Guid id, [FromBody] UpdateBonusRequest request, CancellationToken cancellationToken = default)
        {
            var updated = await bonusService.UpdateAsync(id, request, cancellationToken);

            return Ok(updated);
        }

        [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await bonusService.DeleteAsync(id, cancellationToken);
            
            return NoContent();
        }
    }
}
