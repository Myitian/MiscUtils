using System.Runtime.InteropServices;

namespace SimpleWin32Input;

[StructLayout(LayoutKind.Sequential)]
public partial struct KeyboardInput
{
    public VirtualKey Vk;
    public ushort Scan;
    public KeyboardEventFlag Flags;
    public uint Time;
    public nint ExtraInfo;

    public KeyboardInput(ConsoleKey ck, bool keyUp, bool extended = false) : this((VirtualKey)ck, keyUp, extended)
    {
    }
    public KeyboardInput(VirtualKey vk, bool keyUp, bool extended = false)
    {
        Vk = vk;
        Scan = 0;
        Flags = keyUp ? KeyboardEventFlag.KeyUp : 0
            | (extended ? KeyboardEventFlag.ExtendedKey : 0);
        Time = 0;
        ExtraInfo = 0;
    }
    public KeyboardInput(ushort scan, bool keyUp)
    {
        Vk = 0;
        Scan = (byte)scan;
        Flags = KeyboardEventFlag.ScanCode
            | (keyUp ? KeyboardEventFlag.KeyUp : 0)
            | ((scan & 0xFF00) == 0xE000 ? KeyboardEventFlag.ExtendedKey : 0);
        if (scan == 0xE11D)
        {
            Vk = VirtualKey.Pause;
            Scan = 0;
            Flags &= ~KeyboardEventFlag.ScanCode;
        }
        Time = 0;
        ExtraInfo = 0;
    }
    public KeyboardInput(char unicode, bool keyUp)
    {
        Vk = 0;
        Scan = unicode;
        Flags = KeyboardEventFlag.Unicode
            | (keyUp ? KeyboardEventFlag.KeyUp : 0);
        Time = 0;
        ExtraInfo = 0;
    }

    [LibraryImport("user32", EntryPoint = "keybd_event")]
    private static partial void KeyboardEvent(byte bVk, byte bScan, KeyboardEventFlag dwFlags, nint dwExtraInfo);

    /// <summary>
    /// Calls <c>keybd_event</c>. Recommended to use <see cref="Input.Send"/> instead of this.
    /// </summary>
    /// <remarks>
    /// When using <see cref="KeyboardEventFlag.Unicode" /> may not produce correct results
    /// because the value is truncated to <see cref="byte" />.
    /// </remarks>
    public readonly void SendEvent()
    {
        KeyboardEvent((byte)Vk, (byte)Scan, Flags, ExtraInfo);
    }
}