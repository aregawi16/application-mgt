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


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetAllCandidateAnswers()
    {
        var candidateAnswers = await _cosmosDbService.GetAllCandidateAnswersAsync();
        return Ok(candidateAnswers);
    }

    [HttpGet("program/{applicationProgramId}")]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByProgramAsync(string applicationProgramId)
    {
        var candidateAnswers = await _cosmosDbService.GetCandidatesByProgramAsync(applicationProgramId);
        return Ok(candidateAnswers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Candidate>> GetCandidateAnswer(string id)
    {
        var candidateAnswer = await _cosmosDbService.GetCandidateAnswerAsync(id);
        if (candidateAnswer == null)
        {
            return NotFound();
        }
        return Ok(candidateAnswer);
    }


    // POST: api/Candidates/SubmitApplication
    [HttpPost("")]
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
  
  
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCandidateAnswer(string id, [FromBody] Candidate candidateAnswer)
    {
        if (id != candidateAnswer.id)
        {
            return BadRequest("CandidateAnswer ID mismatch");
        }

        await _cosmosDbService.UpdateCandidateAnswerAsync(id, candidateAnswer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCandidateAnswer(string id)
    {
        await _cosmosDbService.DeleteCandidateAnswerAsync(id);
        return NoContent();
    }

}
