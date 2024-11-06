using Vetra;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

namespace Vetra
{
    public class NotificationService
    {
        IJSRuntime Runtime;
        SettingsService Settings;
        BlazorBootstrap.ToastService Toast;

        public NotificationService(IJSRuntime JS, SettingsService S, BlazorBootstrap.ToastService toast)
        {
            Toast = toast;
            Runtime = JS;
            Settings = S;
        }
        
        public async Task LoadServiceWorker()
        {
            await Runtime.InvokeVoidAsync("loadServiceWorker");
            
            if(Settings.Data.EnableNotifications)
            {
                Settings.Data.EnableNotifications = await Runtime.InvokeAsync<bool>("askForNotifications");
            }
            
            if (await ServerAlive())
            {
                var dotNetReference = DotNetObjectReference.Create(this);
                await Runtime.InvokeVoidAsync("setupMessageListener", dotNetReference);
                
                await Runtime.InvokeVoidAsync("updateNotificationStatus", Settings.Data.EnableNotifications);
            }
        }

        [JSInvokable]
        public void OnNotificationReceived(string title, string body)
        {
            Console.WriteLine($"Notification received: {title} - {body}");
            
            Toast.Notify(new(BlazorBootstrap.ToastType.Info, $"{title}: {body}"));
        }
        
        public async Task<bool> ServerAlive()
        {
            return await Runtime.InvokeAsync<bool>("isServerOnline");
        }
    }
}