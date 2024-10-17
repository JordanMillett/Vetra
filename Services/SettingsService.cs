using System.ComponentModel;

namespace Vetra
{
    public class SettingsService
    {
        public event EventHandler? RefreshNavigation;
        
        Blazored.LocalStorage.ILocalStorageService Local;

        public SettingsData Data = new SettingsData();

        public bool Initialized = false;

        public SettingsService(Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            Local = localStorage;
            _ = LoadSettings();
        }

        private void OnSettingsDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            RefreshNavigation?.Invoke(this, EventArgs.Empty);
            _ = SaveSettings();
        }

        public async Task LoadSettings()
        {
            if(await Local.ContainKeyAsync("settings"))
            {
                try
                {
                    Data = await Local.GetItemAsync<SettingsData>("settings") ?? new SettingsData();
                    Console.WriteLine("Settings loaded.");
                }catch
                {
                    await Local.RemoveItemAsync("settings");
                    Console.WriteLine("Settings corrupt, clearing...");
                    Data = new SettingsData();
                    await SaveSettings();
                }
            }else
            {
                Console.WriteLine("Settings not found, creating...");
                Data = new SettingsData();
                await SaveSettings();
            }
            
            Data.PropertyChanged += OnSettingsDataChanged;
            Initialized = true;
        }
        
        public async Task SaveSettings()
        {
            await Local.SetItemAsync("settings", Data);
            Console.WriteLine("Settings saved.");
        }
    }
}
