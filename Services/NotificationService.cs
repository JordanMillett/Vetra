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
            await Runtime.InvokeVoidAsync("initializePushNotifications");
            
            var dotNetReference = DotNetObjectReference.Create(this);
            await Runtime.InvokeVoidAsync("setupMessageListener", dotNetReference);
        }

        [JSInvokable]
        public void OnNotificationReceived(string title, string body)
        {
            Console.WriteLine($"Notification received: {title} - {body}");
            
            Toast.Notify(new(BlazorBootstrap.ToastType.Info, $"{title}: {body}"));
        }
    }
}