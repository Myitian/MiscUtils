using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace KeyCodeMappings;

public partial class Mappings
{
    public static uint MapVirtualKey(uint code, MapType mapType, nint dwhkl = 0)
    {
        return MapVirtualKeyExW(code, mapType, dwhkl);
    }

    public static unsafe uint HidUsageToI8042ScanCode(
        KeyboardUsage usage,
        KeyboardDirection keyAction = KeyboardDirection.Make,
        KeyboardModifierState modifierState = KeyboardModifierState.None)
    {
        uint result = 0;
        uint status = HidP_TranslateUsagesToI8042ScanCodes(
            in usage,
            1,
            keyAction,
            ref modifierState,
            &HidPInsertScanCodes,
            ref result);
        return result;

        [UnmanagedCallersOnly]
        static unsafe byte HidPInsertScanCodes(uint* context, void* newScanCodes, int length)
        {
            ReadOnlySpan<byte> received = new(newScanCodes, length);
            if (received is [0xE0 or 0xE1, byte, ..])
                *context = BinaryPrimitives.ReadUInt16BigEndian(received);
            else if (received.Length > 0)
                *context = received[0];
            return 1;
        }
    }

    [LibraryImport("user32")]
    private static partial uint MapVirtualKeyExW(uint uCode, MapType uMapType, nint dwhkl = 0);

    [LibraryImport("hid")]
    private static unsafe partial uint HidP_TranslateUsagesToI8042ScanCodes(
        in KeyboardUsage ChangedUsageList,
        int UsageListLength,
        KeyboardDirection KeyAction,
        ref KeyboardModifierState ModifierState,
        delegate* unmanaged<uint*, void*, int, byte> InsertCodesProcedure,
        ref uint InsertCodesContext);
}