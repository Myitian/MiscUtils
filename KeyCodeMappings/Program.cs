using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace KeyCodeMappings;

unsafe partial class Program
{
    static void Main()
    {
        HashSet<int> other = [.. Enumerable.Range(0, byte.MaxValue + 1)];
        int width = Enum.GetNames<KeyboardUsage>().Select(it => it.Length).Max();
        Console.ResetColor();
        Console.WriteLine("HID mappings:");
        Console.WriteLine($"{"HID Usage".PadRight(width + 4)}: ScanCode   , VirtKey, ConsoleKey");
        for (int it = 0; it < 256; it++)
        {
            KeyboardUsage usage = (KeyboardUsage)it;
            uint vsc = HidUsageToI8042ScanCode(usage);
            uint vk = MapVirtualKeyExW(vsc, MapType.VSC_TO_VK_EX);
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
            uint vsc = MapVirtualKeyExW(vk, MapType.VK_TO_VSC_EX);
            PrintKey(vsc, vk);
        }

        static void PrintKey(uint vsc, uint vk)
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
    }

    public static uint HidUsageToI8042ScanCode(
        KeyboardUsage changedUsage,
        KeyboardDirection keyAction = KeyboardDirection.Make,
        KeyboardModifierState modifierState = KeyboardModifierState.None)
    {
        uint result = 0;
        uint status = HidP_TranslateUsagesToI8042ScanCodes(
            in changedUsage,
            1,
            keyAction,
            ref modifierState,
            static (ref context, ref readonly newScanCodes, length) =>
            {
                ReadOnlySpan<byte> received = MemoryMarshal.CreateReadOnlySpan(in newScanCodes, length);
                if (received is [0xE0 or 0xE1, byte, ..])
                    context = BinaryPrimitives.ReadUInt16BigEndian(received);
                else if (received.Length > 0)
                    context = received[0];
                return true;
            },
            ref result);
        return result;
    }

    [LibraryImport("User32.dll")]
    private static partial uint MapVirtualKeyExW(uint uCode, MapType uMapType, nint dwhkl = 0);

    [LibraryImport("Hid.dll")]
    private static partial uint HidP_TranslateUsagesToI8042ScanCodes(
        ref readonly KeyboardUsage ChangedUsageList,
        int UsageListLength,
        KeyboardDirection KeyAction,
        ref KeyboardModifierState ModifierState,
        PHIDP_INSERT_SCANCODES InsertCodesProcedure,
        ref uint InsertCodesContext);

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate bool PHIDP_INSERT_SCANCODES(ref uint Context, ref readonly byte NewScanCodes, int Length);
}