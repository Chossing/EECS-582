using System.Collections.Generic;

namespace Fleck
{
    public interface IWebSocketConnectionInfo
    {
        string SubProtocol { get; }
        string Origin { get; }
        string Host { get; }
        string Path { get; }
        string ClientIpAddress { get; }
        IDictionary<string, string> Cookies { get; }
    }
}
