using SnowaTec.Test.RealTime;

internal class Program
{
    static void Main(string[] args)
    {
        var server = new MqttNetServer();

        server.Start();

        Console.WriteLine("Press any key to exit...");

        Console.ReadLine();

        server.Stop();
    }
}



