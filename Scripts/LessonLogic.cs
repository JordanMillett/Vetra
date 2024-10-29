namespace Vetra
{
    public class LessonLogic
    {
        public VocabHeader Term { get; set; }
        public List<VocabHeader> Filler { get; private set; } 
        public bool Passed { get; set; } 

        public LessonLogic()
        {
            Term = new VocabHeader();
            Filler = new List<VocabHeader>();
            Passed = false;
        }
    }
}
