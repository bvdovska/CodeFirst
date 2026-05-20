using CodeFirst.DTOs;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[ApiController]
[Route("api/pcs")]
public class PCsController : ControllerBase
{
    private readonly IPCService _pcService;

    public PCsController(IPCService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<IEnumerable<PCDto>>> GetPCs()
    {
        var pcs = await _pcService.GetAllPCsAsync();
        return Ok(pcs);
    }

    [HttpGet]
    [Route("{id}/components")]
    public async Task<ActionResult<PCWithComponentsDto>> GetPCComponents(int id)
    {
        var pc = await _pcService.GetPCWithComponentsAsync(id);

        if (pc == null)
        {
            return NotFound();
        }

        return Ok(pc);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<PCDto>> CreatePC([FromBody] PCManipulateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPc = await _pcService.CreatePCAsync(dto);

        return CreatedAtAction(nameof(GetPCs), new { id = createdPc.Id }, createdPc);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdatePC(int id, [FromBody] PCManipulateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updated = await _pcService.UpdatePCAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeletePC(int id)
    {
        var deleted = await _pcService.DeletePCAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}