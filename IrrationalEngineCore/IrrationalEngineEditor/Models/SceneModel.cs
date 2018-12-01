using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IrrationalEngineEditor.Models
{
    public class SceneModel : INotifyPropertyChanged
    {
        private string _sceneName;

        public string SceneName {
            get => _sceneName;
            set {
                if (_sceneName != value)
                {
                    _sceneName = value;
                    OnPropertyChanged(nameof(SceneName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
