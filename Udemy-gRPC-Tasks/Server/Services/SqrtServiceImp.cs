using Grpc.Core;
using Sqrt;

namespace Server.Services;

public class SqrtServiceImp:SqrtService.SqrtServiceBase
{
    public override async Task<SqrtResponse> Sqrt(SqrtRequest request, ServerCallContext context)
    {
        var number = request.Number;
        if (number >= 0)
            return new SqrtResponse { Sqrt = Math.Sqrt(number) };
        else
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid number"));
    }
}