using ApplicationManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ApplicationManagement.Models;

[Route("api/[controller]")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly CosmosDbService _cosmosDbService;

    public CandidateController(CosmosDbService cosmosDbService)
    {   
        _cosmosDbService = cosmosDbService;
    }

    // POST: api/Candidates/SubmitApplication
    [HttpPost("SubmitApplication")]
    public async Task<IActionResult> SubmitApplication([FromBody] Candidate candidateAnswer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        candidateAnswer.id = Guid.NewGuid().ToString();

        await _cosmosDbService.AddCandidateAnswerAsync(candidateAnswer);
        return Ok(new { message = "Application submitted successfully!" });
    }
}
