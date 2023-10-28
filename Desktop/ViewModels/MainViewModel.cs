using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using Soundpad;
using SoundpadDesktop.Models;
using SoundpadDesktop.Repositories;
using Splat;

namespace Desktop.ViewModels;

public class MainViewModel : ViewModelBase
{
    private SoundPlayer soundPlayer;
    private SoundRepository repository;

    public ObservableCollection<Sound> Sounds { get; }

    private Sound selectedSound;

    public Sound? SelectedSound
    {
        get => selectedSound;
        set => this.RaiseAndSetIfChanged(ref selectedSound, value);
    }

    private TimeSpan currentTimeSpan;

    public TimeSpan CurrentTimeSpan
    {
        get => currentTimeSpan;
        set => this.RaiseAndSetIfChanged(ref currentTimeSpan, value);
    }

    private TimeSpan _totalTimeSpan;

    public TimeSpan TotalTimeSpan
    {
        get => _totalTimeSpan;
        set => this.RaiseAndSetIfChanged(ref _totalTimeSpan, value);
    }

    private bool isPlaying = false;

    public bool IsPlaying
    {
        get => isPlaying;
        set => this.RaiseAndSetIfChanged(ref isPlaying, value);
    }

    public ReactiveCommand<Sound, Unit> SelectSoundCommand { get; }
    public ReactiveCommand<Sound, Unit> DeleteSoundCommand { get; }

    public MainViewModel()
    {
        soundPlayer = Locator.Current.GetService<SoundPlayer>();
        repository = Locator.Current.GetService<SoundRepository>();
        Sounds = new ObservableCollection<Sound>(repository.GetAll());

        SetVolume(soundPlayer.Volume);

        repository.SoundSaved += (soundRepository, args) => Sounds.Add(args.Sound);

        repository.SoundDeleted += (soundRepository, args) =>
        {
            if (args.Sound == SelectedSound)
            {
                soundPlayer.UnselectSound();
                SelectedSound = null;
            }

            Sounds.Remove(args.Sound);
        };

        SelectSoundCommand = ReactiveCommand.Create<Sound>(RunSelectSound);
        DeleteSoundCommand = ReactiveCommand.Create<Sound>((sound) => repository.Delete(sound));
    }

    public void AddSound(string path)
    {
        repository.Add(new FileInfo(path));
    }

    public void SetVolume(double value)
    {
        soundPlayer.Volume = (float)value;
    }

    private void RunSelectSound(Sound sound)
    {
        SelectedSound = sound;
        soundPlayer.SelectSound(sound);
        Play();
    }

    public void Play()
    {
        if (SelectedSound == null) return;
        IsPlaying = true;
        TotalTimeSpan = soundPlayer.TotalTime;
        soundPlayer.Play();
        Task.Run(() =>
        {
            while (IsPlaying)
            {
                CurrentTimeSpan = soundPlayer.Position;
            }

            IsPlaying = false;
        });
    }

    public void PauseSound()
    {
        soundPlayer.Pause();
        IsPlaying = false;
    }

    public void SetPosition(TimeSpan time)
    {
        soundPlayer.Position = time;
    }
}