using PacketDotNet;
using SharpPcap;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portKnockingServer
{
    class Sniffer
    {

        static PortKnocker portKnocker;
        static string execCommand;

        public Sniffer(PortKnocker portKnocker, string execCommand)
        {
            portKnocker = portKnocker;
            execCommand = execCommand;
        }

        public static string GenerateFilterString(IEnumerable ports, string protocol)
        { // generate Berkley Packet Filters
            string result = "";
            foreach (string port in ports)
            {
                result += $"{protocol} port {port} or ";
            }
            return result.Substring(0, result.Length - 4);
        }




        public static void sniff(ICaptureDevice device, string filter)
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

        private static void device_OnPacketArrival(object sender, PacketCapture e)
        {
            // var time = e.Header.Timeval.Date;
            // var len = e.Data.Length;
            var destinationPort = e.GetPacket().GetPacket().Extract<TcpPacket>().DestinationPort;
            Console.WriteLine(destinationPort);
            if (portKnocker.check(destinationPort))
            {
                Commander.RunCommand(execCommand, false);
            }


            // Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
            //    time.Hour, time.Minute, time.Second, time.Millisecond, len);

        }

    }
}
