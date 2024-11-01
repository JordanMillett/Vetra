using System.ComponentModel;
using Microsoft.JSInterop;

namespace Vetra
{
    public class SettingsService
    {
        public event EventHandler? RefreshNavigation;
        
        Blazored.LocalStorage.ILocalStorageService Local;
        
        IJSRuntime Runtime;

        public SettingsData Data = new SettingsData();

        public bool Initialized = false;

        public SettingsService(IJSRuntime JS, Blazored.LocalStorage.ILocalStorageService localStorage)
        {
            Runtime = JS;
            Local = localStorage;
            _ = LoadSettings();
        }

        private void OnSettingsDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            _ = ApplySettings();
            _ = SaveSettings();
        }
        
        public async Task ApplySettings()
        {
            if (!Data.EnableSoundEffects)
                Data.EnableMicSounds = false;
            RefreshNavigation?.Invoke(this, EventArgs.Empty);
            await Runtime.InvokeVoidAsync("setDarkTheme", Data.EnableDarkTheme);
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
            //Console.WriteLine("Settings saved.");
        }
    }
}
