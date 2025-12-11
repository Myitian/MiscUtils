using KeyCodeMappings;
using SimpleSetupDiQuery;
using SimpleWin32HardLink;
using SimpleWin32Input;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Test;

public class Program
{
    public static readonly Guid WINUSB_DEVICE = new("dee824ef-729b-4a0e-9c14-b7117d33a817");
    public static readonly Guid USB_DEVICE = new("a5dcbf10-6530-11d2-901f-00c04fb951ed");

    public static async Task Main()
    {
        Console.WriteLine(Unsafe.SizeOf<Input>());
        while (Console.ReadLine() is string line and not "")
        {
            foreach (ReadOnlyMemory<char> file in HardLinkTargetEnumerator.Create(line.AsSpan().Trim().Trim('"')))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("* ");
                Console.ResetColor();
                Console.WriteLine(file.Span);
            }
        }
        //PrintMappings();

        _ = Task.Run(() =>
        {
            Thread.Sleep(1000);
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
            Span<Input> span = [
                new KeyboardInput('你', false),
                new KeyboardInput('好', false),
                new KeyboardInput('世', false),
                new KeyboardInput('界', false)];
            Input.Send(span);
            for (int i = 0; i < span.Length; i++)
            {
                span[i].U.KI.Flags |= KeyboardEventFlag.KeyUp;
            }
            Input.Send(span);
            new KeyboardInput(VirtualKey.Space, true).SendEvent();
        });
        Console.ReadLine();
        Input.Send(new KeyboardInput()
        {
            Scan = 0xE037,
            Flags = KeyboardEventFlag.ScanCode
        });
        Input.Send(new KeyboardInput()
        {
            Scan = 0xE037,
            Flags = KeyboardEventFlag.ScanCode | KeyboardEventFlag.KeyUp
        });

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


    public static void PrintMappings()
    {
        HashSet<int> other = [.. Enumerable.Range(0, byte.MaxValue + 1)];
        int width = Enum.GetNames<KeyboardUsage>().Select(it => it.Length).Max();
        Console.ResetColor();
        Console.WriteLine("HID mappings:");
        Console.WriteLine($"{"HID Usage".PadRight(width + 4)}: ScanCode   , VirtKey, ConsoleKey");
        for (int it = 0; it < 256; it++)
        {
            KeyboardUsage usage = (KeyboardUsage)it;
            uint vsc = Mappings.HidUsageToI8042ScanCode(usage);
            uint vk = Mappings.MapVirtualKey(vsc, MapType.VSC_TO_VK_EX);
            other.Remove((int)vk);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('(');
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{(ushort)usage:X2}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(')');
            Console.ForegroundColor = Enum.IsDefined(usage) ? ConsoleColor.Blue : ConsoleColor.DarkBlue;
            Console.Write(usage.ToString().PadRight(width));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(": ");
            PrintKey(vsc, vk);
        }
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Other virtual keys:");
        Console.WriteLine("ScanCode   , VirtKey, ConsoleKey");
        foreach (int it in other)
        {
            uint vk = (uint)it;
            uint vsc = Mappings.MapVirtualKey(vk, MapType.VK_TO_VSC_EX);
            PrintKey(vsc, vk);
        }


    }

    public static void PrintKey(uint vsc, uint vk)
    {
        if (vsc == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("-          ");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('(');
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{vsc:X4}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(')');
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{vsc,-5}");
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(", ");
        if (vk == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("-      ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(", ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('-');
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('(');
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{vk:X2}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(')');
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{vk,-3}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(", ");
            ConsoleKey ck = (ConsoleKey)vk;
            Console.ForegroundColor = Enum.IsDefined(ck) ? ConsoleColor.Yellow : ConsoleColor.DarkYellow;
            Console.Write(ck);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(';');
    }

    public static bool ListDevices(in Guid guid)
    {
        using DeviceInterfaceEnumerable deviceInfoSet = new(in guid, SetupDiGetClassFlag.PRESENT | SetupDiGetClassFlag.DEVICEINTERFACE);
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