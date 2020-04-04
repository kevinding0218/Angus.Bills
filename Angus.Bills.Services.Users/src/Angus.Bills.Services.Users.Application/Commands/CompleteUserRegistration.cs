using System;
using System.Windows.Input;

namespace Angus.Bills.Services.Users.Application.Commands
{
    [ContractAttribute.ContractAttribute]
    public class CompleteUserRegistration : ICommand
    {
        public Guid CustomerId { get; }
        public string UserName { get; }
        public string Email { get; }

        public CompleteUserRegistration(Guid customerId, string userName, string email)
        {
            CustomerId = customerId;
            UserName = userName;
            Email = email;
        }
    }
}