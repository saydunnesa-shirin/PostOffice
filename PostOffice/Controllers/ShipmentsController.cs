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
    public async Task CreateAsync(ShipmentRequest model)
    {
        ShipmentRequestValidator validation = new ShipmentRequestValidator();
        validation.ValidateAndThrow(model);

        await _shipmentService.CreateAsync(model);
        _logger.LogInformation("Shipment created.");
    }

    [HttpPut]
    public async Task UpdateAsync(ShipmentUpdateRequest model)
    {
        await _shipmentService.UpdateAsync(model);
        _logger.LogInformation("Shipment updated.");
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id)
    {
        await _shipmentService.DeleteAsync(id);
        _logger.LogInformation("Shipment deleted.");
    }
}