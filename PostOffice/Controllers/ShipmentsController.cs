namespace PostOffice.Controllers;

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PostOffice.Common.Requests;
using PostOffice.Common.Responses;
using PostOffice.Service.Services;

[ApiController]
[Route("[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ILogger<ShipmentsController> _logger;
    private IShipmentService _shipmentService;

    public ShipmentsController(ILogger<ShipmentsController> logger,
        IShipmentService shipmentService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _shipmentService = shipmentService ?? throw new ArgumentNullException(nameof(shipmentService));
    }

    [HttpGet]
    public async Task<IEnumerable<ShipmentResponse>> GetAllAsync()
    {
        var Shipments = await _shipmentService.GetAllAsync();
        _logger.LogInformation("Got Shipment list.");
        return Shipments;
    }

    [HttpGet("{id}")]
    public async Task<ShipmentResponse> GetByIdAsync(int id)
    {
        var Shipment = await _shipmentService.GetByIdAsync(id);
        _logger.LogInformation("Got Shipment data.");
        return Shipment;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(ShipmentRequest model)
    {
        ShipmentRequestValidator validation = new ShipmentRequestValidator();
        validation.ValidateAndThrow(model);

        await _shipmentService.CreateAsync(model);
        var successMessage = "Shipment created.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(ShipmentUpdateRequest model)
    {
        ShipmentRequestValidator validation = new ShipmentRequestValidator();
        validation.ValidateAndThrow(model);

        await _shipmentService.UpdateAsync(model);

        var successMessage = "Shipment updated.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _shipmentService.DeleteAsync(id);

        var successMessage = "Shipment deleted.";
        _logger.LogInformation(successMessage);
        return Ok(successMessage);
    }
}