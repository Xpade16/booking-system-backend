using FluentValidation;
using BookingSystem.Application.DTOs.Schedule;

namespace BookingSystem.Application.Validators;

public class UpdateScheduleValidator : AbstractValidator<UpdateScheduleDto>
{
    public UpdateScheduleValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.Title), () =>
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
        });

        When(x => x.Capacity.HasValue, () =>
        {
            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0")
                .LessThanOrEqualTo(1000).WithMessage("Capacity must not exceed 1000");
        });

        When(x => x.RequiredCredits.HasValue, () =>
        {
            RuleFor(x => x.RequiredCredits)
                .GreaterThan(0).WithMessage("Required credits must be greater than 0")
                .LessThanOrEqualTo(10).WithMessage("Required credits must not exceed 10");
        });
    }
}