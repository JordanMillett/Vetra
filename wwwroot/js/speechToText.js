window.speechToText =
{
    recognition: null,

    startRecognition: function (dotnetHelper) {
        if (this.recognition) return; // Prevent multiple recognitions

        this.recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
        this.recognition.lang = "ru-RU"; // Set language
        this.recognition.interimResults = false;
        this.recognition.maxAlternatives = 1;

        this.recognition.onresult = function (event) {
            const transcript = event.results[0][0].transcript;
            dotnetHelper.invokeMethodAsync("OnSpeechRecognized", transcript);
        };

        this.recognition.onerror = function (event) {
            console.error("Speech recognition error:", event.error);
            dotnetHelper.invokeMethodAsync("OnSpeechError", event.error);
        };

        this.recognition.onend = function () {
            dotnetHelper.invokeMethodAsync("OnSpeechEnd");
            window.speechToText.recognition = null;
        };

        this.recognition.start();
    },
    stopRecognition: function ()
    {
        if (this.recognition) {
            this.recognition.stop();
            this.recognition = null;
        }
    }
};
