using System.Runtime.InteropServices;

namespace SimpleWinUSB;

[StructLayout(LayoutKind.Sequential)]
public readonly struct WinUSBPipeInformation
{
    public readonly USBDPipeType PipeType;
    public readonly byte PipeId;
    public readonly ushort MaximumPacketSize;
    public readonly byte Interval;
}