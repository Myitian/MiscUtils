namespace SimpleAdbSocket;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public readonly struct Adb
{
    public readonly string ExecutablePath;
    public static readonly Adb Default = new(null);

    public Adb(string? adbPath)
    {
        if (File.Exists(adbPath))
        {
            ExecutablePath = adbPath;
            return;
        }
        string? path = Environment.GetEnvironmentVariable("PATH");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            adbPath ??= "adb.exe";
            foreach (ReadOnlySpan<char> p in new WindowsEnvironmentPathEnumerator(path))
            {
                string ps = Path.Combine(p.ToString(), adbPath);
                if (File.Exists(ps))
                {
                    ExecutablePath = ps;
                    return;
                }
            }
        }
        else
        {
            adbPath ??= "adb";
            foreach (ReadOnlySpan<char> p in new SpanSplitEnumerator(path, ';'))
            {
                string ps = Path.Combine(p.ToString(), adbPath);
                if (File.Exists(ps))
                {
                    ExecutablePath = ps;
                    return;
                }
            }
        }
        ExecutablePath = adbPath;
    }

    public Process? Execute(string arguments)
    {
        return Process.Start(new ProcessStartInfo
        {
            FileName = ExecutablePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        });
    }
    public Process? Execute(params ReadOnlySpan<string> arguments)
    {
        ProcessStartInfo info = new()
        {
            FileName = ExecutablePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        foreach (string arg in arguments)
            info.ArgumentList.Add(arg);
        return Process.Start(info);
    }
    public Process? Execute(IEnumerable<string> arguments)
    {
        ProcessStartInfo info = new()
        {
            FileName = ExecutablePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        foreach (string arg in arguments)
            info.ArgumentList.Add(arg);
        return Process.Start(info);
    }
}