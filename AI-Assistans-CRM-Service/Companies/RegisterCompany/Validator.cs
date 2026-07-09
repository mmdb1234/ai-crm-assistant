using FluentValidation;
using Features.AI_Assistans.Dtos;

namespace AI_Assistans_CRM_Service.Companies.RegisterCompany;

public class RegisterCompanyValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterCompanyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20);
    }
}
