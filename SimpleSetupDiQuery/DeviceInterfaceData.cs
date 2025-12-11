using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimpleSetupDiQuery;


[StructLayout(LayoutKind.Sequential)]
public partial struct DeviceInterfaceData(nint deviceInfoSet)
{
    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVICE_INTERFACE_DATA
    {
        // dummy struct for calculating size
        public int cbSize;
        public Guid InterfaceClassGuid;
        public uint Flags;
        public nint Reserved;
        public static readonly int Size = Unsafe.SizeOf<SP_DEVICE_INTERFACE_DATA>();
    }
    // SP_DEVICE_INTERFACE_DATA
    public readonly int cbSize = SP_DEVICE_INTERFACE_DATA.Size;
    public Guid InterfaceClassGuid;
    public uint Flags;
    public nint Reserved;
    // Custom extended data
    private readonly nint DeviceInfoSet = deviceInfoSet;

    [LibraryImport("setupapi", EntryPoint = "SetupDiGetDeviceInterfaceDetailW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetupDiGetDeviceInterfaceDetailW(
        nint DeviceInfoSet,
        in DeviceInterfaceData DeviceInterfaceData,
        ref byte DeviceInterfaceDetailData,
        int DeviceInterfaceDetailDataSize,
        out int RequiredSize,
        ref DeviceInfoData DeviceInfoData);

    public readonly bool TryGetInfo([NotNullWhen(true)] out string? devicePath, out DeviceInfoData devInfo)
    {
        Unsafe.SkipInit(out devicePath);
        devInfo = new(DeviceInfoSet);
        SetupDiGetDeviceInterfaceDetailW(DeviceInfoSet, in this, ref Unsafe.NullRef<byte>(), 0, out int size, ref Unsafe.NullRef<DeviceInfoData>());
        if (size < 8 || size > Array.MaxLength)
            return false;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
        try
        {
            // [length][data ...]
            Unsafe.WriteUnaligned(ref buffer[0], 8);
            if (!SetupDiGetDeviceInterfaceDetailW(DeviceInfoSet, in this, ref buffer[0], size, out _, ref devInfo))
                return false;
            ReadOnlySpan<char> chars = MemoryMarshal.Cast<byte, char>(buffer.AsSpan(4, size - 4));
            int nullChar = chars.IndexOf('\0');
            if (nullChar >= 0)
                chars = chars[..nullChar];
            devicePath = new(chars);
            return true;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}