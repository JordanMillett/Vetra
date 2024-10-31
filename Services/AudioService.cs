using Microsoft.JSInterop;

public class AudioService
{
    IJSRuntime Runtime;

    public AudioService(IJSRuntime JS)
    {
        Runtime = JS;
    }

    public async Task Play(string url)
    {
        await Runtime.InvokeVoidAsync("audio.play", url, 0.75f);
    }
}