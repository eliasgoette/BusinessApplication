using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public enum ExportMode
    {
        Json,
        Xml
    }

    public class ExportViewModel : INotifyPropertyChanged
    {
        private List<object> _data;
        private string _result = "";
        private ExportMode _selectedMode = ExportMode.Json;
        public event PropertyChangedEventHandler PropertyChanged;

        public ExportViewModel(List<object> dataAsObjects)
        {
            Save = new RelayCommand(ExecuteSave);

            _data = dataAsObjects;
            ExecuteSerialization();
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
                    ExecuteSerialization();
                }
            }
        }

        public string Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ExportMode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                if (_selectedMode != value)
                {
                    _selectedMode = value;
                    OnPropertyChanged(nameof(SelectedMode));
                    ExecuteSerialization();
                }
            }
        }

        private void ExecuteSerialization()
        {
            if (SelectedMode == ExportMode.Json)
            {
                Result = Serializer.ToJson(_data);
            }
            else
            {
                Result = Serializer.ToXml(_data);
            }
        }

        private void ExecuteSave()
        {
            var dialog = new SaveFileDialog
            {
                FileName = "DataExport",
                DefaultExt = SelectedMode.ToString().ToLower(),
                Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml"
            };

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Save document
                string filename = dialog.FileName;
            }
        }

        public ICommand Save { get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
