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
using System.Net.NetworkInformation;
using System.Collections;
using System.Diagnostics;

namespace portKnockingServer
{
    public class Program
    {
        class Options
        {
            [Option('p', "port", Required = true, HelpText = "Ports sequence")]
            public IEnumerable<string> Ports { get; set; }

            [Option('e', "exec",
            Required = true,
            HelpText = "command to execute after the knock")]
            public string exec { get; set; }

            [Option("protocol",
              Default = "tcp",
              HelpText = "tcp or udp")]
            public string protocol { get; set; }
        }
        static PortKnocker portKnocker;
        static string execCommand;


        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
                {
                    IEnumerable<string> ports = o.Ports;
                    string protocol = o.protocol;
                    execCommand = o.exec;
                    printDevices();
                    ICaptureDevice interfaceResult = getDefaultDevice();
                    string filterString = GenerateFilterString(ports, protocol);
                    Console.WriteLine(filterString);
                    List<int> intPorts = ports.ToList().Select(int.Parse).ToList();
                    portKnocker = new PortKnocker(intPorts, 60);
                    sniff(interfaceResult, filterString);
                });            
        }

        
        private static string GenerateFilterString(IEnumerable ports, string protocol)
        { // generate Berkley Packet Filters
            string result = "";
            foreach (string port in ports)
            {
                result += $"{protocol} port {port} or ";
            }
            return result.Substring(0, result.Length - 4);
        }
        
        private static ICaptureDevice getDefaultDevice()
        {
            // find default interface then convert to Icapturedevice
            var nic = NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(i => i.NetworkInterfaceType != NetworkInterfaceType.Loopback && i.NetworkInterfaceType != NetworkInterfaceType.Tunnel);
                 var name = nic.Name;

            string nicName = nic.Id.ToLower();

            var devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return null;
            }
            ICaptureDevice device = devices.SingleOrDefault(i => i.Name.ToLower().Contains(nicName));

            // If no devices were found print an error

            return device;
        }


        private static void sniff(ICaptureDevice device, string filter)
        {

            // using var device = 1//devices[i];

            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

            //Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            device.Open(DeviceModes.Promiscuous, readTimeoutMilliseconds);

            // tcpdump filter to capture only TCP/IP packets
            device.Filter = filter;

            Console.WriteLine();
            Console.WriteLine
                ("-- The following tcpdump filter will be applied: \"{0}\"",
                filter);
            Console.WriteLine
                ("-- Listening on {0}, hit 'Ctrl-C' to exit...",
                device.Description);

            // Start capture packets
            device.Capture();

        }

        private static void printDevices()
        {
            // Retrieve the device list
            var devices = CaptureDeviceList.Instance;

            // If no devices were found print an error
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return;
            }
            Console.WriteLine("The following devices are available on this machine:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();
            int i = 0;

            // Scan the list printing every entry
            foreach (var dev in devices)
            {
                Console.WriteLine("{0}) {1}", i, dev.Description);
                i++;
            }

        }
        

        private static void device_OnPacketArrival(object sender, PacketCapture e)
        {
            // var time = e.Header.Timeval.Date;
            // var len = e.Data.Length;
            var destinationPort = e.GetPacket().GetPacket().Extract<TcpPacket>().DestinationPort;
            Console.WriteLine(destinationPort);
            if (portKnocker.check(destinationPort))
            {
                Console.WriteLine("unlocked!!");
                RunCommand(execCommand, false);
            }


            // Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
            //    time.Hour, time.Minute, time.Second, time.Millisecond, len);
            
        }

        public static string RunCommand(string arguments, bool readOutput)
        {
            var output = string.Empty;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    Verb = "runas",
                    FileName = "cmd.exe",
                    Arguments = "/C " + arguments,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = false
                };

                var proc = Process.Start(startInfo);

                if (readOutput)
                {
                    output = proc.StandardOutput.ReadToEnd();
                }

                proc.WaitForExit(60000);

                return output;
            }
            catch (Exception)
            {
                return output;
            }
        }


    }
}
