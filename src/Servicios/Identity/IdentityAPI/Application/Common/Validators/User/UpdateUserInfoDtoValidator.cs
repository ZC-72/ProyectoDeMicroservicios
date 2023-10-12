using Application.Common.DTOs.User;
using FluentValidation;

namespace Application.Common.Validators.Users;

public class UpdateUserInfoDtoValidator : AbstractValidator<UpdateUserInfoRequest>
{
    public UpdateUserInfoDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(30);
        RuleFor(x => x.UserName)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(30);
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}