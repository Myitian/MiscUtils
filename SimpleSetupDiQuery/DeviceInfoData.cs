using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimpleSetupDiQuery;

[StructLayout(LayoutKind.Sequential)]
public partial struct DeviceInfoData(nint deviceInfoSet)
{
    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVINFO_DATA
    {
        // dummy impl for calculating size
        public int cbSize;
        public Guid InterfaceClassGuid;
        public uint DevInst;
        public nint Reserved;
        public static readonly int Size = Unsafe.SizeOf<SP_DEVINFO_DATA>();
    }
    // SP_DEVINFO_DATA
    public readonly int cbSize = SP_DEVINFO_DATA.Size;
    public Guid InterfaceClassGuid;
    public uint DevInst;
    public nint Reserved;
    // Custom extended data
    private readonly nint DeviceInfoSet = deviceInfoSet;

    [LibraryImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetupDiGetDevicePropertyW(
        nint DeviceInfoSet,
        in DeviceInfoData DeviceInfoData,
        in DevicePropertyKey PropertyKey,
        out DevicePropertyType PropertyType,
        ref byte PropertyBuffer,
        uint PropertyBufferSize,
        out uint RequiredSize,
        int Flags = 0);

    public readonly bool TryGetProperty<T>(in DevicePropertyKey key, out T property) where T : struct
    {
        Unsafe.SkipInit(out property);
        return SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out _, ref Unsafe.As<T, byte>(ref property), (uint)Unsafe.SizeOf<T>(), out _);
    }
    public readonly bool TryGetProperty<T>(in DevicePropertyKey key, [NotNullWhen(true)] out T[]? property)
    {
        Unsafe.SkipInit(out property);
        uint elementSize = (uint)Unsafe.SizeOf<T>();
        SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out DevicePropertyType type, ref Unsafe.NullRef<byte>(), 0, out uint size);
        (uint count, uint rem) = Math.DivRem(size, elementSize);
        if (rem != 0 || !type.HasFlag(DevicePropertyType.ARRAY))
            return false;
        property = new T[count];
        return SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out _, ref Unsafe.As<T, byte>(ref property[0]), size, out _);
    }
    public readonly bool TryGetProperty(in DevicePropertyKey key, [NotNullWhen(true)] out string? property)
    {
        Unsafe.SkipInit(out property);
        SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out DevicePropertyType type, ref Unsafe.NullRef<byte>(), 0, out uint size);
        if (size > Array.MaxLength || type is not (DevicePropertyType.STRING
            or DevicePropertyType.STRING_INDIRECT
            or DevicePropertyType.SECURITY_DESCRIPTOR_STRING))
            return false;
        using IMemoryOwner<byte> mem = MemoryPool<byte>.Shared.Rent((int)size);
        Span<byte> buffer = mem.Memory.Span;
        if (!SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out _, ref buffer[0], size, out _))
            return false;
        ReadOnlySpan<char> chars = MemoryMarshal.Cast<byte, char>(buffer[..(int)size]);
        if (chars.IndexOf('\0') is >= 0 and int nullChar)
            chars = chars[..nullChar];
        property = new(chars);
        return true;
    }
    public readonly bool TryGetProperty(in DevicePropertyKey key, [NotNullWhen(true)] out List<string>? property)
    {
        Unsafe.SkipInit(out property);
        SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out DevicePropertyType type, ref Unsafe.NullRef<byte>(), 0, out uint size);
        if (size > Array.MaxLength || !type.HasFlag(DevicePropertyType.LIST)) // only string can have flag LIST
            return false;
        using IMemoryOwner<byte> mem = MemoryPool<byte>.Shared.Rent((int)size);
        Span<byte> buffer = mem.Memory.Span;
        if (!SetupDiGetDevicePropertyW(DeviceInfoSet, in this, in key, out _, ref buffer[0], size, out _))
            return false;
        property = [];
        ReadOnlySpan<char> chars = MemoryMarshal.Cast<byte, char>(buffer[..(int)size]);
        while (chars.IndexOf('\0') is > 0 and int nullChar)
        {
            property.Add(new(chars[..nullChar]));
            chars = chars[(nullChar + 1)..];
        }
        return true;
    }


    enum DevicePropertyType
    {
        EMPTY = 0x00000000,
        NULL = 0x00000001,
        SBYTE = 0x00000002,
        BYTE = 0x00000003,
        INT16 = 0x00000004,
        UINT16 = 0x00000005,
        INT32 = 0x00000006,
        UINT32 = 0x00000007,
        INT64 = 0x00000008,
        UINT64 = 0x00000009,
        FLOAT = 0x0000000A,
        DOUBLE = 0x0000000B,
        DECIMAL = 0x0000000C,
        GUID = 0x0000000D,
        CURRENCY = 0x0000000E,
        DATE = 0x0000000F,
        FILETIME = 0x00000010,
        BOOLEAN = 0x00000011,
        STRING = 0x00000012,
        SECURITY_DESCRIPTOR = 0x00000013,
        SECURITY_DESCRIPTOR_STRING = 0x00000014,
        DEVPROPKEY = 0x00000015,
        DEVPROPTYPE = 0x00000016,
        ERROR = 0x00000017,
        NTSTATUS = 0x00000018,
        STRING_INDIRECT = 0x00000019,
        ARRAY = 0x00001000,
        LIST = 0x00002000,
        STRING_LIST = STRING | LIST,
        BINARY = BYTE | ARRAY,
    }
}