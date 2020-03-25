using System;
using Angus.Bills.Services.Users.Application.DTO;

namespace Angus.Bills.Services.Users.Application.Exceptions
{
    public class GetUserDetails : IQuery<UserDetailsDto>
    {
        public Guid CustomerId { get; set; }
    }
}