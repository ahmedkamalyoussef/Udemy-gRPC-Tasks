using ComputeAverage;
using Grpc.Core;

namespace Server.Services;

public class ComputeAverageServiceImp: ComputeAverageService.ComputeAverageServiceBase
{
    public override async Task<PrimesResponse> ComputeAverage(IAsyncStreamReader<RequestNumber> requestStream, ServerCallContext context)
    {
        int sum = 0;
        int times = 0;
        while (await requestStream.MoveNext())
        {
            sum += requestStream.Current.Argument.Number;
            times++;
        }
        return new PrimesResponse(){Result = sum/times};
    }
}