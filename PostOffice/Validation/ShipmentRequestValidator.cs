using FluentValidation;
using PostOffice.Common;
using PostOffice.Common.Requests;
using System.Text.RegularExpressions;

public class ShipmentRequestValidator : AbstractValidator<ShipmentRequest>
{
    public ShipmentRequestValidator()
    {
        RuleFor(x => x.ShipmentNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter shipment number")
            .Must((shipmentNumber) => IsValidShipmentNumber(shipmentNumber))
            .WithMessage("Please enter shipment number in correct format");

        RuleFor(x => x.FlightNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter flight number")
            .Must((flightNumber) => IsValidFlightNumber(flightNumber))
            .WithMessage("Please enter flight number in correct format");

        RuleFor(x => x.Airport).NotNull().NotEmpty().WithMessage("Please select the airport");
        RuleFor(x => x.FlightDate).Must(BeAValidDate).WithMessage("Please enter flight date");

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .Must((ShipmentRequest, Status) => BeAValidBags(ShipmentRequest.BagIds, Status))
            .WithMessage("Please add bags") 
            .Must((shipment, Status) => BeAValidDateWhenFinalize(shipment.FlightDate, Status))
            .WithMessage("Please add valid date");
    }

    public static bool IsValidShipmentNumber(string shipmentNumber)
    {
        var regex = "^[0-9a-zA-Z]{3}-[0-9a-zA-Z]{6}$";
        return shipmentNumber.Trim().Length == 10 && Regex.IsMatch(shipmentNumber, regex);
    }

    public static bool IsValidFlightNumber(string flightNumber)
    {
        var regex = "^[a-zA-Z]{2}[0-9]{4}$";
        return flightNumber.Trim().Length == 6 && Regex.IsMatch(flightNumber, regex);
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }

    private bool BeAValidBags(List<int> bagIds, Status status)
    {
        if (status == Status.Finalized && (bagIds == null || (bagIds != null && bagIds.Count == 0)))
            return false;

        return true;
    }
    private bool BeAValidDateWhenFinalize(DateTime date, Status status)
    {
        if (status == Status.Finalized && date < DateTime.Now.AddHours(1))
            return false;

        return true;
    }
}


