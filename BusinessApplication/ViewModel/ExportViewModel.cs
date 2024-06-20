using BusinessApplication.Model;
using BusinessApplication.Repository;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
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
        private IRepository<Customer> _repository;
        private ILogger _logger;
        private List<Customer>? _data;
        private ExportMode _selectedMode;
        private string _result = "";

        private DateTime _searchTemporalDate;
        private string _searchTemporalHour;
        private string _searchTemporalMinute;
        private string _searchTemporalSecond;

        public ExportViewModel(IRepository<Customer> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
            Data = _repository.GetAll().ToList();
            SetSearchDateTimeNow();
            SelectedMode = (int)ExportMode.Json;

            ResetFilters = new RelayCommand(ExecuteResetFilters);
            Save = new RelayCommand(ExecuteSave);
        }

        public List<Customer>? Data
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

        public string[] AvailableModes { get; } = Enum.GetNames(typeof(ExportMode));

        public int SelectedMode
        {
            get { return Convert.ToInt32(_selectedMode); }
            set
            {
                _selectedMode = (ExportMode)value;
                OnPropertyChanged(nameof(SelectedMode));
                ExecuteSearch();
                ExecuteSerialization();
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


        public DateTime SearchTemporalDate
        {
            get { return _searchTemporalDate; }
            set
            {
                _searchTemporalDate = value;
                OnPropertyChanged(nameof(SearchTemporalDate));
            }
        }

        public string[] AvailableHours { get; } = Enumerable.Range(0, 24).Select(FormatTimeSegment).ToArray();

        public string[] AvailableMinutes { get; } = Enumerable.Range(0, 60).Select(FormatTimeSegment).ToArray();

        public string[] AvailableSeconds { get; } = Enumerable.Range(0, 60).Select(FormatTimeSegment).ToArray();

        public string SearchTemporalHour
        {
            get { return _searchTemporalHour; }
            set
            {
                _searchTemporalHour = value;
                OnPropertyChanged(nameof(SearchTemporalHour));
                ExecuteSerialization();
            }
        }

        public string SearchTemporalMinute
        {
            get { return _searchTemporalMinute; }
            set
            {
                _searchTemporalMinute = value;
                OnPropertyChanged(nameof(SearchTemporalMinute));
                ExecuteSerialization();
            }
        }

        public string SearchTemporalSecond
        {
            get { return _searchTemporalSecond; }
            set
            {
                _searchTemporalSecond = value;
                OnPropertyChanged(nameof(SearchTemporalSecond));
                ExecuteSerialization();
            }
        }

        private void SetSearchDateTimeNow()
        {
            SearchTemporalDate = DateTime.Now;
            SearchTemporalHour = FormatTimeSegment(DateTime.Now.Hour);
            SearchTemporalMinute = FormatTimeSegment(DateTime.Now.Minute);
            SearchTemporalSecond = FormatTimeSegment(DateTime.Now.Second + 1);
        }

        private static string FormatTimeSegment(int input)
        {
            string output = input.ToString();

            if (input < 10)
            {
                output = "0" + output;
            }

            return output;
        }


        private void ExecuteResetFilters()
        {
            SetSearchDateTimeNow();
        }

        private void ExecuteSearch()
        {
            string temporalDateTimeString = SearchTemporalDate.Date.ToShortDateString();

            if (SearchTemporalHour != null && SearchTemporalMinute != null && SearchTemporalSecond != null)
            {
                temporalDateTimeString += $", {SearchTemporalHour}:{SearchTemporalMinute}:{SearchTemporalSecond}";

                DateTime temporalDateTime = DateTime.Parse(temporalDateTimeString).ToUniversalTime();
                Data = _repository.GetAllWhereAsOf(x => true, temporalDateTime);
            }
        }

        private void ExecuteSerialization()
        {
            ExecuteSearch();

            if (Data == null || Data?.Count <= 0)
            {
                _logger.LogMessage("Nothing to export.");
            }
            else
            {
                var mode = (ExportMode)SelectedMode;

                if (mode == ExportMode.Json)
                {
                    Result = CustomerSerializer.ToJson(Data);
                }
                else
                {
                    Result = CustomerSerializer.ToXml(Data);
                }
            }
        }

        private void ExecuteSave()
        {
            try
            {
                var fileTypeFilter = ((ExportMode)SelectedMode == ExportMode.Json) ? "JSON files (*.json)|*.json" : "XML files(*.xml) | *.xml";

                var dialog = new SaveFileDialog
                {
                    FileName = "DataExport",
                    DefaultExt = SelectedMode.ToString().ToLower(),
                    Filter = fileTypeFilter
                };

                bool result = dialog.ShowDialog() ?? false;

                if (result)
                {
                    string filename = dialog.FileName;
                    File.WriteAllText(filename, Result);

                    // TODO: Close window
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public ICommand ResetFilters { get; }
        public ICommand Save { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
