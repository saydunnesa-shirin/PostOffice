using System.Collections;
using FluentValidation;
using PostOffice.Common;
using PostOffice.Common.Requests;

namespace PostOffice.Api.Validation;

public class BagRequestValidator : AbstractValidator<BagRequest>
{
    public BagRequestValidator()
    {
        RuleFor(x => x.BagNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please enter bag number")
            .MaximumLength(15);
        

        RuleFor(x => x.ContentType)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .WithMessage("Please letters or parcels")
            
            .Must((bag, contentType) => bag.ParcelIds != null && BeAValidBagWithLetters(bag.ParcelIds, contentType))
            .WithMessage("Parcel cannot be added with the this bag")
            .Must((bag, contentType) => bag.ParcelIds != null && BeAValidBagWithParcel(bag.ParcelIds, contentType))
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
            .WithMessage("Weight cannot be empty and must be a precision of 3");

        RuleFor(x => x.Price).NotNull().NotEmpty().GreaterThan(0)
            .ScalePrecision(2, 10)
            .When(b => b.ContentType == ContentType.Letter)
            .WithMessage("Price cannot be empty and must be a precision of 2");

        RuleFor(x => x.ItemCount)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotNull().NotEmpty()
            .GreaterThan(0)
            .When(b => b.ContentType == ContentType.Letter)
            .WithMessage("Item count should be grater than zero");

    }

    private static bool BeAValidBagWithParcel(ICollection? parcelIds, ContentType contentType)
    {
        return contentType != ContentType.Parcel || (parcelIds != null && parcelIds.Count > 0);
    }

    private bool BeAValidBagWithLetters(ICollection? parcelIds, ContentType contentType)
    {
        return contentType != ContentType.Letter || parcelIds == null || parcelIds.Count <= 0;
    }
}