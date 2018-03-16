using System;
using Shared.Core;
using System.Threading.Tasks;

class Program
{
    public static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    static async Task MainAsync(string[] args)
    {
        try
        {
            NServiceBusConfiguration.Setup();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        await Task.Delay(-1);
    }
}