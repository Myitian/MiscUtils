using System.Runtime.InteropServices;

namespace SimpleWinUSB;

[StructLayout(LayoutKind.Sequential)]
public readonly struct USBInterfaceDescriptor
{
    public readonly byte Length;
    public readonly byte DescriptorType;
    public readonly byte InterfaceNumber;
    public readonly byte AlternateSetting;
    public readonly byte NumEndpoints;
    public readonly byte InterfaceClass;
    public readonly byte InterfaceSubClass;
    public readonly byte InterfaceProtocol;
    public readonly byte Interface;
}