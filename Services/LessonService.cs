using System.Text.Json;

namespace Vetra
{
    public class LessonService
    {
        public Dictionary<string, LessonHeader> LessonHeaders { get; private set; }
        public Dictionary<string, VocabHeader> VocabHeaders { get; private set; }

        private readonly HttpClient _httpClient;
        
        public LessonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LessonHeaders = new Dictionary<string, LessonHeader>();
            VocabHeaders = new Dictionary<string, VocabHeader>();
        }

        public async Task LoadLessonHeadersAsync(string jsonFilePath)
        {
            try
            {
                var response = await _httpClient.GetAsync(jsonFilePath);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    LessonHeaders = JsonSerializer.Deserialize<Dictionary<string, LessonHeader>>(json)!;
                }

                //throw new Exception();

            }catch (Exception ex)
            {
                Console.WriteLine($"Error loading Lesson Headers: {ex.Message}");
                
                LessonHeaders = new Dictionary<string, LessonHeader>()
                {
                    {"ERROR", new LessonHeader("ERROR", "Bug", new List<string>(){"Test", "Example"})}
                };
                
                string json = JsonSerializer.Serialize(LessonHeaders, new JsonSerializerOptions { WriteIndented = false });
                Console.WriteLine(json);
            }
        }
        
        public async Task LoadVocabHeadersAsync(string jsonFilePath)
        {
            try
            {
                var response = await _httpClient.GetAsync(jsonFilePath);
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    VocabHeaders = JsonSerializer.Deserialize<Dictionary<string, VocabHeader>>(json)!;
                }

                //throw new Exception();

            }catch (Exception ex)
            {
                Console.WriteLine($"Error loading Lesson Headers: {ex.Message}");
                
                VocabHeaders = new Dictionary<string, VocabHeader>()
                {
                    {"ERROR", new VocabHeader("ERROR", "ERROR") }
                };
                
                string json = JsonSerializer.Serialize(VocabHeaders, new JsonSerializerOptions { WriteIndented = false });
                Console.WriteLine(json);
            }
        }
    }
}
