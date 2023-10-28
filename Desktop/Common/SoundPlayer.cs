using System;
using System.ComponentModel.DataAnnotations;
using ExCSS;
using NAudio.Utils;
using NAudio.Wave;
using ReactiveUI;
using SoundpadDesktop.Models;

namespace Soundpad;

public class SoundPlayer
{
    private WaveOutEvent waveOutEvent;
    private Mp3FileReader selectedSound;

    public TimeSpan Position
    {
        get => waveOutEvent.GetPositionTimeSpan();
        set => selectedSound.Position = value.Seconds;
    }

    public TimeSpan TotalTime => selectedSound == null ? TimeSpan.Zero : selectedSound.TotalTime;

    public float Volume
    {
        get => waveOutEvent.Volume;
        set => waveOutEvent.Volume = value;
    }

    public SoundPlayer()
    {
        waveOutEvent = new WaveOutEvent();
        waveOutEvent.DeviceNumber = 0;
        waveOutEvent.Volume = 0;
    }


    public void SelectSound(Sound sound)
    {
        selectedSound = new Mp3FileReader(sound.FileInfo.FullName);
        waveOutEvent.Stop();
        waveOutEvent.Init(selectedSound);
    }

    public void UnselectSound()
    {
        waveOutEvent.Stop();
        selectedSound?.Dispose();
    }

    public void Play() => waveOutEvent.Play();

    public void Pause() => waveOutEvent.Pause();

    public void Restart()
    {
        waveOutEvent.Stop();
        Play();
    }
}