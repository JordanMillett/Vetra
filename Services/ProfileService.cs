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

        public async Task AddProgress(string Term, int Points)
        {
            int index = Data.LearnedVocab.IndexOf(Term);

            if (index == -1)
            {
                Data.LearnedVocab.Add(Term);
                Data.VocabProgression.Add(Points);
            }
            else
            {
                Data.VocabProgression[index] += Points;
            }

            Data.TotalPoints += Points;

            await SaveProfile();
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
