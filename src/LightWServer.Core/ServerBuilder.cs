using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;
using LightWServer.Core.Services;
using LightWServer.Core.Services.FileOperation;
using LightWServer.Core.Services.Mappers;

namespace LightWServer.Core
{
    public sealed class ServerBuilder
    {
        private readonly IFileOperationService fileOperationService;

        private int port;
        private StaticFileRequestHandler staticFileRequestHandler;
        private ILog log;

        public ServerBuilder()
        {
            fileOperationService = new FileOperationService();
            port = 80;
            staticFileRequestHandler = new StaticFileRequestHandler("www", fileOperationService);
            log = new SimpleConsoleLog();
        }

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

            staticFileRequestHandler = new StaticFileRequestHandler(path, fileOperationService);

            return this;
        }

        public ServerBuilder SetLogger(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            return this;
        }

        public LightWServerHost Build()
        {
            return new LightWServerHost(new ExceptionToResponseMapper(), new RequestReader(), new ResponseWriter(fileOperationService),
                staticFileRequestHandler, log, port);
        }
    }
}
