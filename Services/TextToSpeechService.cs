using Microsoft.JSInterop;

public class TextToSpeechService
{
    IJSRuntime Runtime;

    public TextToSpeechService(IJSRuntime JS)
    {
        Runtime = JS;
    }

    public async Task Speak(string text)
    {
        await TryCancel();
        await Runtime.InvokeVoidAsync("textToSpeech.speak", text);
    }
    
    public async Task TryCancel()
    {
        await Runtime.InvokeVoidAsync("textToSpeech.trycancel");
    }
}