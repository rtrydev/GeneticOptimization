using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace Runner.Commands;

public class OpenLogFolder : ICommand
{
    private string _path;

    public OpenLogFolder(string path)
    {
        _path = path;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (OperatingSystem.IsWindows())
        {
            Process.Start("explorer.exe",Path.GetFullPath(_path));
        }
        if (OperatingSystem.IsMacOS())
        {
            Process.Start("open",Path.GetFullPath(_path));
        }
        if (OperatingSystem.IsLinux())
        {
            Process.Start("xdg-open",Path.GetFullPath(_path));
        }
    }

    public event EventHandler? CanExecuteChanged;
}