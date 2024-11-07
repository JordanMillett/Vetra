async function isServerOnline()
{   
    
    try
    {
        const response = await fetch("https://mole-factual-pleasantly.ngrok-free.app/api/pulse", {
            method: 'GET',
            headers: {
                'ngrok-skip-browser-warning': 'true'
            }
        });
        
        return response.ok;

    } catch
    {
        
        console.log("VetraHub is offline");
        return false;
    }
    
}

async function loadServiceWorker()
{
    if ('serviceWorker' in navigator)
    {
        navigator.serviceWorker.register('service-worker.js');
        console.log('Service Worker Registered.');
    }
}

function setupMessageListener(dotnetHelper) {
    navigator.serviceWorker.addEventListener('message', event => {
        const { title, body } = event.data;
        dotnetHelper.invokeMethodAsync('OnNotificationReceived', title, body)
    });
}

async function askForNotifications()
{
    if ('serviceWorker' in navigator && 'PushManager' in window)
    {
        const permission = await Notification.requestPermission();
        
        if (permission === 'granted')
        {
            console.log('Push notifications permission granted.');
            return true;
        } else
        {
            console.log('Push notifications permission denied.');
            return false;
        }
    } else
    {
        console.log('Push messaging is not supported.');
        return false;
    }
}

async function updateNotificationStatus(staySubscribed)
{
    if ('serviceWorker' in navigator && 'PushManager' in window)
    {
        const registration = await navigator.serviceWorker.register('service-worker.js');

        const subscription = await registration.pushManager.subscribe({
            userVisibleOnly: true,
            applicationServerKey: 'BBIS4Xi3mCa3vxm7ymp5TWVgYhlTEMgf7vr1cn1eMsz3akiJ1WLIvA538FyJN4tTtBqKrTmu0t6McyZ6ISnY_qQ' // replace with your actual VAPID public key
        });
        
        const p256dh = subscription.getKey('p256dh');
        const auth = subscription.getKey('auth');
        
        const p256dhBase64 = btoa(String.fromCharCode(...new Uint8Array(p256dh)));
        const authBase64 = btoa(String.fromCharCode(...new Uint8Array(auth)));
        
        const payload = {
            endpoint: subscription.endpoint,
            p256dh: p256dhBase64,
            auth: authBase64
        };
        
        if (await isServerOnline())
        {
            try
            {
                url = staySubscribed ?
                    "https://mole-factual-pleasantly.ngrok-free.app/api/notifications/subscribe" :
                    "https://mole-factual-pleasantly.ngrok-free.app/api/notifications/unsubscribe";
                
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });
        
                if (response.ok) {
                    console.log('Subscription update successful:', await response.json());
                } else {
                    console.error('Subscription update failed with status:', response.status, await response.text());
                }
            } catch (error)
            {
                console.error('Error during subscription update:', error);
            }
        }
    }
}
