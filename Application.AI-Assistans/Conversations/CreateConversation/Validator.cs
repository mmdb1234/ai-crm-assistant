

using FluentValidation;

namespace Features.AI_Assistans.Conversations.CreateConversation
{
    public class CreateConversationValidator : AbstractValidator<CreateConversationRequest>
    {
        public CreateConversationValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }

}
