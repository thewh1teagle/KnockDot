using SharpPcap;
using System.Collections.Generic;
using CommandLine;
using System.Linq;


namespace portKnockingServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
                {
                    if (opts.ListInterfaces)
                    {
                        DeviceManager.PrintDevices();
                        System.Environment.Exit(0);
                    }
                    IEnumerable<string> ports = opts.Ports;
                    string protocol = opts.protocol;

                    ICaptureDevice interfaceResult;
                    if (opts.InterfaceIdx != null)
                    {
                        interfaceResult = DeviceManager.GetInterfaceByIndex(opts.InterfaceIdx - 1); // uman index..
                    } 
                    else
                    {
                        interfaceResult = DeviceManager.GetDefaultDevice();
                    }
                    List<int> intPorts = ports.ToList().Select(int.Parse).ToList();
                    PortKnocker portKnocker = new PortKnocker(intPorts, 60);
                    Sniffer sniffer = new Sniffer(portKnocker, opts.Exec);
                    string filterString = Sniffer.GenerateFilterString(ports, protocol);
                    sniffer.Sniff(interfaceResult, filterString);
                });            
        }
    }
}
