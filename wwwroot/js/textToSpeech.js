window.textToSpeech =
{
    speak: function (text) {
        speechSynthesis.cancel();
        
        var utterance = new SpeechSynthesisUtterance(text);
        utterance.volume = 0.25;
        utterance.lang = false ? "en-US" : "ru-RU";
        speechSynthesis.speak(utterance);
    },
    preload: function() {
        speechSynthesis.getVoices();
        
        var utterance = new SpeechSynthesisUtterance("Preload");
        utterance.volume = 0;
        speechSynthesis.speak(utterance);
    }
};