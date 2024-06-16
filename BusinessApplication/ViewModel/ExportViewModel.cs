using BusinessApplication.Model;
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
        private List<Customer> _data;
        private string _result = "";
        private ExportMode _selectedMode;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ExportViewModel(List<Customer> data)
        {
            SelectedMode = (int)ExportMode.Json;
            Data = data;
            ExecuteSerialization();

            Save = new RelayCommand(ExecuteSave);
        }

        public List<Customer> Data
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

        public string[] AvailableModes { get; } = Enum.GetNames(typeof(ExportMode));

        public string Result
        {
            get { return _result; }
            private set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public int SelectedMode
        {
            get { return Convert.ToInt32(_selectedMode); }
            set
            {
                _selectedMode = (ExportMode)value;
                OnPropertyChanged(nameof(SelectedMode));
                ExecuteSerialization();
            }
        }

        private void ExecuteSerialization()
        {
            var mode = (ExportMode)SelectedMode;

            if (mode == ExportMode.Json)
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
                string filename = dialog.FileName;
                // TODO: Save document
            }
        }

        public ICommand Save { get; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
