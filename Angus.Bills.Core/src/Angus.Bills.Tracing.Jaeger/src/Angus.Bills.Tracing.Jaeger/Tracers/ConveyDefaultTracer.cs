using System.Reflection;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using OpenTracing;

namespace Angus.Bills.Tracing.Jaeger.Tracers
{
    internal sealed class DefaultTracer
    {
        public static ITracer Create()
        {
            return new Tracer.Builder(Assembly.GetEntryAssembly().FullName)
                .WithReporter(new NoopReporter())
                .WithSampler(new ConstSampler(false))
                .Build();
        }
    }
}