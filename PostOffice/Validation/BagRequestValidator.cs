using FluentValidation;
using PostOffice.Common;
using PostOffice.Common.Requests;
using System.Text.RegularExpressions;

public class BagRequestValidator : AbstractValidator<BagRequest>
{
    public BagRequestValidator()
    {
        RuleFor(x => x.BagNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter bag number")
            .MaximumLength(15);
            //.Matches("^[a-zA-Z]?$")
            //.Must((bagNumber) => IsValidBagNumber(bagNumber))
            //.WithMessage("Please enter bag number in correct format");

        RuleFor(x => x.ContentType)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please letters or parcels")
            
            .Must((bag, ContentType) => BeAValidBagWithLetters(bag.ParcelIds, ContentType))
            .WithMessage("Parcel cann't be added with the this bag")
            .Must((bag, ContentType) => BeAValidBagWithParcel(bag.ParcelIds, ContentType))
            .WithMessage("Please add Parcel");

        RuleFor(x => x.ItemCount)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .LessThanOrEqualTo(0)
            .When(b => b.ContentType == ContentType.Parcel)
            .WithMessage("Item count should be zero");

        RuleFor(x => x.ParcelIds)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .When(b => b.ContentType == ContentType.Parcel)
            .WithMessage("Please add Parcel");

        RuleFor(x => x.Weight).LessThanOrEqualTo(0)
            .When(b => b.ContentType == ContentType.Parcel)
            .WithMessage("Weight should be empty");

        RuleFor(x => x.Price).NotNull().LessThanOrEqualTo(0)
            .When(b => b.ContentType == ContentType.Parcel)
            .WithMessage("Price should be empty");

        RuleFor(x => x.Weight).NotNull().NotEmpty().GreaterThan(0)
            .ScalePrecision(3, 10)
            .When(b => b.ContentType == ContentType.Letter)
            .WithMessage("Weight cannot be empty and must be a precison of 3");

        RuleFor(x => x.Price).NotNull().NotEmpty().GreaterThan(0)
            .ScalePrecision(2, 10)
            .When(b => b.ContentType == ContentType.Letter)
            .WithMessage("Price cannot be empty and must be a precison of 2");

        RuleFor(x => x.ItemCount)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .GreaterThan(0)
            .When(b => b.ContentType == ContentType.Letter)
            .WithMessage("Item count should be grater than zero");

    }

    public static bool IsValidBagNumber(string bagNumber)
    {
        var regex = "^[a-zA-Z][0-9]$";
        return bagNumber.Trim().Length <= 15 && Regex.IsMatch(bagNumber, regex);
    }

    private bool BeAValidBagWithParcel(List<int> parcelIds, ContentType contentType)
    {
        if (contentType == ContentType.Parcel && (parcelIds == null || (parcelIds != null && parcelIds.Count == 0)))
            return false;
        return true;
    }

    private bool BeAValidBagWithLetters(List<int> parcelIds, ContentType contentType)
    {
        if (contentType == ContentType.Letter && parcelIds != null && parcelIds.Count > 0)
            return false;
        return true;
    }
}


