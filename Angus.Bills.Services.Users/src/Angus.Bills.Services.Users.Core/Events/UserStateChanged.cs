using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Events
{
    public class UserStateChanged : IDomainEvent
    {
        public User User { get; }
        public UserState PreviousState { get; }

        public UserStateChanged(User user, UserState previousState)
        {
            User = user;
            PreviousState = previousState;
        }
    }
}