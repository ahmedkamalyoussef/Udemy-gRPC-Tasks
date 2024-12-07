using ComputeAverage;
using Greeting;
using Grpc.Core;
using Prime;
using Sqrt;
using Argument = Prime.Argument;

internal class Program
{
    const string target = "127.0.0.1:50052";

    private async static Task Main(string[] args)
    {
        Channel channel = new Grpc.Core.Channel(target, ChannelCredentials.Insecure);
        await channel.ConnectAsync().ContinueWith((task) =>
        {
            if (task.Status == TaskStatus.RanToCompletion)
                Console.WriteLine("connected");
        });

        // await CallPrimeService(channel);
        // await CallComputeAverageService(channel);
        // await CallSqrtService(channel);
        await CallGreetingService(channel);
        
        
        
        
        await channel.ShutdownAsync();
    }

    private static async Task CallPrimeService(Channel channel)
    {
        var client = new PrimesService.PrimesServiceClient(channel);
        var request = new NumberRequest() { Argument = new Argument() { Number = 120 } };
        var response = client.Primes(request);
        while (await response.ResponseStream.MoveNext())
        {
            Console.WriteLine(response.ResponseStream.Current.Result);
            await Task.Delay(400);
        }
    }

    private static async Task CallComputeAverageService(Channel channel)
    {
        var client = new ComputeAverageService.ComputeAverageServiceClient(channel);
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var stream = client.ComputeAverage();
        foreach (var number in numbers)
        {
            await stream.RequestStream.WriteAsync(new RequestNumber() { Argument = new ComputeAverage.Argument() { Number = number } });
        }
        await stream.RequestStream.CompleteAsync();
        var response = await stream.ResponseAsync;
        Console.WriteLine(response.Result);
    }

    private static async Task CallSqrtService(Channel channel)
    {
        var client = new SqrtService.SqrtServiceClient(channel);
        int num = 4;
        try
        {
            var response = client.Sqrt(new SqrtRequest() { Number = num });
            Console.WriteLine(response.Sqrt);
        }
        catch (RpcException e)
        {
            Console.WriteLine("Error: " + e);
        }
    }
    
    private static async Task CallGreetingService(Channel channel)
    {
        var client = new GreetingService.GreetingServiceClient(channel);
        try
        {
            var response=client.GreetWithDeadLine(new GreetRequest(){Name = "kemo"}, 
                                                  deadline:DateTime.UtcNow.AddSeconds(2));
            Console.WriteLine(response.Result);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e.Status.Detail);
            throw;
        }
    }
}
