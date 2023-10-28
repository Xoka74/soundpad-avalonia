using Soundpad;
using SoundpadDesktop.Repositories;

var player = new SoundPlayer();
var repository = new SoundRepository();
var sound = repository.GetAll().First();
Console.WriteLine(sound.Name);
player.SelectSound(sound);
player.Play();
while (true)
{
    Console.WriteLine(player.Position);
}