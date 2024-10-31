using Microsoft.JSInterop;

public class TextToSpeechService
{
    IJSRuntime Runtime;

    public TextToSpeechService(IJSRuntime JS)
    {
        Runtime = JS;
    }

    public void Preload()
    {   
        _ = Runtime.InvokeVoidAsync("textToSpeech.preload");
    }

    public void Speak(string text)
    {   
        _ = Runtime.InvokeVoidAsync("textToSpeech.speak", text);
    }
}