using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Accounts.Commands.Create;

public static class CreateAccount
{
    public record Response(int Id);

    public record Command(string Email, string Password) :IRequest<Response> ;

    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<CreateAccount.Handler> logger) => _logger = logger;

        public Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("HANDLER");


            // TODO : Add Serilog and enricher 
            if(Activity.Current != null)
            {
                foreach (var (key, value) in Activity.Current.Baggage)
                {
                    _logger.LogInformation($"{key} : {value}");
                }
            }

            return Task.FromResult(new Response(1));
        }
    }
}