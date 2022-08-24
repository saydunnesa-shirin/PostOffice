using System.Text.RegularExpressions;
using FluentValidation;
using PostOffice.Common.Requests;

namespace PostOffice.Api.Validation;

public class ParcelRequestValidator : AbstractValidator<ParcelRequest>
{
    public ParcelRequestValidator()
    {
        RuleFor(x => x.ParcelNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter parcel number")
            .Must(IsValidParcelNumber)
            .WithMessage("Please enter parcel number in correct format");

        RuleFor(x => x.RecipientName).MaximumLength(100);

        RuleFor(x => x.DestinationCountry)
            .Must(IsValidDestinationCountry)
            .WithMessage("Please enter destination country in correct format");

        RuleFor(x => x.Weight).NotNull().NotEmpty().ScalePrecision(3, 10).WithMessage("Weight cannot be empty and maximum precision of 3");
        RuleFor(x => x.Price).NotNull().NotEmpty().ScalePrecision(2, 10).WithMessage("Price cannot be empty and maximum precision of 2");
    }

    public static bool IsValidParcelNumber(string parcelNumber)
    {
        const string regex = "^[a-zA-Z]{2}[0-9]{6}[a-zA-Z]{2}$";
        return parcelNumber.Trim().Length == 10 && Regex.IsMatch(parcelNumber, regex);
    }

    public static bool IsValidDestinationCountry(string destinationCountry)
    {
        const string regex = "^[a-zA-Z]{2}$";
        return destinationCountry.Trim().Length == 2 && Regex.IsMatch(destinationCountry, regex);
    }
}