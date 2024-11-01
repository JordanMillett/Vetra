using System.Text.Json;
using System.Text.Encodings.Web;

namespace Vetra
{
    public class LessonService
    {
        public Dictionary<string, LessonHeader> LessonHeaders { get; private set; }
        public Dictionary<string, VocabHeader> VocabHeaders { get; private set; }

        private readonly HttpClient _httpClient;
        private ProfileService Profile;
        private SettingsService Settings;

        public int LessonType = -1; 
        
        public LessonService(HttpClient httpClient, ProfileService profile, SettingsService settings)
        {
            _httpClient = httpClient;
            Profile = profile;
            Settings = settings;
            LessonHeaders = new Dictionary<string, LessonHeader>();
            VocabHeaders = new Dictionary<string, VocabHeader>();
        }
        
        public void Fill(ref LessonLogic Logic)
        {
            Fill(ref Logic, "");
        }
        
        public void Fill(ref LessonLogic Logic, string LessonKey)
        {
            do
            {
                Random Rand = new Random();
                int RandomIndex;
                string RandomKey;
                
                if(LessonKey != "")
                {
                    RandomIndex = Rand.Next(0, LessonHeaders[LessonKey].Vocab.Count);
                    RandomKey = LessonHeaders[LessonKey].Vocab[RandomIndex];
                }else
                {
                    if (Profile.Data.LearnedVocab.Count < 4)
                    {
                        RandomIndex = Rand.Next(0, VocabHeaders.Count);
                        RandomKey = VocabHeaders.ElementAt(RandomIndex).Key;
                    }
                    else
                    {
                        RandomIndex = Rand.Next(0, Profile.Data.LearnedVocab.Count);
                        RandomKey = Profile.Data.LearnedVocab[RandomIndex];
                    }
                }

                VocabHeader Filler = VocabHeaders[RandomKey];

                if (!Logic.Filler.Contains(Filler) && Logic.Term != Filler)
                    Logic.Filler.Add(Filler);

            } while (Logic.Filler.Count < 3);
        }
        
        public int GetLessonType(string Term)
        {
            int Index = Profile.Data.LearnedVocab.IndexOf(Term);
            
            if(Index == -1) //if term not known
            {
                return 0;
            }else
            {
                Random Rand = new Random();
                int Chosen = 0; //include, exclude  CHANGE WHEN ADDING NEW GAME
                
                if(Settings.Data.EnableSpeechLessons)
                    Chosen = Rand.Next(1, 6);
                else
                    Chosen = Rand.Next(1, 5);

                if(Profile.Data.VocabProgression[Index] == 0) //if term at zero
                    Chosen = 0;

                return Chosen;
            }
        }
        
        public VocabHeader GetNextTerm() //used for practice
        {
            Random Rand = new Random();
            int StartingIndex = Rand.Next(0, Profile.Data.LearnedVocab.Count);
            string ReturnKey = Profile.Data.LearnedVocab[StartingIndex];
            float Progress = 100f;

            for (int i = 0; i < Profile.Data.LearnedVocab.Count; i++)
            {
                if (Profile.Data.VocabProgression[i] < Progress)
                {
                    ReturnKey = Profile.Data.LearnedVocab[i];
                    Progress = Profile.Data.VocabProgression[i];
                }
            }

            LessonType = GetLessonType(ReturnKey);
            return VocabHeaders[ReturnKey];
        }
        
        public VocabHeader GetNextTerm(string Key) //used for lessons
        {
            Random Rand = new Random();
            int StartingIndex = Rand.Next(0, LessonHeaders[Key].Vocab.Count);
            string ReturnKey = LessonHeaders[Key].Vocab[StartingIndex];
            float Progress = 100f;

            for (int i = 0; i < LessonHeaders[Key].Vocab.Count; i++)
            {
                string VocabKey = LessonHeaders[Key].Vocab[i];

                if (!Profile.Data.LearnedVocab.Contains(VocabKey)) //Term is not known show next!
                {
                    LessonType = GetLessonType(VocabKey);
                    return VocabHeaders[VocabKey];
                }
                else                                              //Term is known
                {
                    if (Profile.Data.VocabProgression[Profile.Data.LearnedVocab.IndexOf(VocabKey)] < Progress)
                    {
                        ReturnKey = VocabKey;
                        Progress = Profile.Data.VocabProgression[Profile.Data.LearnedVocab.IndexOf(VocabKey)];
                    }
                }

            }

            LessonType = GetLessonType(ReturnKey);
            return VocabHeaders[ReturnKey];
        }

        public async Task LoadLessonHeadersAsync(string jsonFilePath)
        {
            try
            {
                var response = await _httpClient.GetAsync(jsonFilePath);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    LessonHeaders = JsonSerializer.Deserialize<Dictionary<string, LessonHeader>>(json)!;
                }

                //throw new Exception();

            }catch (Exception ex)
            {
                Console.WriteLine($"Error loading Lesson Headers: {ex.Message}");
                
                LessonHeaders = new Dictionary<string, LessonHeader>()
                {
                    {"ERROR", new LessonHeader()}
                };
                
                string json = JsonSerializer.Serialize(LessonHeaders, new JsonSerializerOptions { WriteIndented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping  });
                Console.WriteLine(json);
            }
        }
        
        public async Task LoadVocabHeadersAsync(string jsonFilePath)
        {
            try
            {
                var response = await _httpClient.GetAsync(jsonFilePath);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    VocabHeaders = JsonSerializer.Deserialize<Dictionary<string, VocabHeader>>(json)!;
                }

                //throw new Exception();

            }catch (Exception ex)
            {
                Console.WriteLine($"Error loading Lesson Headers: {ex.Message}");
                
                VocabHeaders = new Dictionary<string, VocabHeader>()
                {
                    {"ERROR", new VocabHeader() }
                };
                
                string json = JsonSerializer.Serialize(VocabHeaders, new JsonSerializerOptions { WriteIndented = false, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping  });
                Console.WriteLine(json);
            }
        }
    }
}
