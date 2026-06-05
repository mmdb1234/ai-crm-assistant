using System;
using System.Collections.Generic;
using System.Text;

namespace Features.AI_Assistans.Users.GetUsers
{

    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = default!;

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }


    }

    public class GetUserResponse
    {
        public long TotalCount { get; set; }    
        public List<UserDto> Users { set; get; } = new List<UserDto>();
    }
}
