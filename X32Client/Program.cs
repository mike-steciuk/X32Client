using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X32.OSC;

namespace X32
{
    class Program
    {
        static void Main(string[] args)
        {
            var x32Client = new X32Client("192.168.0.109");
            x32Client.ReceiveTimeout = 100;
            //x32Client.ReceiveTimeout = 1;

            //TestGate(x32Client);

            for (int i = 0; i < 32; i++)
            {
                var chan = x32Client.GetChannel(i + 1);
                Console.WriteLine($"{i}: - {chan.Name} | {chan.Color.GetName()} | {chan.Source.GetName()} | {chan.Delay.On} | {chan.Delay.Time.ToString()}");
            }

            //var chan1 = x32Client.GetChannel(1);
            //chan1.Name = "Hello";
            //chan1.Color = X32Color.InvertedRed;
            //chan1.Icon = 3;
            //chan1.Source = X32Source.In02;

            Console.WriteLine("Press Any Key");
            Console.ReadKey();

        }

        static void TestGate(X32Client client)
        {
            var channel = client.GetChannel(1);
            var gate = channel.Gate;
            Console.WriteLine($"{gate.On} | {gate.GateMode.GetName()} |  {gate.Threshold} |  {gate.Range} |  {gate.Attack} |  {gate.Hold} |  {gate.Release} |  {gate.KeySource.GetName()} |  {gate.FilterOn} |  {gate.FilterType.GetName()} |  {gate.FilterFreq}");
        }
        static void PrintOsc(OscMessage message)
        {
            Console.WriteLine(message.Address);
            message.Arguments.ForEach(arg => Console.WriteLine(arg.ToString()));
        }
    }
}
