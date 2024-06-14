using System.ComponentModel;

namespace BusinessApplication.ViewModel
{
    public class ExportViewModel : INotifyPropertyChanged
    {
        private List<object> _data;

        public event PropertyChangedEventHandler PropertyChanged;

        public ExportViewModel(List<object> dataAsObjects)
        {
            _data = dataAsObjects;
        }

        public List<object> Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
