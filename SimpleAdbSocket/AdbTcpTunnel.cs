using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleAdbSocket;

public class AdbTcpTunnel : IDisposable, IAsyncDisposable
{
    private bool _disposed = false;
    public Adb Adb { get; }
    public int LocalPort { get; }
    public string? Serial { get; }
    public TcpClient? Client { get; }
    public bool IsReverse => Client is null;
    private AdbTcpTunnel(Adb adb, int localPort, string? serial, TcpClient? client)
    {
        Adb = adb;
        LocalPort = localPort;
        Serial = serial;
        Client = client;
    }

    public static async Task<AdbTcpTunnel?> CreateAsync(
        int localPort,
        int remotePort,
        bool reverse = false,
        string? serial = null,
        Adb? adb = null,
        bool noThrow = false)
    {
        Adb adb0 = adb ?? Adb.Default;
        string mode = reverse ? "reverse" : "forward";
        string message;
        int code;
        using (Process? createProc = adb0.Execute(serial is null ? (ReadOnlySpan<string>)
            [mode, $"tcp:{localPort}", $"tcp:{remotePort}"] :
            ["-s", serial, mode, $"tcp:{localPort}", $"tcp:{remotePort}"]))
        {
            if (createProc is null)
            {
                if (!noThrow)
                    throw new Exception("Unable to start process");
                return null;
            }
            message = (await createProc.StandardOutput.ReadToEndAsync()).Trim();
            await createProc.WaitForExitAsync();
            code = createProc.ExitCode;
        }
        if (code != 0 || !(string.IsNullOrEmpty(message) || int.TryParse(message, out localPort)))
        {
            if (!noThrow)
                throw new Exception($"Unexcepted exit ({code}): {message}") { HResult = code };
            return null;
        }
        TcpClient? client = null;
        try
        {
            if (!reverse)
            {
                client = new();
                await client.ConnectAsync(IPAddress.Loopback, localPort);
            }
            return new(adb0, localPort, serial, client);
        }
        catch
        {
            client?.Dispose();
            using (Process? removeProc = adb0.Execute(serial is null ? (ReadOnlySpan<string>)
                [mode, "--remove", $"tcp:{localPort}"] :
                ["-s", serial, mode, "--remove", $"tcp:{localPort}"]))
                if (removeProc is not null)
                    await removeProc.WaitForExitAsync();
            if (!noThrow)
                throw;
        }
        return null;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        Client?.Dispose();
        string mode = IsReverse ? "reverse" : "forward";
        using (Process? removeProc = Adb.Execute(Serial is null ? (ReadOnlySpan<string>)
            [mode, "--remove", $"tcp:{LocalPort}"] :
            ["-s", Serial, mode, "--remove", $"tcp:{LocalPort}"]))
            removeProc?.WaitForExit();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;
        Client?.Dispose();
        string mode = IsReverse ? "reverse" : "forward";
        using (Process? removeProc = Adb.Execute(Serial is null ? (ReadOnlySpan<string>)
            [mode, "--remove", $"tcp:{LocalPort}"] :
            ["-s", Serial, mode, "--remove", $"tcp:{LocalPort}"]))
            if (removeProc is not null)
                await removeProc.WaitForExitAsync();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}