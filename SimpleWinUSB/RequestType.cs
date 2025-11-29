namespace SimpleWinUSB;

[Flags]
public enum RequestType : byte
{
    HostToDevice = 0b00000000,
    DeviceToHost = 0b10000000,

    Type_Standard = HostToDevice,
    Type_Class = 0b00100000,
    Type_Vendor = 0b01000000,

    Recipient_Device = HostToDevice,
    Recipient_Interface = 0b00000001,
    Recipient_Endpoint = 0b00000010,
    Recipient_Other = 0b00000011,
}