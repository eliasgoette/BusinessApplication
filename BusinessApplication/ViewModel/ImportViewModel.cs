﻿using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
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
        private ICommand _closeWindow;

        public ImportViewModel(IRepository<Customer> repository, ILogger logger, ICommand closeWindow)
        {
            _repository = repository;
            _logger = logger;
            Browse = new RelayCommand(ExecuteBrowse);
            Confirm = new RelayCommand(ExecuteConfirm);
            _closeWindow = closeWindow;
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
                LoadDataFromPath();
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void LoadDataFromPath()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var fileEnding = FilePath.Split('.').LastOrDefault()?.ToLower();
                    var fileContent = File.ReadAllText(FilePath);

                    switch (fileEnding)
                    {
                        case "json":
                            Data = CustomerSerializer.FromJson(fileContent);
                            ConfirmIsEnabled = true;
                            break;

                        case "xml":
                            Data = CustomerSerializer.FromXml(fileContent);
                            ConfirmIsEnabled = true;
                            break;

                        default:
                            Data = [];
                            _logger.LogMessage("Wrong file extension.");
                            break;
                    }
                }
                else
                {
                    Data = [];
                    _logger.LogMessage("File doesn't exist.");
                }
            }
            catch (Exception ex)
            {
                Data = [];
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

            _closeWindow.Execute(this);
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
