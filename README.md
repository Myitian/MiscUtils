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
> All `ReadOnlySpan<char>` for APIs that expect LPCWSTR should be terminated by a NULL character (`'\0'`)!
>
> Or you can pass a `string`, which should actually be terminated by a NULL character in .NET runtime.