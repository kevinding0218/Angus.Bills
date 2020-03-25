using Newtonsoft.Json;

namespace Drex.CQRS.Events
{
    public class RejectedEvent : IRejectedEvent
    {
        [JsonConstructor]
        public RejectedEvent(string reason, string code)
        {
            Reason = reason;
            Code = code;
        }

        public string Reason { get; }
        public string Code { get; }

        public static IRejectedEvent For(string name)
        {
            return new RejectedEvent("There was an error when executing: " +
                                     $"{name}", $"{name}_error");
        }
    }
}