namespace PostOffice.Controllers;

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
    public async Task CreateAsync(BagRequest model)
    {
        await _bagService.CreateAsync(model);
        _logger.LogInformation("Bag created.");
    }

    [HttpPut]
    public async Task UpdateAsync(BagUpdateRequest model)
    {
        await _bagService.UpdateAsync(model);
        _logger.LogInformation("Bag updated.");
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _bagService.DeleteAsync(id);
        _logger.LogInformation("Bag deleted.");
    }
}