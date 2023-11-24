using AIHouseKeeperBackend.AIDomain.Constants;
using AIHouseKeeperBackend.AIDomain.Services;
using AIHouseKeeperBackend.AIDomain.ViewModels;
using AIHouseKeeperBackend.AuthorisationDomain.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AIHouseKeeperBackend.Controllers;

[Authorise]
[ApiController]
[Route("[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiService _aiService;

    public AiController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("Prompt")]
    public async Task<ActionResult> Prompt(PromptRequestViewModel viewModel)
    {
        var type = await _aiService.DetectPromptTypeAsync(viewModel);
        if (type == PromptType.Statement)
        {
            await _aiService.StoreMemoryAsync(viewModel);
            return Ok(
                new
                {
                    Message = "toast: statement stored"
                });
        }

        string res;
        try
        {
            res = (await _aiService.GetAnswerFromPromptAsync(viewModel)).Content;
        }
        catch (InvalidOperationException e)
        {
            return Ok(new
            {
                Message = "Ops, seems you didn't tell me anything yet"
            });
        }

        return Ok(new
        {
            Message = res
        });
    }

    [HttpGet("Memory")]
    public async Task<ActionResult<List<string>>> Memory(
        [FromQuery] long userId)
    {
        return Ok(await _aiService.GetMemoryList(userId));
    }
}