using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostOffice.Api.Validation;
using PostOffice.Common.Requests;
using PostOffice.Common.Responses;
using PostOffice.Service.Services;

namespace PostOffice.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ILogger<ShipmentsController> _logger;
    private readonly IShipmentService _shipmentService;

    public ShipmentsController(ILogger<ShipmentsController> logger,
        IShipmentService shipmentService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _shipmentService = shipmentService ?? throw new ArgumentNullException(nameof(shipmentService));
    }

    [HttpGet]
    public async Task<IEnumerable<ShipmentResponse>> GetAllAsync()
    {
        var shipments = await _shipmentService.GetAllAsync();
        _logger.LogInformation("Got Shipment list.");
        return shipments;
    }

    [HttpGet("{id}")]
    public async Task<ShipmentResponse> GetByIdAsync(int id)
    {
        var shipment = await _shipmentService.GetByIdAsync(id);
        _logger.LogInformation("Got Shipment data.");
        return shipment;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ShipmentRequest model)
    {
        var validation = new ShipmentRequestValidator();
        await validation.ValidateAndThrowAsync(model);

        var result = await _shipmentService.CreateAsync(model);

        string message;

        if (result > 0)
        {
            message = "Shipment created.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to create shipment.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ShipmentUpdateRequest model)
    {
        var validation = new ShipmentRequestValidator();
        await validation.ValidateAndThrowAsync(model);

        var result = await _shipmentService.UpdateAsync(model);

        string message;

        if (result)
        {
            message = "Shipment updated.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to update shipment.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _shipmentService.DeleteAsync(id);

        string message;

        if (result)
        {
            message = "Shipment deleted.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to delete shipment.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }
}