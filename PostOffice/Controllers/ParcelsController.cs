﻿namespace PostOffice.Controllers;

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

        var result = await _ParcelService.CreateAsync(model);

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
        var result = await _ParcelService.UpdateAsync(model);
        string message;

        if (result == true)
        {
            message = "Parcel updated.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to update parcel.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _ParcelService.DeleteAsync(id);
        string message;

        if (result == true)
        {
            message = "Parcel deleted.";
            _logger.LogInformation(message);
            return Ok(message);
        }

        message = "Failed to delete parcel.";
        _logger.LogInformation(message);
        return BadRequest(message);
    }
}