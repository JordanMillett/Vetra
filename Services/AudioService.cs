using Microsoft.JSInterop;
using Vetra;

public class AudioService
{
    public enum Sounds
    {
        Click,
        Correct,
        Incorrect
    }
    
    IJSRuntime Runtime;
    SettingsService Settings;

    public AudioService(IJSRuntime JS, SettingsService S)
    {
        Runtime = JS;
        Settings = S;
    }
    
    public void Click()
    {
        Play(Sounds.Click);
    }
    
    public void Play(Sounds Sound)
    {   
        switch (Sound)
        {
            case Sounds.Click : PlayCustom("click.mp3"); break;
            case Sounds.Correct : PlayCustom("correct.mp3"); break;
            case Sounds.Incorrect : PlayCustom("wrong.mp3"); break;
        }
    }

    public void PlayCustom(string url)
    {
        if (Settings.Data.EnableSoundEFfects)
        {
            _ = Runtime.InvokeVoidAsync("audio.play", "audio/" + url);
        }
    }
}