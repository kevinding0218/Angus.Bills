using System;

namespace Angus.Bills.HTTP.RestEase
{
    public interface IRestEaseOptionsBuilder
    {
        IRestEaseOptionsBuilder WithLoadBalancer(string loadBalancer);
        IRestEaseOptionsBuilder WithService(Func<IRestEaseServiceBuilder, IRestEaseServiceBuilder> buildService);
        RestEaseOptions Build();
    }
}