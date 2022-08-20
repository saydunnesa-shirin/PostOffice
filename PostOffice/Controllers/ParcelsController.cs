namespace PostOffice.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostOffice.Common.Requests;
using PostOffice.Common.Responses;
using PostOffice.Service.Services;

[ApiController]
[Route("[controller]")]
public class ParcelsController : ControllerBase
{
    private readonly ILogger<ParcelsController> _logger;
    private IParcelService _ParcelService;

    public ParcelsController(ILogger<ParcelsController> logger,
        IParcelService ParcelService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ParcelService = ParcelService ?? throw new ArgumentNullException(nameof(ParcelService));
    }

    [HttpGet]
    public async Task<IEnumerable<ParcelResponse>> GetAllAsync()
    {
        var Parcels = await _ParcelService.GetAllAsync();
        _logger.LogInformation("Got Parcel List.");
        return Parcels;
    }

    [HttpGet("{id}")]
    public async Task<ParcelResponse> GetByIdAsync(int id)
    {
        var Parcel = await _ParcelService.GetByIdAsync(id);
        _logger.LogInformation("Got Parcel data.");
        return Parcel;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ParcelRequest model)
    {
        ParcelRequestValidator validation = new ParcelRequestValidator();
        validation.ValidateAndThrow(model);

        await _ParcelService.CreateAsync(model);
        var successMessage = "Bag created.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ParcelRequest model)
    {
        await _ParcelService.UpdateAsync(model);
        var successMessage = "Bag updated.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _ParcelService.DeleteAsync(id);
        var successMessage = "Bag deleted.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);
    }
}