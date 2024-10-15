using System.Text.Json;

namespace Vetra
{
    public class LessonService
    {
        public Dictionary<string, LessonHeader> LessonHeaders { get; private set; }

        private readonly HttpClient _httpClient;
        
        public LessonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LessonHeaders = new Dictionary<string, LessonHeader>();
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

            }catch (Exception ex)
            {
                Console.WriteLine($"Error loading Lesson Headers: {ex.Message}");
                
                LessonHeaders = new Dictionary<string, LessonHeader>()
                {
                    {"ERROR", new LessonHeader("ERROR", "Bug", new Lesson())}
                };
            }
            }
    }
}
