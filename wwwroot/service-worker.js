self.addEventListener('install', event => {
    console.log('Service Worker Installed');
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    console.log('Service Worker Activated');
    
    console.log("SHAKE DA PHONE");
    navigator.vibrate([20000, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10]);
    console.log(navigator.userAgent);
    
    
    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
    event.respondWith(
        (async () =>
        {
            try
            {
                
                const response = await fetch(event.request);
                
                
                return response;
                
            } catch (error)
            {
                //console.error('Failed to fetch due to: ', error);

                return new Response('Fetching Error', {
                    status: 500,
                    statusText: 'Internal Server Error'
                });
            }
        })()
    );
});

// Listen for push notifications
self.addEventListener('push', event => {
    const data = event.data ? event.data.json() : {};
    
    const options =
    {
        title: data.Title,
        actions: [
          {
            action: 'launch',
            url: 'https://jordanmillett.github.io/Vetra',
            title: 'Launch Vetra'
          }
        ],
        body: data.Body,
        dir: 'auto',
        icon: '/images/icon_192.png',
        badge: '/images/icon_96.png',
        lang: 'en',
        url: 'https://jordanmillett.github.io/Vetra',
        tag: 'vetra-alert',
        requireInteraction: false,
        timestamp: Date.now()
    };
    
    event.waitUntil(
        self.registration.showNotification(data.Title, options)
    );
    
     // You can also send a message to the client to alert the user
     event.waitUntil(
        self.clients.matchAll().then(clients => {
            clients.forEach(client => {
                client.postMessage({
                    title: data.Title,
                    body: data.Body
                });
            });
        })
    );
});

// Handle notification click
self.addEventListener('notificationclick', event =>
{
    event.notification.close();
    
    //const action = event.action;
    
    event.waitUntil(
        clients.openWindow(event.notification.url)
    );
});