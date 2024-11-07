self.addEventListener('install', event => {
    console.log('Service Worker Installed');
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    console.log('Service Worker Activated');
    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
    //console.log('Service Worker Fetching: ', event.request.url);
    event.respondWith(fetch(event.request));
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