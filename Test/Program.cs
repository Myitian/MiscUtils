using SimpleAdbSocket;
using SimpleSetupDiQuery;
using SimpleWin32Input;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Test;

public class Program
{
    public static readonly Guid WINUSB_DEVICE = new("dee824ef-729b-4a0e-9c14-b7117d33a817");
    public static readonly Guid USB_DEVICE = new("a5dcbf10-6530-11d2-901f-00c04fb951ed");

    public static async Task Main()
    {

        _ = Task.Run(() =>
        {
            Thread.Sleep(100);
            Input.Send([
                new KeyboardInput(VirtualKey.LShift, false),
                new KeyboardInput(VirtualKey.H, false),
                new KeyboardInput(VirtualKey.H, true),
                new KeyboardInput(VirtualKey.LShift, true)]);
            Input.Send([
                new KeyboardInput(VirtualKey.E, false),
                new KeyboardInput(VirtualKey.E, true),
                new KeyboardInput(VirtualKey.L, false),
                new KeyboardInput(VirtualKey.L, true),
                new KeyboardInput(VirtualKey.L, false),
                new KeyboardInput(VirtualKey.L, true)]);
            new KeyboardInput(VirtualKey.O, false).SendEvent();
            new KeyboardInput(VirtualKey.O, true).SendEvent();
            new KeyboardInput(VirtualKey.OemComma, false).SendEvent();
            new KeyboardInput(VirtualKey.OemComma, true).SendEvent();
            Input.Send([
                new KeyboardInput(VirtualKey.Space, false),
                new KeyboardInput(VirtualKey.Space, true),
                new KeyboardInput(VirtualKey.W, false),
                new KeyboardInput(VirtualKey.W, true),
                new KeyboardInput(VirtualKey.O, false),
                new KeyboardInput(VirtualKey.O, true),
                new KeyboardInput(VirtualKey.R, false),
                new KeyboardInput(VirtualKey.R, true),
                new KeyboardInput(VirtualKey.L, false),
                new KeyboardInput(VirtualKey.L, true),
                new KeyboardInput(VirtualKey.D, false),
                new KeyboardInput(VirtualKey.D, true),
                new KeyboardInput(VirtualKey.LShift, false),
                new KeyboardInput(VirtualKey.D1, false),
                new KeyboardInput(VirtualKey.D1, true),
                new KeyboardInput(VirtualKey.LShift, true)]);
            new KeyboardInput(VirtualKey.Space, false).SendEvent();
            new KeyboardInput(VirtualKey.Space, true).SendEvent();
            Input.Send([
                new KeyboardInput('你'),
                new KeyboardInput('好'),
                new KeyboardInput('世'),
                new KeyboardInput('界')]);
        });
        Console.ReadLine();

        if (!ListDevices(in USB_DEVICE))
        {
            int error = Marshal.GetLastPInvokeError();
            Console.WriteLine($"Failed to list devices: (0x{error:X8}){error}");
        }

        //using AdbTcpTunnel? tunnel = await AdbTcpTunnel.CreateAsync(1234, 5678);
        //if (tunnel is not null)
        //{
        //    using TcpClient? client = tunnel.Client;
        //}
    }

    public static bool ListDevices(in Guid guid)
    {
        DeviceInterfaceEnumerable deviceInfoSet = new(in guid, SetupDiGetClassFlag.PRESENT | SetupDiGetClassFlag.DEVICEINTERFACE);
        if (deviceInfoSet.IsInvalid)
            return false;
        foreach (DeviceInterfaceData data in deviceInfoSet)
        {
            Console.WriteLine();
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
        }
        return true;
    }
}