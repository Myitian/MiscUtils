using System.Runtime.InteropServices;

namespace SimpleWinUSB;

[StructLayout(LayoutKind.Sequential)]
public struct WinUSBSetupPacket
{
    public RequestType RequestType;
    public byte Request;
    public ushort Value;
    public ushort Index;
    public ushort Length;
}