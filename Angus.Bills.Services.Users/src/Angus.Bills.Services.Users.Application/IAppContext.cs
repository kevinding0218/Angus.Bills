namespace Angus.Bills.Services.Users.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }
}