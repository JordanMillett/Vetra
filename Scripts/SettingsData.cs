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
        
        void MarkDirty()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}