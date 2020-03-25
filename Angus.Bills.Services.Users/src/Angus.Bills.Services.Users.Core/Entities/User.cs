using System;
using System.Collections.Generic;
using System.Linq;
using Angus.Bills.Services.Users.Core.Events;
using Angus.Bills.Services.Users.Core.Exceptions;

namespace Angus.Bills.Services.Users.Core.Entities
{
    public class User : AggregateRoot
    {
        private ISet<Guid> _loggedTransactions = new HashSet<Guid>();

        public string Email { get; private set; }
        public string UserName { get; private set; }
        public bool IsVip { get; private set; }
        public UserLevel UserLevel { get; private set; }
        public UserState UserState { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IEnumerable<Guid> LoggedTransactions
        {
            get => _loggedTransactions;
            set => _loggedTransactions = new HashSet<Guid>(value);
        }

        public User(Guid id, string email, DateTime createdAt) : this(id, email, createdAt, string.Empty,
            string.Empty, false, UserState.Incomplete, Enumerable.Empty<Guid>())
        {
        }

        public User(Guid id, string email, DateTime createdAt, string userName, string address, bool isVip,
            UserLevel userLevel, UserState state, IEnumerable<Guid> loggedTransactions = null)
        {
            Id = id;
            Email = email;
            CreatedAt = createdAt;
            UserName = userName;
            IsVip = isVip;
            LoggedTransactions = loggedTransactions ?? Enumerable.Empty<Guid>();
            UserLevel = userLevel;
            UserState = state;
        }

        public void CompleteRegistration(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidUserFullNameException(Id, userName);
            }

            if (UserState != UserState.Incomplete)
            {
                throw new CannotChangeUserStateException(Id, UserState);
            }

            UserName = userName;
            UserLevel = UserLevel.Newbee;
            UserState = UserState.Valid;
            AddEvent(new UserRegistrationCompleted(this));
        }

        public void SetValid() => SetUserState(UserState.Valid);
        
        public void SetIncomplete() => SetUserState(UserState.Incomplete);

        public void Lock() => SetUserState(UserState.Locked);

        public void MarkAsSuspicious() => SetUserState(UserState.Suspicious);

        private void SetUserState(UserState state)
        {
            var previousState = UserState;
            UserState = state;
            AddEvent(new UserStateChanged(this, previousState));
        }

        public void SetVip()
        {
            if (IsVip)
            {
                return;
            }

            IsVip = true;
            UserLevel = UserLevel.Unstoppable;
            AddEvent(new UserBecameVip(this));
        }

        public void AddLoggedTransactions(Guid transactionId)
        {
            if (transactionId.Equals(Guid.Empty))
            {
                return;
            }

            _loggedTransactions.Add(transactionId);
        }
    }
}