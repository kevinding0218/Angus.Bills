using System;

namespace Angus.Bills.Services.Users.Application.Exceptions
{
    public class UserNameAlreadyRegisteredException : AppException
    {
        public override string Code => "user_name_already_registered";
        public string UserName { get; }
        
        public UserNameAlreadyRegisteredException(string userName) 
            : base($"User name {userName} has already been registered.")
        {
            UserName = userName;
        }
    }
}