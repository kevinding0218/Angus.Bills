using System;

namespace Angus.Bills.Services.Users.Core.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public override string Code => "user_not_found";
        public Guid Id { get; }

        public UserNotFoundException(Guid id) : base($"User with id: {id} was not found.")
        {
            Id = id;
        }
    }
}