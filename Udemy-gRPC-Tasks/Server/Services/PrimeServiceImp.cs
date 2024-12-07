
using Grpc.Core;
using Prime;

namespace Server.Services;

public class PrimeServiceImp:PrimesService.PrimesServiceBase
{
    public override async Task Primes(NumberRequest request, IServerStreamWriter<PrimesResponse> responseStream, ServerCallContext context)
    {
        var k = 2;
        var n = request.Argument.Number;
        while (n>1)
        {
            if (n % k == 0)
            {
                n /= k;
               await responseStream.WriteAsync(new PrimesResponse { Result = k });
            }
            else
            {
                k += 1;
            }
        }
    }
}