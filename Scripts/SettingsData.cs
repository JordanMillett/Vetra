using System.ComponentModel;

namespace Vetra
{
    public class SettingsData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _DevToolsEnabled = false;
        public bool DevToolsEnabled { get => _DevToolsEnabled; set{
            if (_DevToolsEnabled != value){
                _DevToolsEnabled = value;
                MarkDirty();
            }}}
            
        private bool _HideNotifications = true;
        public bool HideNotifications { get => _HideNotifications; set{
            if (_HideNotifications != value){
                _HideNotifications = value;
                MarkDirty();
            }}}
            
        private bool _EnableTTS = true;
        public bool EnableTTS { get => _EnableTTS; set{
            if (_EnableTTS != value){
                _EnableTTS = value;
                MarkDirty();
            }}}
            
        private bool _EnableSoundEffects = true;
        public bool EnableSoundEffects { get => _EnableSoundEffects; set{
            if (_EnableSoundEffects != value){
                _EnableSoundEffects = value;
                MarkDirty();
            }}}
        
        private bool _EnableSpeechLessons = true;
        public bool EnableSpeechLessons { get => _EnableSpeechLessons; set{
            if (_EnableSpeechLessons != value){
                _EnableSpeechLessons = value;
                MarkDirty();
            }}}
        
        void MarkDirty()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}