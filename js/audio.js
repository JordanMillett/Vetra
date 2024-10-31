window.audioPlayer =
{
    play: function (url, volume)
    {
        var audio = new Audio(url);
        audio.volume = volume;
        audio.play();
    }
};