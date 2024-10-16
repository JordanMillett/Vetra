namespace Vetra
{
    public class VocabHeader
    {
        public string EN { get; set; }
        public string RU { get; set; }

        //Definitions, sentence pairs (tuple?)

        public VocabHeader(string en, string ru)
        {
            EN = en;
            RU = ru;
        }
    }
}
