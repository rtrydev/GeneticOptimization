using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Runner.Models;

namespace Runner.Commands;

public class Compile : ICommand
{
    private string[] CostFunctions { get; set; }

    public Compile(string[] costFunctions)
    {
        CostFunctions = costFunctions;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var log = (ConsoleLogModel) parameter;
        var compiler = new CodeCompiler.CodeCompiler();
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = false;
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var selectedFiles = await fileDialog.ShowAsync(app?.MainWindow);
        if (selectedFiles is null) return;
        var file = selectedFiles.First();

        var moduleName = "";

        if (OperatingSystem.IsWindows())
        {
            moduleName = file.Split("\\").Last().Split(".").First();
        }
        else
        {
            moduleName = file.Split("/").Last().Split(".").First();
        }
        
        var code = File.ReadAllText(file);
        var result = compiler.CompileCSharp(code, moduleName);
        

        if (!result.IsSuccessful)
        {
            foreach (var message in result.Messages)
            {
                log.AppendLog(message);
            }
            File.Delete($"Modules/{moduleName}.dll");
        }
        else
        {
            log.AppendLog($"Compilation of the {moduleName}.dll module has been finished successfully. Restart the application to make new modules visible.");
        }
        
    }

    public event EventHandler? CanExecuteChanged;
}