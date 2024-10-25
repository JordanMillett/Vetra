using Microsoft.AspNetCore.Components;

namespace Vetra
{
    public class PracticePageBase : ComponentBase
    {
        [Inject]
        public LessonService Lessons { get; set; } = default!;

        [Inject]
        public ProfileService Profile { get; set; } = default!;

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
            foreach (string Term in Logic.Results.Keys)
            {
                float Change = Profile.ChangeProgress(Term, Logic.Results[Term]);
                if (Change > 0)
                {
                    StatusRight = $"{Term} +{Change:F2}%";
                    StatusWrong = "";
                }
                else
                {
                    StatusRight = "";
                    StatusWrong = $"{Term} {Change:F2}%";
                }
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
                Logic.Included.Add(Lessons.GetNextTerm());
                Lessons.Fill(ref Logic);
            }
            else
            {
                Logic.Included.Add(Lessons.GetNextTerm(LessonKey));
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
