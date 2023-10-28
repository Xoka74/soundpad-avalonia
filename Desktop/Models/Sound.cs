using System.IO;

namespace SoundpadDesktop.Models;

public class Sound
{
    public FileInfo FileInfo { get; }
    public string Name => FileInfo.Name;
    public string AbsolutePath => FileInfo.FullName;

    public Sound(string path)
    {
        FileInfo = new FileInfo(path);
    }
}