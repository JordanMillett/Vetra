namespace Vetra
{
    public class LessonHeader
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Lesson Data { get; set; }

        public LessonHeader(string name, string icon, Lesson data)
        {
            Name = name;
            Icon = icon;
            Data = data;
        }
    }
}
