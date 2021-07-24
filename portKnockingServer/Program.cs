using System;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;
using PacketDotNet.Tcp;
using PacketDotNet.Utils;
using System.Collections.Generic;
using CommandLine;
using System.Net.NetworkInformation;
using System.Linq;
using System.Collections;
using System.Diagnostics;

namespace portKnockingServer
{
    public class Program
    {
       
        static PortKnocker portKnocker;
        static string execCommand;


        public static void Main(string[] args)
        {
            
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts =>
                {
                    if (opts.listInterfaces)
                    {

                        DeviceManager.printDevices();
                        System.Environment.Exit(0);
                    }
                    IEnumerable<string> ports = opts.Ports;
                    string protocol = opts.protocol;
                    ICaptureDevice interfaceResult = DeviceManager.getDefaultDevice();
                    List<int> intPorts = ports.ToList().Select(int.Parse).ToList();
                    portKnocker = new PortKnocker(intPorts, 60);
                    Sniffer sniffer = new Sniffer(portKnocker, opts.exec);
                    string filterString = Sniffer.GenerateFilterString(ports, protocol);
                    Sniffer.sniff(interfaceResult, filterString);
                });            
        }
    }
}
