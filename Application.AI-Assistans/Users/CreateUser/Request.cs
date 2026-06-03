namespace Features.AI_Assistans.Users.CreateUser
{
    public class CreateUserRequest
    {
        public string UserName { get; set; } = default!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }

}
