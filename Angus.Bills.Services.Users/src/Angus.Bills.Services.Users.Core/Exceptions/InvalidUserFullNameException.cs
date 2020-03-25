using System;

namespace Angus.Bills.Services.Users.Core.Exceptions
{
    public class InvalidUserFullNameException : DomainException
    {
        public override string Code => "invalid_user_fullname";
        public Guid Id { get; }
        public string UserName { get; }

        public InvalidUserFullNameException(Guid id, string userName) : base(
            $"User with id: {id} has invalid user name.")
        {
            Id = id;
            UserName = userName;
        }
    }
}