async function loadServiceWorker()
{
    if ('serviceWorker' in navigator) {
        window.addEventListener('load', () => {
            navigator.serviceWorker.register('service-worker.js')
                .then(registration => {
                    console.log('Service Worker Registered.');
                })
        });
    }
}

function setupMessageListener(dotnetHelper) {
    navigator.serviceWorker.addEventListener('message', event => {
        const { title, body } = event.data;
        dotnetHelper.invokeMethodAsync('OnNotificationReceived', title, body)
    });
}

async function initializePushNotifications()
{
    // Check if service workers and push messaging are supported
    if ('serviceWorker' in navigator && 'PushManager' in window)
    {
        // Register the service worker
        const registration = await navigator.serviceWorker.register('service-worker.js');

        // Ask for permission to send notifications
        const permission = await Notification.requestPermission();
        
        if (permission === 'granted') {
            // Subscribe the user to push notifications
            const subscription = await registration.pushManager.subscribe({
                userVisibleOnly: true,
                applicationServerKey: 'BBIS4Xi3mCa3vxm7ymp5TWVgYhlTEMgf7vr1cn1eMsz3akiJ1WLIvA538FyJN4tTtBqKrTmu0t6McyZ6ISnY_qQ' // replace with your actual VAPID public key
            });
            
            // Extract the keys
            const p256dh = subscription.getKey('p256dh');
            const auth = subscription.getKey('auth');
            
            const p256dhBase64 = btoa(String.fromCharCode(...new Uint8Array(p256dh)));
            const authBase64 = btoa(String.fromCharCode(...new Uint8Array(auth)));
            
            const payload = {
                endpoint: subscription.endpoint,
                p256dh: p256dhBase64,
                auth: authBase64
            };

            try {
                // Send the subscription details to your server
                const response = await fetch('https://mole-factual-pleasantly.ngrok-free.app/api/notifications/subscribe', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });
        
                // Check if the response is successful
                if (response.ok) {
                    console.log('Subscription successful:', await response.json());
                } else {
                    console.error('Subscription failed with status:', response.status, await response.text());
                }
            } catch (error) {
                console.error('Error during subscription:', error);
            }
            
        } else {
            console.log('Push notifications permission denied.');
        }
    } else {
        console.log('Push messaging is not supported.');
    }
}
