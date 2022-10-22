using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;

namespace LightWServer.Core
{
    public sealed class ServerBuilder
    {
        private int port = 80;
        private StaticFileRequestHandler staticFileRequestHandler = new StaticFileRequestHandler("www");
        private ILog log = new SimpleComsoleLog();

        public ServerBuilder SetPort(int port)
        {
            if (port < 1)
                throw new ArgumentException("Invalid port", nameof(port));

            this.port = port;

            return this;
        }

        public ServerBuilder SetStaticFileRequestHandler(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

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
