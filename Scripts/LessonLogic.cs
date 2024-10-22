namespace Vetra
{
    public class LessonLogic
    {
        public string LessonKey { get; set; }
        
        public List<VocabHeader> Included { get; private set; } 
        public Dictionary<string, bool> Results { get; private set; }

        public LessonLogic(string lessonkey)
        {
            LessonKey = lessonkey;
            Included = new List<VocabHeader>();
            Results = new Dictionary<string, bool>();
        }
    }
}
