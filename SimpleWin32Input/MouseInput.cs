using System.Runtime.InteropServices;

namespace SimpleWin32Input;

[StructLayout(LayoutKind.Sequential)]
public partial struct MouseInput
{
    public int Dx;
    public int Dy;
    public int MouseData;
    public MouseEventFlag Flags;
    public uint Time;
    public nint ExtraInfo;

    [LibraryImport("user32", EntryPoint = "mouse_event")]
    private static partial void MouseEvent(MouseEventFlag dwFlags, int dx, int dy, int dwData, nint dwExtraInfo);

    /// <summary>Use <c>mouse_event</c>. Not recommended.</summary>
    public readonly void SendEvent()
    {
        MouseEvent(Flags, Dx, Dy, MouseData, ExtraInfo);
    }
}