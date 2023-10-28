using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Desktop.ViewModels;
using Desktop.Views;
using ReactiveUI;
using Soundpad;
using SoundpadDesktop.Repositories;
using Splat;

namespace Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Locator.CurrentMutable.Register<SoundPlayer>(() => new SoundPlayer());
        Locator.CurrentMutable.Register<SoundRepository>(() => new SoundRepository());
        Locator.CurrentMutable.Register<MainViewModel>(() => new MainViewModel());
        Locator.CurrentMutable.Register<Preferences>(() => new Preferences());
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        
        base.OnFrameworkInitializationCompleted();
    }
}