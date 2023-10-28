using System;
using SoundpadDesktop.Models;

namespace SoundpadDesktop;

public class EventBus
{
    public event Action<Sound> SoundDeleted;
    public void DeleteSound(Sound sound) => SoundDeleted?.Invoke(sound);
}