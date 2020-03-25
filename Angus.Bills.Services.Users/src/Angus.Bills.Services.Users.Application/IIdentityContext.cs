using System;
using System.Collections.Generic;

namespace Angus.Bills.Services.Users.Application
{
    public interface IIdentityContext
    {
        Guid Id { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
        bool IsVip { get; }
        IDictionary<string, string> Claims { get; }
    }
}