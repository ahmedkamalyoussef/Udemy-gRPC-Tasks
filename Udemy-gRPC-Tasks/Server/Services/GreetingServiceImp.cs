using Greeting;
using Grpc.Core;

namespace Server.Services;

public class GreetingServiceImp:GreetingService.GreetingServiceBase
{
    public override async Task<GreetResponse> GreetWithDeadLine(GreetRequest request, ServerCallContext context)
    {
        await Task.Delay(5000);
        return new GreetResponse(){Result = "Hello "+request.Name};
    }
}