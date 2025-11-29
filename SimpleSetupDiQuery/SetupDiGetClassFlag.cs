namespace SimpleSetupDiQuery;

[Flags]
public enum SetupDiGetClassFlag
{
    DEFAULT = 0x00000001,
    PRESENT = 0x00000002,
    ALLCLASSES = 0x00000004,
    PROFILE = 0x00000008,
    DEVICEINTERFACE = 0x00000010,
}