using System;
using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Exceptions
{
    public class CannotChangeUserStateException : DomainException
    {
        public override string Code => "cannot_change_user_state";
        public Guid Id { get; }
        public UserState UserState { get; }

        public CannotChangeUserStateException(Guid id, UserState userState) : base(
            $"Cannot change user: {id} state to: {userState}.")
        {
            Id = id;
            UserState = userState;
        }
    }
}