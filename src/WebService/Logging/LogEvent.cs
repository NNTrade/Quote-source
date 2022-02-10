using Microsoft.Extensions.Logging;

namespace WebService.Logging
{
    public static class LogEvent
    {
        public static EventId CallEndpoint(string ControllerName, string MethodName)
        {
            return new EventId(1, $"Call {ControllerName}.{MethodName}");
        }

    }
}
