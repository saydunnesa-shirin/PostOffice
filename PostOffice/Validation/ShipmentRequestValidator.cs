using FluentValidation;
using PostOffice.Common.Requests;

public class ShipmentRequestValidator : AbstractValidator<ShipmentRequest>
{
    public ShipmentRequestValidator()
    {
        //RuleFor(x => x.ShipmentNumber).NotNull().NotEmpty()
        //    .WithMessage("Please enter shipment number");
        RuleFor(x => x.ShipmentNumber).NotNull().NotEmpty()
            .Must(BeAValidShipmentNumber)
            .WithMessage("Please enter shipment number in correct format");
        RuleFor(x => x.FlightNumber).NotNull().NotEmpty().WithMessage("Please enter flight number");
        RuleFor(x => x.Airport).NotNull().NotEmpty().WithMessage("Please select the airport");
        RuleFor(x => x.FlightDate).Must(BeAValidDate).WithMessage("Please enter flight date");
    }

    private bool BeAValidShipmentNumber(string arg)
    {
        return false;
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}
