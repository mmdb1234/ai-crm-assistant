using System;
using System.Collections.Generic;
using System.Text;

namespace Features.AI_Assistans.Users.GetUsers
{
    public class GetRequest
    {
        public string? SearchText { get; set; }

        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;
    }

}
