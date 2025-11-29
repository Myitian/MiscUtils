using SimpleAdbSocket;
using SimpleSetupDiQuery;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Test;

class Program
{
    public static readonly Guid WINUSB_DEVICE = new("dee824ef-729b-4a0e-9c14-b7117d33a817");
    public static readonly Guid USB_DEVICE = new("a5dcbf10-6530-11d2-901f-00c04fb951ed");

    static async Task Main()
    {
        if (!ListDevices(in USB_DEVICE))
        {
            int error = Marshal.GetLastPInvokeError();
            Console.WriteLine($"Failed to list devices: (0x{error:X8}){error}");
        }
        using AdbTcpTunnel? tunnel = await AdbTcpTunnel.CreateAsync(1234, 5678);
        if (tunnel is not null)
        {
            using TcpClient? client = tunnel.Client;
        }
    }

    static bool ListDevices(in Guid guid)
    {
        DeviceInterfaceEnumerable deviceInfoSet = new(in guid, SetupDiGetClassFlag.PRESENT | SetupDiGetClassFlag.DEVICEINTERFACE);
        if (deviceInfoSet.IsInvalid)
            return false;
        foreach (DeviceInterfaceData data in deviceInfoSet)
        {
            if (data.TryGetInfo(out string? devicePath, out DeviceInfoData info))
            {
                Console.WriteLine(devicePath);
                if (info.TryGetProperty(in DevicePropertyKey.Device_DeviceDesc, out string? value))
                {
                    Console.WriteLine("** DeviceDesc");
                    Console.WriteLine(value);
                }
                if (info.TryGetProperty(in DevicePropertyKey.Device_Service, out value))
                {
                    Console.WriteLine("** Service");
                    Console.WriteLine(value);
                }
                if (info.TryGetProperty(in DevicePropertyKey.Device_Class, out value))
                {
                    Console.WriteLine("** Class");
                    Console.WriteLine(value);
                }
                if (info.TryGetProperty(in DevicePropertyKey.Device_ClassGuid, out Guid guidValue))
                {
                    Console.WriteLine("** ClassGuid");
                    Console.WriteLine(guidValue);
                }
                if (info.TryGetProperty(in DevicePropertyKey.Device_HardwareIds, out List<string>? ids))
                {
                    Console.WriteLine("** HardwareIds");
                    ids.ForEach(Console.WriteLine);
                }
                if (info.TryGetProperty(in DevicePropertyKey.Device_CompatibleIds, out ids))
                {
                    Console.WriteLine("** CompatibleIds");
                    ids.ForEach(Console.WriteLine);
                }

            }
            else
            {
                Console.WriteLine(data.Reserved);
                Console.WriteLine("Unable to get details");
            }
            Console.WriteLine();
        }

        return true;
    }
}