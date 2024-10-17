namespace Vetra
{
    public class VocabHeader
    {
        public string RU { get; set; }
        public string EN { get; set; }
        
        //Definitions, sentence pairs (tuple?)

        public VocabHeader()
        {
            RU = "Russian";
            EN = "English";
        }
    }
}
