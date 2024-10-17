namespace Vetra
{
    public class LessonHeader
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<string> Vocab { get; set; }

        public LessonHeader()
        {
            Name = "ERROR";
            Icon = "Bug";
            Vocab = new List<string>(){"Vocab", "Vocab", "Vocab"};
        }
    }
}
