using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Events
{
    public class UserBecameVip : IDomainEvent
    {
        public User User { get; }

        public UserBecameVip(User user)
        {
            User = user;
        }
    }
}