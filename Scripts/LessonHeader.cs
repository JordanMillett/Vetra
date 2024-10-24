namespace Vetra
{
    public class LessonHeader
    {
        public string Name { get; set; }
        public string IconBW { get; set; }
        public List<string> Vocab { get; set; }

        public LessonHeader()
        {
            Name = "ERROR";
            IconBW = "MISSING";
            Vocab = new List<string>(){"Vocab", "Vocab", "Vocab"};
        }
    }
}
