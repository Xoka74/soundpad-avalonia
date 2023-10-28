using System;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Svg.Skia;
using Avalonia.Threading;
using Desktop.ViewModels;
using DynamicData;
using ReactiveUI;
using SkiaSharp;
using SoundpadDesktop.Models;
using Splat;
using Svg.Skia;


namespace Desktop.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel viewModel;
    public MainWindow()
    {
        InitializeComponent();
        viewModel = Locator.Current.GetService<MainViewModel>();
        DataContext = viewModel;

        this.WhenAnyValue(x => x.VolumeSlider.Value)
            .Subscribe(x =>viewModel.SetVolume(x));
        this.WhenAnyValue(x => x.viewModel.IsPlaying)
            .Subscribe(x => Dispatcher.UIThread.Post(() => { ChangePlayButtonState(x); }));
        
        this.WhenAnyValue(x => x.viewModel.TotalTimeSpan)
            .Subscribe(x => Dispatcher.UIThread.Post(() =>
            {
                CurrentTimeSlider.Maximum = viewModel.TotalTimeSpan.TotalSeconds;
            }));
        this.WhenAnyValue(x => x.viewModel.CurrentTimeSpan)
            .Subscribe(x => Dispatcher.UIThread.Post(() =>
            {
                PlayingTime.Text = string.Format("{0:mm\\:ss}", x);
                CurrentTimeSlider.Value = x.TotalSeconds;
            }));
    }

    private async void OnAddSoundButtonClick(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Add sound",
            AllowMultiple = false
        });

        foreach (var file in files)
            viewModel.AddSound(file.Path.AbsolutePath);
    }

    private void ChangePlayButtonState(bool isPlaying)
    {
        var svg = new SvgSource();

        if (isPlaying)
        {
            svg.Load("C:\\Users\\Xoka74\\Desktop\\MyProjects\\Soundpad\\Desktop\\Assets\\pause-ic.svg");
        }
        else
        {
            svg.Load("C:\\Users\\Xoka74\\Desktop\\MyProjects\\Soundpad\\Desktop\\Assets\\play-ic.svg");
        }

        PlaySvgImage.Source = new SvgImage { Source = svg };
    }

    private void OnPlayButtonClick(object? sender, RoutedEventArgs e)
    {
        if (viewModel.IsPlaying)
            viewModel.PauseSound();
        else
            viewModel.Play();

        ChangePlayButtonState(viewModel.IsPlaying);
    }
}