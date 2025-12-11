using System.Buffers;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SimpleWin32HardLink;

public static partial class HardLink
{
    public static bool Create(string fileName, string existingFileName)
        => CreateHardLinkW(fileName, existingFileName, 0);

    [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateHardLinkW(
        string lpFileName,
        string lpExistingFileName,
        nint lpSecurityAttributes /* Reserved, always 0 */);
}