using Microsoft.JSInterop;

public class AudioService
{
    public enum Sounds
    {
        Click,
        Correct,
        Incorrect
    }
    
    IJSRuntime Runtime;

    public AudioService(IJSRuntime JS)
    {
        Runtime = JS;
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
        _ = Runtime.InvokeVoidAsync("audio.play", "audio/" + url);
    }
}