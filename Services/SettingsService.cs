namespace Vetra
{
    public class SettingsService
    {
        public bool UsingDarkTheme { get; set; } = false;

        public void ToggleTheme()
        {
            UsingDarkTheme = !UsingDarkTheme;
        }
    }
}
