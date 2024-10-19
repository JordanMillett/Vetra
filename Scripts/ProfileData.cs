namespace Vetra
{
    public class ProfileData
    {
        public string Name { get; set; }
        public float TotalPoints { get; set; }
        public List<string> LearnedVocab { get; set; }
        public List<float> VocabProgression { get; set; }
        public List<int> VocabStreak { get; set; }

        public ProfileData()
        {
            Name = "User";
            TotalPoints = 0;
            LearnedVocab = new List<string>();
            VocabProgression = new List<float>();
            VocabStreak = new List<int>();
        }
    }
}
