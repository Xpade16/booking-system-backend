using FluentValidation;
using BookingSystem.Application.DTOs.Schedule;

namespace BookingSystem.Application.Validators;

public class CreateScheduleValidator : AbstractValidator<CreateScheduleDto>
{
    public CreateScheduleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required")
            .Length(2, 10).WithMessage("Country code must be between 2 and 10 characters");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0")
            .LessThanOrEqualTo(1000).WithMessage("Capacity must not exceed 1000");

        RuleFor(x => x.RequiredCredits)
            .GreaterThan(0).WithMessage("Required credits must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Required credits must not exceed 10");
    }
}