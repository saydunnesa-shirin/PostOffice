using System.Collections;
using System.Text.RegularExpressions;
using FluentValidation;
using PostOffice.Common;
using PostOffice.Common.Requests;

namespace PostOffice.Api.Validation;

public class ShipmentRequestValidator : AbstractValidator<ShipmentRequest>
{
    public ShipmentRequestValidator()
    {
        RuleFor(x => x.ShipmentNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter shipment number")
            .Must(IsValidShipmentNumber)
            .WithMessage("Please enter shipment number in correct format");

        RuleFor(x => x.FlightNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter flight number")
            .Must(IsValidFlightNumber)
            .WithMessage("Please enter flight number in correct format");

        RuleFor(x => x.Airport).NotNull().NotEmpty().WithMessage("Please select the airport");
        RuleFor(x => x.FlightDate).Must(BeAValidDate).WithMessage("Please enter flight date");

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .Must((shipmentRequest, status) => shipmentRequest.BagIds != null && BeAValidBags(shipmentRequest.BagIds, status))
            .WithMessage("Please add bags") 
            .Must((shipment, status) => BeAValidDateWhenFinalize(shipment.FlightDate, status))
            .WithMessage("Please add valid date");
    }

    public static bool IsValidShipmentNumber(string shipmentNumber)
    {
        const string regex = "^[0-9a-zA-Z]{3}-[0-9a-zA-Z]{6}$";
        return shipmentNumber.Trim().Length == 10 && Regex.IsMatch(shipmentNumber, regex);
    }

    public static bool IsValidFlightNumber(string flightNumber)
    {
        const string regex = "^[a-zA-Z]{2}[0-9]{4}$";
        return flightNumber.Trim().Length == 6 && Regex.IsMatch(flightNumber, regex);
    }

    private static bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }

    private static bool BeAValidBags(ICollection? bagIds, Status status)
    {
        return status != Status.Finalized || (bagIds != null && bagIds.Count != 0);
    }
    private static bool BeAValidDateWhenFinalize(DateTime date, Status status)
    {
        return status != Status.Finalized || date >= DateTime.Now.AddHours(1);
    }
}