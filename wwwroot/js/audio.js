window.audio =
{
    play: function (url)
    {
        var audio = new Audio(url);
        audio.volume = 0.5;
        audio.play();
    }
};