using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Media;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class HistoryViewModel : ViewModelBase
{

    [Reactive] public ObservableCollection<ResultFileModel> Files { get; set; }
    private DateTime _lastOpen = DateTime.Now;

    private ResultFileModel _selection;
    public ResultFileModel Selection
    {
        get => _selection;
        set
        {
            if (_lastOpen.AddMilliseconds(100) > DateTime.Now) return;
            _lastOpen = DateTime.Now;
            if (!value.TileColor.Equals(Brushes.Transparent))
            {
                var index = Files.IndexOf(value);

                var newValue = new ResultFileModel { FileName = value.FileName + "/result.json", TileColor = Brushes.Transparent };
                Files[index] = newValue;
            }


            OpenRunInfo.Execute(value);

        }
    }

    public ICommand OpenRunInfo { get; set; }
    
    public HistoryViewModel()
    {
        Directory.CreateDirectory("Results");
        LoadFiles();
        OpenRunInfo = new OpenRunInfo();
    }

    public void LoadFiles()
    {
        DirectoryInfo info = new DirectoryInfo("Results");
        var files = info.GetDirectories().OrderByDescending(p => p.CreationTime).ToArray();
        var list= files.Select(x => new ResultFileModel { FileName = x.Name, TileColor = Brushes.Transparent }).ToList();
        Files = new ObservableCollection<ResultFileModel>(list);
    }

    public void RefreshFiles()
    {
        DirectoryInfo info = new DirectoryInfo("Results");
        var files = info.GetDirectories().OrderByDescending(p => p.CreationTime).ToArray();
        var fileModels = files.Select(x => new ResultFileModel { FileName = x.Name, TileColor= Brushes.Transparent }).ToArray();
        fileModels[0].TileColor = Brushes.Green;
        Files.Insert(0, fileModels[0]);
    }
}