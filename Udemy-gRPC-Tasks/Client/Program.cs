using Grpc.Core;
using Prime;

internal class Program
{
    const string target = "127.0.0.1:50052";
    private async static Task Main(string[] args)
    {
        Channel channel =new Grpc.Core.Channel(target, ChannelCredentials.Insecure);
        await channel.ConnectAsync().ContinueWith((task) =>
        {
            if (task.Status == TaskStatus.RanToCompletion)
                Console.WriteLine("connected");
        });
        
        var client=new PrimesService.PrimesServiceClient(channel);
        var request =new NumberRequest(){Argument = new Argument(){Number=120 }};
        var response=client.Primes(request);
        while (await response.ResponseStream.MoveNext())
        {
            Console.WriteLine(response.ResponseStream.Current.Result);
            await Task.Delay(4000);
        }
        channel.ShutdownAsync().Wait();
        {
            
        }
    }
}