window.textToSpeech =
{
    speak: function (text)
    {
        var utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = false ? "en-US" : "ru-RU";
        speechSynthesis.speak(utterance);
    },
    
    trycancel: function() 
    {
        if (speechSynthesis.speaking || speechSynthesis.pending)
        {
            speechSynthesis.cancel();
        }
    }
};