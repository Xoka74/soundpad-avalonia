using System;
using SoundpadDesktop.Models;

namespace SoundpadDesktop.Repositories;

public class SoundEventArgs : EventArgs
{
    public Sound Sound { get; }

    public SoundEventArgs(Sound sound)
    {
        Sound = sound;
    }
}

public class SoundRenamedEventArgs : EventArgs
{
    public Sound OldSound { get; }
    public Sound NewSound { get; }
    
    public SoundRenamedEventArgs(Sound oldSound, Sound newSound)
    {
        OldSound = oldSound;
        NewSound = newSound;
    }
}