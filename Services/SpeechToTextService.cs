using System.Text;
using Microsoft.JSInterop;

public class SpeechToTextService
{
    public string Text = "";
    public bool isRecognizing = false;
    
    public event Action? OnRecognitionUpdated;
    public event Action? OnSpeechOver;
    
    IJSRuntime Runtime;
    AudioService Audio;

    public SpeechToTextService(IJSRuntime JS, AudioService A)
    {
        Runtime = JS;
        Audio = A;
    }

    [JSInvokable]
    public void OnSpeechRecognized(string transcript)
    {
        Text = transcript;
        OnRecognitionUpdated?.Invoke();
    }

    [JSInvokable]
    public void OnSpeechError(string error)
    {
        Console.WriteLine($"Speech Recognition Error: {error}");
    }
    
    [JSInvokable]
    public void OnSpeechEnd()
    {
        Audio.Play(AudioService.Sounds.MicOff);
        isRecognizing = false;
        OnRecognitionUpdated?.Invoke();
        OnSpeechOver?.Invoke();
    }
    
    public async Task StartRecognition()
    {
        if (isRecognizing) return;
        
        var dotNetReference = DotNetObjectReference.Create(this);
        await Runtime.InvokeVoidAsync("speechToText.startRecognition", dotNetReference);
        Audio.Play(AudioService.Sounds.MicOn);
        isRecognizing = true;
        OnRecognitionUpdated?.Invoke();
    }
    
    public async Task StopRecognition()
    {
        if (!isRecognizing) return;

        await Runtime.InvokeVoidAsync("speechToText.stopRecognition");
        isRecognizing = false;
        OnRecognitionUpdated?.Invoke();
    }
}