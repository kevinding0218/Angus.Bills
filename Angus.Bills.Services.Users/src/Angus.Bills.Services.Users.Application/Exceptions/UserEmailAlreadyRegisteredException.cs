using System;

namespace Angus.Bills.Services.Users.Application.Exceptions
{
    public class EmailAddressAlreadyRegisteredException : AppException
    {
        public override string Code => "email_address_already_registered";
        public string Email { get; }
        
        public EmailAddressAlreadyRegisteredException(string email) 
            : base($"Email address {email} has already been registered.")
        {
            Email = email;
        }
    }
}