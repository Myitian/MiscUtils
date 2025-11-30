using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SimpleWin32Input;

[StructLayout(LayoutKind.Explicit)]
public partial struct Input
{
    [FieldOffset(0)]
    public InputType Type;
    [FieldOffset(8)]
    public MouseInput MI;
    [FieldOffset(8)]
    public KeyboardInput KI;
    [FieldOffset(8)]
    public HardwareInput HI;
    public static readonly int Size = Unsafe.SizeOf<Input>();

    public Input(MouseInput mi)
    {
        Type = InputType.Mouse;
        MI = mi;
    }
    public Input(KeyboardInput ki)
    {
        Type = InputType.Keyboard;
        KI = ki;
    }
    public Input(HardwareInput hi)
    {
        Type = InputType.Hardware;
        HI = hi;
    }

    [LibraryImport("user32", SetLastError = true)]
    private static partial int SendInput(int cInputs, scoped ReadOnlySpan<Input> pInputs, int cbSize);

    public static int Send(params scoped ReadOnlySpan<Input> inputs)
    {
        return SendInput(inputs.Length, inputs, Size);
    }

    public static implicit operator Input(MouseInput mi) => new(mi);
    public static implicit operator Input(KeyboardInput ki) => new(ki);
    public static implicit operator Input(HardwareInput hi) => new(hi);
    public static explicit operator MouseInput(Input input) => input.MI;
    public static explicit operator KeyboardInput(Input input) => input.KI;
    public static explicit operator HardwareInput(Input input) => input.HI;
}