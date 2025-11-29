using System.Runtime.InteropServices;

namespace SimpleWinUSB;

public partial struct WinUSB : IDisposable
{
    private nint _hDevice;
    private nint _hWinUSB;
    public readonly bool IsInvalid => _hDevice is -1 || _hWinUSB is -1;
    public WinUSB(scoped ReadOnlySpan<char> devicePath)
    {
        _hDevice = CreateFileW(devicePath);
        if (_hDevice is -1 || !WinUsb_Initialize(_hDevice, out _hWinUSB))
        {
            _hWinUSB = -1;
            Dispose();
        }
    }

    [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint CreateFileW(
        scoped ReadOnlySpan<char> lpFileName,
        DesiredAccess dwDesiredAccess = DesiredAccess.GENERIC_READ | DesiredAccess.GENERIC_WRITE,
        FileShare dwShareMode = FileShare.ReadWrite,
        nint lpSecurityAttributes = 0,
        FileMode dwCreationDisposition = FileMode.Open,
        FileAttribute dwFlagsAndAttributes = FileAttribute.OVERLAPPED,
        nint hTemplateFile = 0);
    [LibraryImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseHandle(
        nint hObject);
    [LibraryImport("winusb", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WinUsb_Initialize(
        nint DeviceHandle,
        out nint InterfaceHandle);
    [LibraryImport("winusb", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WinUsb_Free(
        nint InterfaceHandle);
    [LibraryImport("winusb", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WinUsb_ControlTransfer(
        nint InterfaceHandle,
        in WinUSBSetupPacket SetupPacket,
        scoped ReadOnlySpan<byte> Buffer,
        int BufferLength,
        out int LengthTransferred,
        nint Overlapped = 0);
    [LibraryImport("winusb", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WinUsb_QueryInterfaceSettings(
        nint InterfaceHandle,
        byte AlternateInterfaceNumber,
        out USBInterfaceDescriptor UsbAltInterfaceDescriptor);
    [LibraryImport("winusb", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool WinUsb_QueryPipe(
        nint InterfaceHandle,
        byte AlternateInterfaceNumber,
        byte PipeIndex,
        out WinUSBPipeInformation PipeInformation);

    public readonly bool ControlTransfer(in WinUSBSetupPacket setupPacket, scoped ReadOnlySpan<byte> buffer)
    {
        bool result = WinUsb_ControlTransfer(_hWinUSB, in setupPacket, buffer, buffer.Length, out int transferred);
        return result && buffer.Length == transferred;
    }
    public readonly bool ControlTransfer(in WinUSBSetupPacket setupPacket, scoped Span<byte> buffer, out int transferred)
    {
        return WinUsb_ControlTransfer(_hWinUSB, in setupPacket, buffer, buffer.Length, out transferred);
    }
    public readonly bool QueryInterfaceSettings(byte alternateInterfaceNumber, out USBInterfaceDescriptor descriptor)
    {
        return WinUsb_QueryInterfaceSettings(_hWinUSB, alternateInterfaceNumber, out descriptor);
    }
    public readonly bool QueryPipe(byte alternateInterfaceNumber, byte pipeIndex, out WinUSBPipeInformation pipeInfo)
    {
        return WinUsb_QueryPipe(_hWinUSB, alternateInterfaceNumber, pipeIndex, out pipeInfo);
    }
    public void Dispose()
    {
        if (_hWinUSB is not -1)
        {
            WinUsb_Free(_hWinUSB);
            _hWinUSB = -1;
        }
        if (_hDevice is not -1)
        {
            CloseHandle(_hDevice);
            _hDevice = -1;
        }
    }

    [Flags]
    enum DesiredAccess : uint
    {
        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000
    }
    [Flags]
    enum FileAttribute : uint
    {
        NORMAL = 0x00000080,
        OVERLAPPED = 0x40000000,
        NO_BUFFERING = 0x20000000,
    }
}