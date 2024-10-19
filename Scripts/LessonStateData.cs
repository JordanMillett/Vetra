namespace Vetra
{
    public class LessonStateData
    {
        public string Key { get; set; }
        
        public List<string> Tested { get; set; }
        public List<bool> Results { get; set; }

        public LessonStateData(string key)
        {
            Key = key;
            Tested = new List<string>();
            Results = new List<bool>();
        }
    }
}
