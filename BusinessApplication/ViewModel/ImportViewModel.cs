using BusinessApplication.Model;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        private ILogger _logger;
        private string _filePath = "";
        private List<Customer> _data = [];

        public ImportViewModel(ILogger logger)
        {
            _logger = logger;
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

        public List<Customer> Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        public void ExecuteBrowse()
        {
            try
            {
                var fileTypeFilters = "JSON files (*.json)|*.json|XML files(*.xml)|*.xml";

                var dialog = new OpenFileDialog
                {
                    FileName = "",
                    DefaultExt = ".json",
                    Filter = fileTypeFilters
                };

                bool result = dialog.ShowDialog() ?? false;

                if (result)
                {
                    FilePath = dialog.FileName;

                    if (File.Exists(FilePath))
                    {
                        var fileEnding = FilePath.Split('.').LastOrDefault()?.ToLower();

                        switch (fileEnding)
                        {
                            case "json":
                                string json = File.ReadAllText(FilePath);
                                Data = Serializer.FromJson<Customer>(json);
                                break;

                            case "xml":
                                string xml = File.ReadAllText(FilePath);
                                Data = Serializer.FromXml<Customer>(xml);
                                break;

                            default:
                                _logger.LogMessage("Wrong file extension.");
                                break;
                        }
                    }
                    else
                    {
                        _logger.LogMessage("File doesn't exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
