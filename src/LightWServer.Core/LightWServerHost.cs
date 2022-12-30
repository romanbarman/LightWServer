using LightWServer.Core.HttpContext;
using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;
using LightWServer.Core.Services;
using LightWServer.Core.Services.Mappers;
using LightWServer.Core.Utils;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LightWServer.Core
{
    public sealed class LightWServerHost
    {
        private const string UnexpectedErrorMessage = "Unexpected error";

        private readonly IExceptionToResponseMapper exceptionToResponseMapper;
        private readonly IRequestReader requestReader;
        private readonly IResponseWriter responseWriter;
        private readonly IRequestHandler handler;
        private readonly ILog log;
        private readonly int port;

        internal LightWServerHost(IExceptionToResponseMapper exceptionToResponseMapper, IRequestReader requestReader,
            IResponseWriter responseWriter, IRequestHandler handler, ILog log, int port)
        {
            this.exceptionToResponseMapper = exceptionToResponseMapper;
            this.requestReader = requestReader;
            this.responseWriter = responseWriter;
            this.handler = handler;
            this.log = log;

            if (port < 1)
                throw new ArgumentException("Invalid port", nameof(port));

            this.port = port;
        }

        public async Task RunAsync()
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            LogStart();

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
                Request? request = null;
                Response? response = null;

                try
                {
                    request = await requestReader.ReadAsync(networkStream);

                    response = handler.Handle(request);

                    await responseWriter.WriteAsync(response, networkStream);
                }
                catch(Exception ex)
                {
                    response = exceptionToResponseMapper.Map(ex);

                    if (networkStream != null && networkStream.CanWrite)
                    {
                        await responseWriter.WriteAsync(response, networkStream);
                    }

                    log.Log(LogLevel.Error,
                        response.StatusCode == HttpStatusCode.InternalServerError ? UnexpectedErrorMessage : ex.Message,
                        ex);
                }

                if (request != null && response != null)
                    LogResult(request, response);
            }
        }

        private void LogStart()
        {
            log.Log(LogLevel.Information, "Server start");
            log.Log(LogLevel.Information, $"Address: http://localhost:{port}");
        }

        private void LogResult(Request request, Response response)
        {
            var resultMessage = new StringBuilder();
            resultMessage.Append($"{request.HttpMethod} {request.Path}. ");

            var headerInfo = string.Join("|", HeadersFilter.Filter(request).Select(x => $"{x.Name}:{x.Value}"));
            resultMessage.Append($"{headerInfo}. ");

            resultMessage.Append($"{(int)response.StatusCode} {response.StatusCode}");

            log.Log(LogLevel.Information, resultMessage.ToString());
        }
    }
}
