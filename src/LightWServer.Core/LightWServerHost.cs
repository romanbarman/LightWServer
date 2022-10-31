using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;
using LightWServer.Core.Utils;
using System.Net;
using System.Net.Sockets;

namespace LightWServer.Core
{
    public sealed class LightWServerHost
    {
        private const string UnexpectedErrorMessage = "Unexpected error";

        private readonly IRequestHandler handler;
        private readonly ILog log;
        private readonly int port;

        internal LightWServerHost(IRequestHandler handler, ILog log, int port)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            if (port < 1)
                throw new ArgumentException("Invalid port", nameof(port));

            this.port = port;
        }

        public async Task RunAsync()
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            log.Log(LogLevel.Information, "Server start");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = ProcessClientAsync(client);
            }
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            using (var networkStream = client.GetStream())
            {
                try
                {
                    var request = await RequestParser.ReadAsync(networkStream);

                    var response = handler.Handle(request);

                    await ResponseWriter.WriteResponse(response, networkStream);
                }
                catch(Exception ex)
                {
                    var response = ExceptionUtil.ExceptionToResponse(ex);

                    if (networkStream != null && networkStream.CanWrite)
                    {
                        await ResponseWriter.WriteResponse(response, networkStream);
                    }

                    log.Log(LogLevel.Error,
                        response.StatusCode == HttpStatusCode.InternalServerError ? UnexpectedErrorMessage : ex.Message,
                        ex);
                }
            }
        }
    }
}
