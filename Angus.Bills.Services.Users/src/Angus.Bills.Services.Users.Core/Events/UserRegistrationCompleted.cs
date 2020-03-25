using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Events
{
    public class UserRegistrationCompleted : IDomainEvent
    {
        public User User { get; }

        public UserRegistrationCompleted(User user)
        {
            User = user;
        }
    }
}