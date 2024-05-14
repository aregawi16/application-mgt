using ApplicationManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationManagement.Models;
using System;

[Route("api/[controller]")]
[ApiController]
public class ApplicationProgramController : ControllerBase
{
    private readonly CosmosDbService _cosmosDbService;

    public ApplicationProgramController(CosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApplicationProgram>>> GetAllPrograms()
    {
        var programs = await _cosmosDbService.GetAllProgramsAsync();
        return Ok(programs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationProgram>> GetProgram(string id)
    {
        var program = await _cosmosDbService.GetProgramAsync(id);
        if (program == null)
        {
            return NotFound();
        }
        return Ok(program);
    }
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] ApplicationProgram program)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        program.id =  Guid.NewGuid().ToString();
        await _cosmosDbService.AddProgramAsync(program);
        return Ok(new { message = "program created successfully!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgram(string id, [FromBody] ApplicationProgram program)
    {
        if (id != program.id)
        {
            return BadRequest("Program ID mismatch");
        }

        await _cosmosDbService.UpdateProgramAsync(id, program);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgram(string id)
    {
        await _cosmosDbService.DeleteProgramAsync(id);
        return NoContent();
    }
}
