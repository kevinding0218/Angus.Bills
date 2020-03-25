using System.Linq;
using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Services
{
    public interface IVipPolicy
    {
        void ApplyVipStatusIfEligible(User user);
    }
    
    public class VipPolicy : IVipPolicy
    {
        public void ApplyVipStatusIfEligible(User user)
        {
            if (user.IsVip)
            {
                return;
            }

            if (user.LoggedTransactions.Count() < 1500)
            {
                return;
            }
            
            user.SetVip();
        }
    }
}