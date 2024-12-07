using ComputeAverage;
using Grpc.Core;
using Prime;
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

        var client = new PrimesService.PrimesServiceClient(channel);
        var request = new NumberRequest() { Argument = new Argument() { Number = 120 } };
        var response = client.Primes(request);
        while (await response.ResponseStream.MoveNext())
        {
            Console.WriteLine(response.ResponseStream.Current.Result);
            await Task.Delay(400);
        }

        Console.WriteLine("-------------------------------------------------------------------");
        var client2 = new ComputeAverageService.ComputeAverageServiceClient(channel);
        var numbers = new[] {1, 2, 3, 4, 5};
        var stream = client2.ComputeAverage();
        foreach (var number in numbers)
        {
            await stream.RequestStream.WriteAsync(new RequestNumber() { Argument = new ComputeAverage.Argument() { Number = number } });
        }
        await stream.RequestStream.CompleteAsync();
        var response2 = await stream.ResponseAsync;
        Console.WriteLine(response2.Result);
        
    channel.ShutdownAsync().Wait();
    }
}