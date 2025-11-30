using System.Runtime.InteropServices;

namespace SimpleWin32Input;

[StructLayout(LayoutKind.Sequential)]
public struct HardwareInput
{
    public uint Msg;
    public ushort ParamL;
    public ushort ParamH;
}