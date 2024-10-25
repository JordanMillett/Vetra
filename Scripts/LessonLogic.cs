namespace Vetra
{
    public class LessonLogic
    {
        public List<VocabHeader> Included { get; private set; } 
        public List<VocabHeader> Filler { get; private set; } 
        public Dictionary<string, bool> Results { get; private set; }

        public LessonLogic() //used for practice
        {
            Included = new List<VocabHeader>();
            Filler = new List<VocabHeader>();
            Results = new Dictionary<string, bool>();
        }
    }
}
