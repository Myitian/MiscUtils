using System.Text;
using static SimpleWin32Cursor.Cursor;

namespace FormTest;

public partial class MainForm : Form
{
    Dictionary<ushort, nint> cursors = [];

    public MainForm()
    {
        InitializeComponent();
        NUD_Value.Maximum = TB_Value.Maximum = ushort.MaxValue;
        L_ValueR.Text = ushort.MaxValue.ToString();
        SetValue((ushort)NUD_Value.Value);
    }

    private void SetValue(ushort value)
    {
        TB_Value.Value = value;
        NUD_Value.Value = value;
        nint cursur = LoadCursor(value);
        L_Pointer.Text = cursur.ToString();
        if (cursur != 0)
            Cursor = new(cursur);
    }

    private void TB_Value_Scroll(object sender, EventArgs e)
    {
        SetValue((ushort)TB_Value.Value);
    }

    private void NUD_Value_ValueChanged(object sender, EventArgs e)
    {
        SetValue((ushort)NUD_Value.Value);
    }

    private void B_S_Click(object sender, EventArgs e)
    {
        Task.Run(() =>
        {
            Thread.Sleep(2000);
            for (int i = 0; i <= ushort.MaxValue; i++)
            {
                ushort u = (ushort)i;
                nint p = LoadCursor(u);
                if (p != 0)
                {
                    cursors[u] = p;
                    Invoke(() => SetValue(u));
                    Thread.Sleep(100);
                }
            }
        });
    }

    private void B_P_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new();
        int i = 0;
        foreach ((ushort key, nint value) in cursors)
        {
            sb.Append($"{key:D5}: 0x{value:X8}");
            sb.Append(++i % 2 == 0 ? Environment.NewLine : " \t");
        }
        MessageBox.Show(sb.ToString());
    }
}
