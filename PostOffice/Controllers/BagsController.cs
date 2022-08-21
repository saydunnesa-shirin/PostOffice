namespace PostOffice.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostOffice.Common.Requests;
using PostOffice.Common.Responses;
using PostOffice.Service.Services;

[ApiController]
[Route("[controller]")]
public class BagsController : ControllerBase
{
    private readonly ILogger<BagsController> _logger;
    private IBagService _bagService;

    public BagsController(ILogger<BagsController> logger,
        IBagService bagService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bagService = bagService ?? throw new ArgumentNullException(nameof(bagService));
    }

    [HttpGet]
    public async Task<IEnumerable<BagResponse>> GetAllAsync()
    {
        var bags = await _bagService.GetAllAsync();
        _logger.LogInformation("Got Bag list.");
        return bags;
    }

    [HttpGet("{id}")]
    public async Task<BagResponse> GetByIdAsync(int id)
    {
        var bag = await _bagService.GetByIdAsync(id);
        _logger.LogInformation("Got Bag data.");
        return bag;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(BagRequest model)
    {
        BagRequestValidator validation = new BagRequestValidator();
        validation.ValidateAndThrow(model);

        var result = await _bagService.CreateAsync(model);
        string message;

        if (result > 0)
        {
            message = "Bag created.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to create bag.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(BagRequest model)
    {
        BagRequestValidator validation = new BagRequestValidator();
        validation.ValidateAndThrow(model);

        var result = await _bagService.UpdateAsync(model);
        string message;

        if (result == true)
        {
            message = "Bag updated.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to update bag.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _bagService.DeleteAsync(id);
        string message;

        if (result == true)
        {
            message = "Bag deleted.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to delete bag.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }
}