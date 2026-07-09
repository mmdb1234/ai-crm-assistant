using FluentValidation;

namespace Features.AI_Assistans.Messages.CreateMessage
{
    public class CreateMessageValidator : AbstractValidator<CreateMessageRequest>
    {
        public CreateMessageValidator()
        {
            RuleFor(x => x.ConversationId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(5000);

            RuleFor(x => x.Role)
                .IsInEnum();
        }
    }

}
