using System.Collections;
using System.Runtime.InteropServices;

namespace SimpleSetupDiQuery;

public partial struct DeviceInterfaceEnumerable(nint handle, in Guid classGuid)
    : IDisposable, IEnumerable<DeviceInterfaceData>
{
    public nint RawHandle = handle;
    public readonly Guid ClassGuid = classGuid;
    public readonly bool IsInvalid => RawHandle is -1;
    public DeviceInterfaceEnumerable(in Guid classGuid, SetupDiGetClassFlag flags, string? enumerator = null, nint parent = 0)
        : this(SetupDiGetClassDevsW(in classGuid, enumerator, parent, flags), in classGuid)
    {
    }
    public void Dispose()
    {
        if (!IsInvalid)
        {
            SetupDiDestroyDeviceInfoList(RawHandle);
            RawHandle = -1;
        }
    }
    public readonly Enumerator GetEnumerator() => new(in this);
    readonly IEnumerator<DeviceInterfaceData> IEnumerable<DeviceInterfaceData>.GetEnumerator() => GetEnumerator();
    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [LibraryImport("setupapi", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    private static partial nint SetupDiGetClassDevsW(
        in Guid ClassGuid,
        string? Enumerator,
        nint hwndParent,
        SetupDiGetClassFlag Flags);
    [LibraryImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetupDiDestroyDeviceInfoList(
        nint DeviceInfoSet);

    [LibraryImport("setupapi", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetupDiEnumDeviceInterfaces(
        nint DeviceInfoSet,
        nint DeviceInfoData,
        in Guid InterfaceClassGuid,
        uint MemberIndex,
        ref DeviceInterfaceData DeviceInterfaceData);

    public partial struct Enumerator(in DeviceInterfaceEnumerable handle)
        : IEnumerator<DeviceInterfaceData>
    {
        private DeviceInterfaceData data = new(handle.RawHandle);
        public readonly nint RawHandle = handle.RawHandle;
        public readonly Guid ClassGuid = handle.ClassGuid;
        private uint memberIndex = 0;
        public readonly DeviceInterfaceData Current => data;
        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            bool result = SetupDiEnumDeviceInterfaces(RawHandle, 0, in ClassGuid, memberIndex, ref data);
            if (result)
                memberIndex++;
            return result;
        }
        public void Reset()
        {
            memberIndex = 0;
        }
        public readonly void Dispose()
        {
        }
    }
}