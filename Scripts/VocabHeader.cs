namespace Vetra
{
    public class VocabHeader
    {
        public string RU { get; set; }
        public string EN { get; set; }
        public string Category { get; set; }
        public string? Gender { get; set; }
        public string? Icon { get; set; }
        public string? IconBW { get; set; }
        
        public string? Special { get; set; }

        public VocabHeader()
        {
            RU = "MISSING";
            EN = "MISSING";
            Category = "MISSING";
            Gender = null;
            Icon = null;
            IconBW = null;
            Special = null;
        }
    }
}
