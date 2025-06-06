using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Vetra;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddBlazorBootstrap();
builder.Services.AddSingleton<BlazorBootstrap.ToastService>();

builder.Services.AddSingleton<HelperService>();
//builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton<TextToSpeechService>();
builder.Services.AddSingleton<SpeechToTextService>();
builder.Services.AddSingleton<AudioService>();

builder.Services.AddSingleton<SettingsService>();
builder.Services.AddSingleton<ProfileService>();
builder.Services.AddSingleton<LessonService>();

var host = builder.Build();

//await host.Services.GetRequiredService<NotificationService>().LoadServiceWorker();

host.Services.GetRequiredService<TextToSpeechService>().Preload();

var lessonService = host.Services.GetRequiredService<LessonService>();
await lessonService.LoadLessonHeadersAsync("data/lesson-headers.json");
await lessonService.LoadVocabHeadersAsync("data/vocab-headers.json");

await host.RunAsync();