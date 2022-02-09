using Microsoft.Extensions.Logging;
using WebService.Controllers;

namespace WebService
{
    public static class LogEvent
    {
        public static EventId CallEndpoint(string ControllerName, string MethodName)
        {
            return new EventId(1, $"Call {ControllerName}.{MethodName}");
        }

    }
}
