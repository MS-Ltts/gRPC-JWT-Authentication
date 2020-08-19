using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GRPC_Web_Client3.Models;
using Grpc.Net.Client;
using GrpcServer.Protos;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Grpc.Core;

using System.Web;

namespace GRPC_Web_Client3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public IConfiguration Configuration { get; set; }
     
        private static string _token;
        private string _server;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public async Task<IActionResult>  Index()
            {
     
            string channelTarget = Configuration["ServerURL"];
            _token = await Authenticate();
            var channel = CreateAuthenticatedChannel(channelTarget);
           
           
            var userClient = new User.UserClient(channel);

            var clientreq = new UserInput { UserId=4 };
            var user = await userClient.GetUserInfoAsync(clientreq);
            var jsonResult = Json(new { getAllResult = user });
            return jsonResult;
        }
        private static GrpcChannel CreateAuthenticatedChannel(string address)
        {
           
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;

            });

            // SslCredentials is used here because this channel is using TLS.

            // Channels that aren't using TLS should use ChannelCredentials.Insecure instead.
            var httpHandler = new HttpClientHandler();

            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

           
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                HttpHandler = httpHandler
            });

            return channel;

        }
        private static async Task<string> Authenticate()

        {
            try {
                Console.WriteLine($"Authenticating as {Environment.UserName}...");

                var httpClient = new HttpClient();

                var request = new HttpRequestMessage

                {

                    RequestUri = new Uri($" https://localhost:5001/generateJwtToken?name={HttpUtility.UrlEncode(Environment.UserName)}"),

                    Method = HttpMethod.Get,

                    Version = new Version(2, 0)

                };
            
                var tokenResponse = await httpClient.SendAsync(request);

                tokenResponse.EnsureSuccessStatusCode();
                var token = await tokenResponse.Content.ReadAsStringAsync();
                Console.WriteLine("Successfully authenticated.");
                return token;
            } 
            catch (Exception ex) {
                throw ex;
                
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
