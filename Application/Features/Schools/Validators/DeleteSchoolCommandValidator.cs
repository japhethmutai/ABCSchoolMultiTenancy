using Application.Features.Schools.Commands;
using Domain.Entities;
using FluentValidation;

namespace Application.Features.Schools.Validators
{
    public class DeleteSchoolCommandValidator : AbstractValidator<DeleteSchoolCommand>
    {
        public DeleteSchoolCommandValidator(ISchoolService schoolService)
        {
            RuleFor(command => command.SchoolId)
                .NotEmpty()
                .MustAsync(async (id, ct) => await schoolService.GetSchoolByIdAsync(id) is School schoolInDb && schoolInDb.Id == id)
                .WithMessage("School does not exist.");
        }
    }
}
