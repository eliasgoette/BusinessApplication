using BusinessApplication.Model;
using System.ComponentModel;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        private string _filePath = "";
        private List<Customer> _data = [];

        public ImportViewModel()
        {
            Browse = new RelayCommand(ExecuteBrowse);
            Confirm = new RelayCommand(ExecuteConfirm);
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        public List<Customer> Data => _data;

        public void ExecuteBrowse()
        {
            var fileTypeFilters = "JSON files (*.json)|*.json|XML files(*.xml) | *.xml";

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "";
            dialog.DefaultExt = ".json";
            dialog.Filter = fileTypeFilters;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                FilePath = dialog.FileName;
            }
        }

        public void ExecuteConfirm()
        {

        }
        
        public ICommand Browse { get; }
        public ICommand Confirm { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
