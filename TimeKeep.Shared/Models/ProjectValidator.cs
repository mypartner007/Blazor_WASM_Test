using FluentValidation;

namespace TimeKeep.Shared.Models
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(project => project.Name).NotEmpty().WithMessage("Name is a required field.")
                .Length(3, 50).WithMessage("Name must be between 3 and 50 characters.");
        }
    }
}