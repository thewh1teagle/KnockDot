using SharpPcap;
using System;
using System.Linq;
using System.Net.NetworkInformation;


namespace KnockDot
{
    class DeviceManager
    {
        public static ICaptureDevice GetDefaultDevice()
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


        public static ICaptureDevice GetInterfaceByIndex(int index)
        {
            var devices = CaptureDeviceList.Instance;
            if (devices.Count < 1)
            {
                Console.WriteLine("No devices were found on this machine");
                return null;
            }
            return devices[index];

        }
        public static void PrintDevices()
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
                Console.WriteLine("{0}. {1}", i + 1, dev.Description);
                i++;
            }
        }
    }
}
