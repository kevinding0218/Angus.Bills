using System;
using System.Collections.Generic;

namespace Angus.Bills.Services.Users.Application.DTO
{
    public class UserDetailsDto : UserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserLevel { get; set; }
        public bool IsVip { get; set; }
        public IEnumerable<Guid> LoggedTransactions { get; set; }
    }
}
}