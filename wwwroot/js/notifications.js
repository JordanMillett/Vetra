async function isServerOnline()
{   
    try
    {
        const response = await fetch("https://vetra.jordanmillett.net/api/pulse", {
            method: 'GET'
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

        let subscription = await registration.pushManager.getSubscription();

        if (!subscription)
        {
            const subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: 'BBIS4Xi3mCa3vxm7ymp5TWVgYhlTEMgf7vr1cn1eMsz3akiJ1WLIvA538FyJN4tTtBqKrTmu0t6McyZ6ISnY_qQ'
            });
        }
        
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
                    "https://vetra.jordanmillett.net/api/notifications/subscribe" :
                    "https://vetra.jordanmillett.net/api/notifications/unsubscribe";
                
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });
        
                if (response.ok)
                {
                    console.log('Subscription update successful');
                } else
                {
                    console.error('Subscription update failed with status: ', response.status);
                }
            } catch (error)
            {
                console.error('Error during subscription update:', error);
            }
        }
    }
}

async function getEndpoint()
{
    if ('serviceWorker' in navigator && 'PushManager' in window)
    {
        const registration = await navigator.serviceWorker.register('service-worker.js');
        
        let subscription = await registration.pushManager.getSubscription();
        
        if (!subscription)
            return "";
        
        const endpoint = subscription.endpoint;

        return endpoint.toString();
    }

    return "";
}