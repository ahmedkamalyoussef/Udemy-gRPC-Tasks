using ComputeAverage;
using Greeting;
using Grpc.Core;
using Grpc.Reflection;
using Grpc.Reflection.V1Alpha;
using Prime;
using Server.Services;
using Sqrt;

internal class Program
{
    const int port = 50052;
    private static void Main(string[] args)
    {
        Grpc.Core.Server s = null;
        try
        {
            var reflectionServiceImp = new ReflectionServiceImpl(GreetingService.Descriptor, PrimesService.Descriptor,
                ServerReflection.Descriptor);
            s = new Grpc.Core.Server()
            {
                Services =
                {
                    PrimesService.BindService(new PrimeServiceImp()),
                    ComputeAverageService.BindService(new ComputeAverageServiceImp()),
                    SqrtService.BindService(new SqrtServiceImp()),
                    GreetingService.BindService(new GreetingServiceImp()),
                    ServerReflection.BindService(reflectionServiceImp)
                },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            s.Start();
            Console.WriteLine("lesting on " + port);
            Console.ReadKey();
        }
        catch (IOException e)
        {
            Console.WriteLine($"{e.Message}");
            throw;
        }
        finally
        {
            s?.ShutdownAsync().Wait();
        }
    }
}