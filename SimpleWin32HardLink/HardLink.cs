using System.Buffers;
using System.Runtime.InteropServices;

namespace SimpleWin32HardLink;

public static partial class HardLink
{
    public static bool CreateUnsafe(scoped ReadOnlySpan<char> fileName, scoped ReadOnlySpan<char> existingFileName)
    {
        return CreateHardLinkW(fileName, existingFileName, 0);
    }
    public static bool Create(string fileName, string existingFileName)
    {
        return CreateUnsafe(fileName, existingFileName);
    }
    public static bool Create(scoped ReadOnlySpan<char> fileName, string existingFileName)
    {
        int length = fileName.Length + 1;
        if (length <= 256)
        {
            Span<char> span = stackalloc char[length];
            fileName.CopyTo(span);
            span[fileName.Length] = '\0';
            return CreateUnsafe(span, existingFileName);
        }
        char[] buffer = ArrayPool<char>.Shared.Rent(length);
        try
        {
            Span<char> span = buffer;
            fileName.CopyTo(span);
            span[fileName.Length] = '\0';
            return CreateUnsafe(span, existingFileName);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
    public static bool Create(string fileName, scoped ReadOnlySpan<char> existingFileName)
    {
        int length = existingFileName.Length + 1;
        if (length <= 256)
        {
            Span<char> span = stackalloc char[length];
            existingFileName.CopyTo(span);
            span[existingFileName.Length] = '\0';
            return CreateUnsafe(fileName, span);
        }
        char[] buffer = ArrayPool<char>.Shared.Rent(length);
        try
        {
            Span<char> span = buffer;
            existingFileName.CopyTo(span);
            span[existingFileName.Length] = '\0';
            return CreateUnsafe(fileName, span);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
    public static bool Create(scoped ReadOnlySpan<char> fileName, scoped ReadOnlySpan<char> existingFileName)
    {
        int length = fileName.Length + existingFileName.Length + 2;
        if (length <= 256)
        {
            Span<char> span1 = stackalloc char[length];
            fileName.CopyTo(span1);
            span1[fileName.Length] = '\0';
            Span<char> span2 = span1[(fileName.Length + 1)..];
            existingFileName.CopyTo(span2);
            span2[existingFileName.Length] = '\0';
            return CreateUnsafe(span1, span2);
        }
        char[] buffer = ArrayPool<char>.Shared.Rent(length);
        try
        {
            Span<char> span1 = buffer;
            fileName.CopyTo(span1);
            span1[fileName.Length] = '\0';
            Span<char> span2 = span1[(fileName.Length + 1)..];
            existingFileName.CopyTo(span2);
            span2[existingFileName.Length] = '\0';
            return CreateUnsafe(span1, span2);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }

    [LibraryImport("kernel32", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateHardLinkW(
        ReadOnlySpan<char> lpFileName,
        ReadOnlySpan<char> lpExistingFileName,
        nint lpSecurityAttributes /* Reserved, always 0 */);
}
class NullTerminatedAttribute(string baseMethod, params string[] args) : Attribute;