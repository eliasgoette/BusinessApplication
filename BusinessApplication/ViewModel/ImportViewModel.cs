using BusinessApplication.Model;
using BusinessApplication.Repository;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        private ILogger _logger;
        private IRepository<Customer> _repository;
        private string _filePath = "";
        private List<Customer> _data = [];
        private bool _confirmIsEnabled = false;

        public ImportViewModel(ILogger logger, IRepository<Customer> repository)
        {
            _logger = logger;
            _repository = repository;
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

        public bool ConfirmIsEnabled
        {
            get => _confirmIsEnabled;
            set
            {
                _confirmIsEnabled = value;
                OnPropertyChanged(nameof(ConfirmIsEnabled));
            }
        }

        private void ExecuteBrowse()
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
                        var fileContent = File.ReadAllText(FilePath);

                        switch (fileEnding)
                        {
                            case "json":
                                Data = Serializer.FromJson<Customer>(fileContent);
                                ConfirmIsEnabled = true;
                                break;

                            case "xml":
                                Data = Serializer.FromXml<Customer>(fileContent);
                                ConfirmIsEnabled = true;
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

        private async void ExecuteConfirm()
        {
            ConfirmIsEnabled = false;

            foreach (var item in Data)
            {
                await _repository.AddAsync(item);
            }

            ConfirmIsEnabled = true;

            // TODO: Close window
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
