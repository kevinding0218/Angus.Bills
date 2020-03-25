using System;

namespace Angus.Bills.Services.Users.Application.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}