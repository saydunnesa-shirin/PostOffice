using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostOffice.Api.Validation;
using PostOffice.Common.Requests;
using PostOffice.Common.Responses;
using PostOffice.Service.Services;

namespace PostOffice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ParcelsController : ControllerBase
{
    private readonly ILogger<ParcelsController> _logger;
    private readonly IParcelService _parcelService;

    public ParcelsController(ILogger<ParcelsController> logger,
        IParcelService parcelService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _parcelService = parcelService ?? throw new ArgumentNullException(nameof(parcelService));
    }

    [HttpGet]
    public async Task<IEnumerable<ParcelResponse>> GetAllAsync()
    {
        var parcels = await _parcelService.GetAllAsync();
        _logger.LogInformation("Got Parcel List.");
        return parcels;
    }

    [HttpGet("{id}")]
    public async Task<ParcelResponse> GetByIdAsync(int id)
    {
        var parcel = await _parcelService.GetByIdAsync(id);
        _logger.LogInformation("Got Parcel data.");
        return parcel;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ParcelRequest model)
    {
        var validation = new ParcelRequestValidator();
        await validation.ValidateAndThrowAsync(model);

        var result = await _parcelService.CreateAsync(model);

        string message;

        if (result > 0)
        {
            message = "Parcel created.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to create parcel.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ParcelRequest model)
    {
        var validation = new ParcelRequestValidator();
        await validation.ValidateAndThrowAsync(model);

        var result = await _parcelService.UpdateAsync(model);
        string message;

        if (result)
        {
            message = "Parcel updated.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to update parcel.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }
}