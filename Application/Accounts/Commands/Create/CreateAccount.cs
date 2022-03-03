using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;

namespace Application.Accounts.Commands.Create;

public static class CreateAccount
{
    public record Response(int Id);

    public record Command(string Email, string Password) :IRequest<Response> ;

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly HttpClient _httpClient;

        public Handler(ILogger<CreateAccount.Handler> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("WeatherApi");
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("HANDLER");


            // TODO : Add Serilog and enricher 
            if (Activity.Current != null)
            {

                _logger.LogInformation($"TRACEID: {Activity.Current.TraceId}");


                foreach (var (key, value) in Activity.Current.Baggage)
                {
                    _logger.LogInformation($"{key} : {value}");
                }
            }

            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}WeatherForecast");

            return new Response(1);
        }
    }
}