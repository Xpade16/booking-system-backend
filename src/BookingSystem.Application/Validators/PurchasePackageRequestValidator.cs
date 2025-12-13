using FluentValidation;
using BookingSystem.Application.DTOs.Package;

namespace BookingSystem.Application.Validators;

public class PurchasePackageRequestValidator : AbstractValidator<PurchasePackageRequestDto>
{
    public PurchasePackageRequestValidator()
    {
        RuleFor(x => x.PackageId)
            .GreaterThan(0).WithMessage("Package ID must be greater than 0");

        RuleFor(x => x.PaymentToken)
            .NotEmpty().WithMessage("Payment token is required")
            .MaximumLength(500).WithMessage("Payment token is too long");
    }
}