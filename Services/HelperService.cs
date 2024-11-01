public class HelperService
{
    public string CapitalizedCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
            
        bool isNumber = int.TryParse(input, out int number);
        string output = input;
        
        if (isNumber)
        {
            if (RussianNumbers.ContainsKey(number))
                output = RussianNumbers[number];
        }
        
        output = output.ToLower();
        output = output.Replace("ё", "е");
        output = char.ToUpper(output[0]) + output.Substring(1).ToLower();
        return output;
    }
    
    public string ConvertFromURL(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "";
        
        return string.Join(' ', input.Replace('+', ' ').Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }
    
    Dictionary<int, string> RussianNumbers = new Dictionary<int, string>
    {
        { 0, "ноль" },
        { 1, "один" },
        { 2, "два" },
        { 3, "три" },
        { 4, "четыре" },
        { 5, "пять" },
        { 6, "шесть" },
        { 7, "семь" },
        { 8, "восемь" },
        { 9, "девять" },
        { 10, "десять" }
    };
}