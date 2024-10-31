using Microsoft.AspNetCore.Components;

namespace Vetra
{
    public class PracticePageBase : ComponentBase
    {
        [Inject]
        public LessonService Lessons { get; set; } = default!;

        [Inject]
        public ProfileService Profile { get; set; } = default!;
        
        [Inject]
        public AudioService Audio { get; set; } = default!;
        
        public LessonLogic Logic = default!;
        public string LessonKey = "";
        public float TotalFinished = 0f;
        public int LessonInstance = 0;
        public string StatusRight = "";
        public string StatusWrong = "";
        public bool Endless = false;

        protected override async Task OnInitializedAsync()
        {
            while (!Profile.Initialized)
            {
                await Task.Delay(250);
            }

            if(Profile.Data.LearnedVocab.Count > 0)
                NextLesson();
        }

        public void LessonFinished()
        {
            float Change = Profile.ChangeProgress(Logic.Term.RU, Logic.Passed);
            if (Change > 0)
            {
                if (Lessons.LessonType != 0)
                    Audio.Play(AudioService.Sounds.Correct);
                else
                    Audio.Play(AudioService.Sounds.Click);
                    
                StatusRight = $"{Logic.Term.RU} +{Change:F2}%";
                StatusWrong = "";
            }
            else
            {
                Audio.Play(AudioService.Sounds.Incorrect);
                StatusRight = "";
                StatusWrong = $"{Logic.Term.RU} {Change:F2}%";
            }
            

            _ = Profile.SaveProfile();
            NextLesson();
        }

        public void NextLesson()
        {
            RecalculateProgressBar();
            Logic = new LessonLogic();

            if (LessonKey == "")
            {
                Logic.Term = Lessons.GetNextTerm();
                Lessons.Fill(ref Logic);
            }
            else
            {
                Logic.Term = Lessons.GetNextTerm(LessonKey);
                Lessons.Fill(ref Logic, LessonKey);
            }

            LessonInstance++;
        }

        public void ToggleEndless()
        {
            Endless = !Endless;
        }

        public virtual void RecalculateProgressBar()
        {
            int Num = LessonKey == "" ? Profile.Data.LearnedVocab.Count : Lessons.LessonHeaders[LessonKey].Vocab.Count;
            TotalFinished = 0f;

            if (LessonKey == "")
            {
                for (int i = 0; i < Profile.Data.LearnedVocab.Count; i++)
                {
                    TotalFinished += Profile.Data.VocabProgression[i];
                }
            }else
            {
                foreach (string V in Lessons.LessonHeaders[LessonKey].Vocab)
                {
                    int index = Profile.Data.LearnedVocab.IndexOf(V);
                    if (index != -1)
                    {
                        TotalFinished += Profile.Data.VocabProgression[index];
                    }
                }
            }

            TotalFinished /= (float)Num;
            StateHasChanged();
        }
    }
}
