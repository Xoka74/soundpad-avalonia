using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using SoundpadDesktop.Models;

namespace SoundpadDesktop.Repositories;

public class SoundRepository
{
    private DirectoryInfo SoundsDirectory;

    public event Action<SoundRepository, SoundEventArgs> SoundSaved;
    public event Action<SoundRepository, SoundEventArgs> SoundDeleted;
    public event Action<SoundRepository, SoundRenamedEventArgs> SoundRenamed;

    public SoundRepository()
    {
        if (!Directory.Exists("./Sounds"))
            Directory.CreateDirectory("./Sounds");

        SoundsDirectory = new DirectoryInfo("Sounds");
    }

    public IEnumerable<Sound> GetAll()
    {
        return SoundsDirectory
            .EnumerateFiles()
            .Select(x => new Sound(x.FullName));
    }

    public void Delete(Sound sound)
    {
        SoundDeleted?.Invoke(this, new SoundEventArgs(sound));
        sound.FileInfo.Delete();
    }

    public void Rename(Sound sound, string name)
    {
        var newSound = new Sound(Path.Combine(sound.FileInfo.Directory.FullName, name));
        sound.FileInfo.MoveTo(newSound.AbsolutePath);
        SoundRenamed?.Invoke(this, new SoundRenamedEventArgs(sound, newSound));
    }

    public void Add(FileInfo fileInfo)
    {
        var newPath = Path.Combine(SoundsDirectory.FullName, fileInfo.Name);
        File.Copy(fileInfo.FullName, newPath);
        SoundSaved?.Invoke(this, new SoundEventArgs(new Sound(newPath)));
    }
}