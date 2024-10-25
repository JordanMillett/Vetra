using Microsoft.AspNetCore.Components;

namespace Vetra
{
    public class LessonTypeBase : ComponentBase
    {
        [Inject]
        public LessonService Lessons { get; set; } = default!;

        [Inject]
        public ProfileService Profile { get; set; } = default!;
        
        [Inject]
        public TextToSpeechService TTS { get; set; } = default!;

        [Parameter, EditorRequired]
        public LessonLogic Logic { get; set; } = default!;

        [Parameter]
        public EventCallback<LessonLogic> LogicChanged { get; set; }

        [Parameter, EditorRequired]
        public Action? OnFinish { get; set; }
        
        public Random Rand = new Random();
        
        public bool NotReady()
        {
            if (Logic == null)
                return true;
            if (Logic.Included.Count == 0)
                return true;
            if (Logic.Filler.Count == 0)
                return true;

            return false;
        }
        
        public void PlayOneShot()
        {
            _ = TTS.Speak(Logic.Included[0].RU);
        }
    }
}
