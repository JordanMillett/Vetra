namespace Vetra
{
    public class VocabHeader
    {
        public string RU { get; set; }
        public string EN { get; set; }
        public string Category { get; set; }
        public string Gender { get; set; }

        public VocabHeader()
        {
            RU = "Russian";
            EN = "English";
            Category = "Category";
            Gender = "Gender";
        }
    }
}
