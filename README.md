# MiscUtils
My miscellaneous .NET utilities.

- KeyCodeMappings
- SimpleAdbSocket
- SimpleSetupDiQuery
- SimpleWin32HardLink
- SimpleWin32Input
- SimpleWinUSB

> [!IMPORTANT]
>
> In all methods named `CreateUnsafe`, all `ReadOnlySpan<char>` should be terminated by a NULL character (`'\0'`)!
>
> Alternatively, you can use a non-unsafe version (which will copy and potentially create a new object), or pass a `string`, which is already null-terminated in the .NET runtime's implementation.