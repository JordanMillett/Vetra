namespace Vetra
{
    public class ProfileService
    {   
        Blazored.LocalStorage.ILocalStorageService Local;

        public ProfileData Data = new ProfileData();

        public bool Initialized = false;

        public ProfileService(Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            Local = localStorage;
            _ = LoadProfile();
        }

        float ScoreIncrement = 0.6f;
        float PunishScoreIncrement = 1.2f;
        float ScoreMultiplierIncrement = 1.5f;

        public async Task ChangeProgress(string Term, bool Correct)
        {
            int index = Data.LearnedVocab.IndexOf(Term);
            float points = ScoreIncrement;
            
            if (index == -1) //if term not known
            {
                Data.LearnedVocab.Add(Term);
                Data.VocabProgression.Add(points);
                Data.VocabStreak.Add(1);
            }
            else //if term known
            {
                bool LastCorrect = Data.VocabStreak[index] >= 1;
                if(LastCorrect != Correct)
                {
                    Data.VocabStreak[index] = Correct ? 1 : -1;
                }else
                {
                    Data.VocabStreak[index] += Correct ? 1 : -1;
                }

                points = (Correct ? ScoreIncrement : PunishScoreIncrement) * ((Data.VocabProgression[index]/75f) + 1f);
                points = points * (Data.VocabStreak[index] * ScoreMultiplierIncrement);
                Data.VocabProgression[index] += points;

                Data.VocabProgression[index] = Math.Clamp(Data.VocabProgression[index], 0, 100);
            }

            //Data.TotalPoints += Points; //TotalKnowledge instead

            await SaveProfile();
        }
        
        public async Task ForgetTerm(string Term)
        {
            for (int i = 0; i < Data.LearnedVocab.Count; i++)
            {
                if (Data.LearnedVocab[i] == Term)
                {
                    Data.LearnedVocab.RemoveAt(i);
                    Data.VocabProgression.RemoveAt(i);
                    Data.VocabStreak.RemoveAt(i);
                    
                    await SaveProfile();
                }
            }   
        }

        public async Task LoadProfile()
        {
            if(await Local.ContainKeyAsync("profile"))
            {
                try
                {
                    Data = await Local.GetItemAsync<ProfileData>("profile") ?? new ProfileData();
                    Console.WriteLine("Profile loaded.");
                }catch
                {
                    await Local.RemoveItemAsync("profile");
                    Console.WriteLine("Profile corrupt, clearing...");
                    Data = new ProfileData();
                    await SaveProfile();
                }
            }else
            {
                Console.WriteLine("Profile not found, creating...");
                Data = new ProfileData();
                await SaveProfile();
            }
            
            Initialized = true;
        }
        
        public async Task SaveProfile()
        {
            await Local.SetItemAsync("profile", Data);
            Console.WriteLine("Profile saved.");
        }
    }
}
