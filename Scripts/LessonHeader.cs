namespace Vetra
{
    public class LessonHeader
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<string> Vocab { get; set; }

        public LessonHeader(string name, string icon, List<string> vocab)
        {
            Name = name;
            Icon = icon;
            Vocab = vocab;
        }
    }
}
