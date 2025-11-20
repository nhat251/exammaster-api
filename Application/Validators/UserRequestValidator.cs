using Application.DTOs.Requests;
using FluentValidation;

namespace Application.Validators
{
    public class UserRequestValidator : AbstractValidator<RegisterRequest>
    {
        public UserRequestValidator() {
            RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
        }
    }
}
