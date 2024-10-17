namespace Vetra
{
    public class ProfileData
    {
        public string Name { get; set; }
        public int TotalPoints { get; set; }
        public List<string> LearnedVocab { get; set; }
        public List<int> VocabProgression { get; set; }

        public ProfileData()
        {
            Name = "User";
            TotalPoints = 0;
            LearnedVocab = new List<string>();
            VocabProgression = new List<int>();
        }
    }
}
