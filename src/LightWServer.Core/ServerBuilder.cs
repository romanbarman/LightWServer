using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;

namespace LightWServer.Core
{
    public sealed class ServerBuilder
    {
        private int port = 80;
        private StaticFileRequestHandler staticFileRequestHandler = new StaticFileRequestHandler("www");
        private ILog log = new SimpleConsoleLog();

        public ServerBuilder SetPort(int port)
        {
            if (port < 1)
                throw new ArgumentException("Invalid port", nameof(port));

            this.port = port;

            return this;
        }

        public ServerBuilder SetStaticFileRequestHandler(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (path.Trim().Equals(string.Empty))
                throw new ArgumentException("Path is empty", nameof(path));

            staticFileRequestHandler = new StaticFileRequestHandler(path);

            return this;
        }

        public ServerBuilder SetLogger(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            return this;
        }

        public LightWServerHost Build()
        {
            return new LightWServerHost(staticFileRequestHandler, log, port);
        }
    }
}
