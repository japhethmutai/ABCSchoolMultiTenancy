using FluentValidation;

namespace Application.Features.Schools.Validators
{
    internal class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
    {
        public CreateSchoolRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("School name is required. Please provide one.")
                .MaximumLength(100).WithMessage("School name should be less than 100 characters");
            RuleFor(request => request.EstablishedOn)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Date established cannot be a future date.");
        }
    }
}
