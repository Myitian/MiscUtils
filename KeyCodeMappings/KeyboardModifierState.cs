namespace KeyCodeMappings;

[Flags]
public enum KeyboardModifierState
{
    None,
    LeftControl = 0b0000_0000_0001,
    LeftShift = 0b0000_0000_0010,
    LeftAlt = 0b0000_0000_0100,
    LeftGUI = 0b0000_0000_1000,
    RightControl = 0b0000_0001_0000,
    RightShift = 0b0000_0010_0000,
    RightAlt = 0b0000_0100_0000,
    RigthGUI = 0b0000_1000_0000,
    CapsLock = 0b0001_0000_0000,
    ScollLock = 0b0010_0000_0000,
    NumLock = 0b0100_0000_0000
}