using System.Runtime.InteropServices;

namespace SimpleSetupDiQuery;

[StructLayout(LayoutKind.Sequential)]
public readonly struct DevicePropertyKey(Guid fmtid, uint pid)
{
    // DEVPROPKEY
    public readonly Guid fmtid = fmtid;
    public readonly uint pid = pid;

    public static readonly Guid Guid_Name = new(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac);
    public static readonly DevicePropertyKey Name = new(Guid_Name, 10);
    public static readonly Guid Guid_Device = new(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0);
    public static readonly DevicePropertyKey Device_DeviceDesc = new(Guid_Device, 2);
    public static readonly DevicePropertyKey Device_HardwareIds = new(Guid_Device, 3);
    public static readonly DevicePropertyKey Device_CompatibleIds = new(Guid_Device, 4);
    public static readonly DevicePropertyKey Device_Service = new(Guid_Device, 6);
    public static readonly DevicePropertyKey Device_Class = new(Guid_Device, 9);
    public static readonly DevicePropertyKey Device_ClassGuid = new(Guid_Device, 10);
    public static readonly DevicePropertyKey Device_Driver = new(Guid_Device, 11);
    public static readonly DevicePropertyKey Device_ConfigFlags = new(Guid_Device, 12);
    public static readonly DevicePropertyKey Device_Manufacturer = new(Guid_Device, 13);
    public static readonly DevicePropertyKey Device_FriendlyName = new(Guid_Device, 14);
    public static readonly DevicePropertyKey Device_LocationInfo = new(Guid_Device, 15);
    public static readonly DevicePropertyKey Device_PDOName = new(Guid_Device, 16);
    public static readonly DevicePropertyKey Device_Capabilities = new(Guid_Device, 17);
    public static readonly DevicePropertyKey Device_UINumber = new(Guid_Device, 18);
    public static readonly DevicePropertyKey Device_UpperFilters = new(Guid_Device, 19);
    public static readonly DevicePropertyKey Device_LowerFilters = new(Guid_Device, 20);
    public static readonly DevicePropertyKey Device_BusTypeGuid = new(Guid_Device, 21);
    public static readonly DevicePropertyKey Device_LegacyBusType = new(Guid_Device, 22);
    public static readonly DevicePropertyKey Device_BusNumber = new(Guid_Device, 23);
    public static readonly DevicePropertyKey Device_EnumeratorName = new(Guid_Device, 24);
    public static readonly DevicePropertyKey Device_Security = new(Guid_Device, 25);
    public static readonly DevicePropertyKey Device_SecuritySDS = new(Guid_Device, 26);
    public static readonly DevicePropertyKey Device_DevType = new(Guid_Device, 27);
    public static readonly DevicePropertyKey Device_Exclusive = new(Guid_Device, 28);
    public static readonly DevicePropertyKey Device_Characteristics = new(Guid_Device, 29);
    public static readonly DevicePropertyKey Device_Address = new(Guid_Device, 30);
    public static readonly DevicePropertyKey Device_UINumberDescFormat = new(Guid_Device, 31);
    public static readonly DevicePropertyKey Device_PowerData = new(Guid_Device, 32);
    public static readonly DevicePropertyKey Device_RemovalPolicy = new(Guid_Device, 33);
    public static readonly DevicePropertyKey Device_RemovalPolicyDefault = new(Guid_Device, 34);
    public static readonly DevicePropertyKey Device_RemovalPolicyOverride = new(Guid_Device, 35);
    public static readonly DevicePropertyKey Device_InstallState = new(Guid_Device, 36);
    public static readonly DevicePropertyKey Device_LocationPaths = new(Guid_Device, 37);
    public static readonly DevicePropertyKey Device_BaseContainerId = new(Guid_Device, 38);
}