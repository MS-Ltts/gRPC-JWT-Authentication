gRPC: high-performance Remote Procedure Call (RPC) framework.

There are 2 microservice .

1.User_Server
2.User_Client

JWT Autentication is implemented in this application.
Whenever client make a request server first validate client and then generate a token for client.
After generating token successfully server will execute client's request.

How to run project:
 1. run the server first through visual studio or commad prompt.
 2. run client through visual studio or commad prompt.
	
	2.1 In Client side this method execute first. this method will hit the server and server will validate user and generate token.
	
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

2.2 After generating token client creating a channel by running this method.

private static GrpcChannel CreateAuthenticatedChannel(string address){

            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                HttpHandler = httpHandler
            });
}

2.3 After that client communicating with server by these lines in Index Method.
 var clientreq = new UserInput { UserId=4 };
            var user = await userClient.GetUserInfoAsync(clientreq);

        
2.4 In this project client requesting server to send user data according to user id.