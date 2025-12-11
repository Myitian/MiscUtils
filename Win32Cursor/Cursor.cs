using System.Runtime.InteropServices;

namespace SimpleWin32Cursor;

public static partial class Cursor
{
    [LibraryImport("user32", SetLastError = true)]
    private static partial nint LoadCursorW(nint hInstance, nint lpCursorName);
    [LibraryImport("user32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint LoadCursorW(nint hInstance, ReadOnlySpan<char> lpCursorName);
    [LibraryImport("user32", SetLastError = true)]
    public static partial nint SetCursor(nint hCursor);
    public static nint LoadCursor(ushort lpCursorName, nint hInstance = 0)
    {
        return LoadCursorW(hInstance, lpCursorName);
    }
    public static nint LoadCursor(ReadOnlySpan<char> lpCursorName, nint hInstance)
    {
        return LoadCursorW(hInstance, lpCursorName);
    }
}
