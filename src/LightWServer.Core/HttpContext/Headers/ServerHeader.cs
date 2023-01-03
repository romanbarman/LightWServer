using System.Diagnostics;
using System.Reflection;

namespace LightWServer.Core.HttpContext.Headers
{
    internal sealed class ServerHeader : Header
    {
        private const string HeaderName = "Server";

        private static string Version = FileVersionInfo
            .GetVersionInfo(Assembly.GetExecutingAssembly().Location)
            .ProductVersion ?? "0.0.1";
        private static string HeaderValue = $"LightWServer/{Version}";

        internal ServerHeader() : base(HeaderName, HeaderValue) { }
    }
}
