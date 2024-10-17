namespace Vetra
{
    public class VocabHeader
    {
        public string EN { get; set; }
        public string RU { get; set; }

        //Definitions, sentence pairs (tuple?)

        public VocabHeader()
        {
            EN = "ENGLISH";
            RU = "RUSSIAN";
        }
    }
}
