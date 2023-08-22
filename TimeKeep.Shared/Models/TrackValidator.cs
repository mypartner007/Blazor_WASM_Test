using FluentValidation;

namespace TimeKeep.Shared.Models
{
    public class TrackValidator : AbstractValidator<Track>
    {
        public TrackValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(working => working.Hours).NotEmpty().WithMessage("Hours is a required field.");
            RuleFor(working => working.Date).NotEmpty().WithMessage("WorkingDay is a required field.");
        }
    }
}