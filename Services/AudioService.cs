using Microsoft.JSInterop;
using Vetra;

public class AudioService
{
    public enum Sounds
    {
        Click,
        Correct,
        Incorrect,
        MicOn,
        MicOff
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
            case Sounds.MicOn : PlayCustom("mic-on.mp3"); break;
            case Sounds.MicOff : PlayCustom("mic-off.mp3"); break;
        }
    }

    public void PlayCustom(string url)
    {
        if (Settings.Data.EnableSoundEffects)
        {
            _ = Runtime.InvokeVoidAsync("audio.play", "audio/" + url);
        }
    }
}