using BusinessApplication;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.View;
using BusinessApplication.ViewModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;

public class CustomerViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Customer> _customerRepository;
    ILogger _logger;

    private List<Customer> _searchResults;
    private Customer? _selectedCustomer;

    private string? _searchCustomerNumber;
    private DateTime _searchTemporalDate;
    private string _searchTemporalHour;
    private string _searchTemporalMinute;
    private string _searchTemporalSecond;

    private string? _customerNumber;
    private string? _firstName;
    private string? _lastName;
    private string? _email;
    private string? _website;
    private string? _password;

    private Address? _customerAddress;
    private string? _customerAddressCountry;
    private string? _customerAddressZipCode;
    private string? _customerAddressCity;
    private string? _customerAddressStreetAddress;


    public CustomerViewModel(IRepository<Customer> repository, ILogger logger)
    {
        _customerRepository = repository;
        _logger = logger;

        SetSearchDateTimeNow();
        SearchResults = _customerRepository.GetAll().ToList();

        Search = new RelayCommand(ExecuteSearch);
        ResetFilters = new RelayCommand(ExecuteResetFilters);
        ClearSelection = new RelayCommand(() => SelectedCustomer = null);
        Export = new RelayCommand(ExecuteExport);
        Add = new RelayCommand(() => ExecuteAdd());
        Update = new RelayCommand(ExecuteUpdate);
        Remove = new RelayCommand(ExecuteRemove);
    }


    public List<Customer> SearchResults
    {
        get => _searchResults;
        set
        {
            _searchResults = value;
            OnPropertyChanged(nameof(SearchResults));
        }
    }

    public Customer? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            _selectedCustomer = value;
            OnPropertyChanged(nameof(SelectedCustomer));

            CustomerNumber = value?.CustomerNumber;
            FirstName = value?.FirstName;
            LastName = value?.LastName;
            Email = value?.Email;
            Website = value?.Website;
            Password = value?.PasswordHash;

            var address = value?.CustomerAddress;
            CustomerAddressCountry = address?.Country;
            CustomerAddressZipCode = address?.ZipCode;
            CustomerAddressCity = address?.City;
            CustomerAddressStreetAddress = address?.StreetAddress;
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

    public string? SearchCustomerNumber
    {
        get { return _searchCustomerNumber; }
        set
        {
            _searchCustomerNumber = value;
            OnPropertyChanged(nameof(SearchCustomerNumber));
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
        }
    }

    public string SearchTemporalMinute
    {
        get { return _searchTemporalMinute; }
        set
        {
            _searchTemporalMinute = value;
            OnPropertyChanged(nameof(SearchTemporalMinute));
        }
    }

    public string SearchTemporalSecond
    {
        get { return _searchTemporalSecond; }
        set
        {
            _searchTemporalSecond = value;
            OnPropertyChanged(nameof(SearchTemporalSecond));
        }
    }


    public string? CustomerNumber
    {
        get { return _customerNumber; }
        set
        {
            _customerNumber = value;
            OnPropertyChanged(nameof(CustomerNumber));
        }
    }

    public string? FirstName
    {
        get { return _firstName; }
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string? LastName
    {
        get { return _lastName; }
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string? Email
    {
        get { return _email; }
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string? Website
    {
        get { return _website; }
        set
        {
            _website = value;
            OnPropertyChanged(nameof(Website));
        }
    }

    public string? Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string? CustomerAddressCountry
    {
        get { return _customerAddressCountry; }
        set
        {
            _customerAddressCountry = value;
            OnPropertyChanged(nameof(CustomerAddressCountry));
        }
    }

    public string? CustomerAddressZipCode
    {
        get { return _customerAddressZipCode; }
        set
        {
            _customerAddressZipCode = value;
            OnPropertyChanged(nameof(CustomerAddressZipCode));
        }
    }

    public string? CustomerAddressCity
    {
        get { return _customerAddressCity; }
        set
        {
            _customerAddressCity = value;
            OnPropertyChanged(nameof(CustomerAddressCity));
        }
    }

    public string? CustomerAddressStreetAddress
    {
        get { return _customerAddressStreetAddress; }
        set
        {
            _customerAddressStreetAddress = value;
            OnPropertyChanged(nameof(CustomerAddressStreetAddress));
        }
    }

    private void UpdateCustomerAddress()
    {
        if (
            CustomerAddressCountry != null
            && CustomerAddressZipCode != null
            && CustomerAddressCity != null
            && CustomerAddressStreetAddress != null
        )
        {
            if (_customerAddress != null)
            {
                _customerAddress.Country = CustomerAddressCountry;
                _customerAddress.ZipCode = CustomerAddressZipCode;
                _customerAddress.City = CustomerAddressCity;
                _customerAddress.StreetAddress = CustomerAddressStreetAddress;
            }
            else
            {
                _customerAddress = new Address
                {
                    Country = CustomerAddressCountry,
                    ZipCode = CustomerAddressZipCode,
                    City = CustomerAddressCity,
                    StreetAddress = CustomerAddressStreetAddress
                };
            }
        }
    }


    public ICommand Search { get; }
    public ICommand ResetFilters { get; }
    public ICommand ClearSelection { get; }
    public ICommand Export { get; }
    public ICommand Add { get; }
    public ICommand Update { get; }
    public ICommand Remove { get; }


    private void ExecuteSearch()
    {
        Expression<Func<Customer, bool>> predicate;

        string temporalDateTimeString = SearchTemporalDate.Date.ToShortDateString();
        temporalDateTimeString += $", {SearchTemporalHour}:{SearchTemporalMinute}:{SearchTemporalSecond}";

        DateTime temporalDateTime = DateTime.Parse(temporalDateTimeString).ToUniversalTime();

        if (SearchCustomerNumber != null)
        {
            predicate = x => x.CustomerNumber.Contains(SearchCustomerNumber);
        }
        else
        {
            predicate = x => true;
        }

        SearchResults = _customerRepository.GetAllWhereAsOf(predicate, temporalDateTime);
    }

    private void ExecuteResetFilters()
    {
        SearchCustomerNumber = null;
        SetSearchDateTimeNow();
    }

    private void ExecuteExport()
    {
        if (_searchResults.Count <= 0) _logger.LogMessage("Nothing to export.");
        else
        {
            var exportWindow = new ExportView(_searchResults);
            exportWindow.Show();
        }
    }

    private async Task ExecuteAdd()
    {
        if (ValidateInput())
        {
            var newCustomer = new Customer
            {
                CustomerAddress = new Address
                {
                    Country = CustomerAddressCountry,
                    ZipCode = CustomerAddressZipCode,
                    City = CustomerAddressCity,
                    StreetAddress = CustomerAddressStreetAddress
                },
                CustomerNumber = CustomerNumber,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Website = Website,
                PasswordHash = Password
            };

            await _customerRepository.AddAsync(newCustomer);
            SetSearchDateTimeNow();
            ExecuteSearch();
        }
    }

    private void ExecuteUpdate()
    {
        if (ValidateInput())
        {
            SelectedCustomer.CustomerNumber = CustomerNumber;
            SelectedCustomer.FirstName = FirstName;
            SelectedCustomer.LastName = LastName;
            SelectedCustomer.Email = Email;
            SelectedCustomer.Website = Website;
            SelectedCustomer.PasswordHash = Password;

            SelectedCustomer.CustomerAddress.Country = CustomerAddressCountry;
            SelectedCustomer.CustomerAddress.ZipCode = CustomerAddressZipCode;
            SelectedCustomer.CustomerAddress.City = CustomerAddressCity;
            SelectedCustomer.CustomerAddress.StreetAddress = CustomerAddressStreetAddress;

            _customerRepository.Update(SelectedCustomer);
            SetSearchDateTimeNow();
            ExecuteSearch();
        }
    }

    private void ExecuteRemove()
    {
        if (SelectedCustomer != null)
        {
            _customerRepository.Remove(SelectedCustomer);
        }
        else
        {
            _logger.LogMessage("No item selected.");
        }

        ExecuteSearch();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(CustomerNumber))
        {
            _logger.LogMessage("Customer number must be defined.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            _logger.LogMessage("First name must be defined.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(LastName))
        {
            _logger.LogMessage("Last name must be defined.");
            return false;
        }
        if (!ValidateAddressInput())
        {
            _logger.LogMessage("Address must be completely defined.");
            return false;
        }

        return true;
    }

    private bool ValidateAddressInput()
    {
        return !string.IsNullOrWhiteSpace(CustomerAddressCountry) &&
               !string.IsNullOrWhiteSpace(CustomerAddressZipCode) &&
               !string.IsNullOrWhiteSpace(CustomerAddressCity) &&
               !string.IsNullOrWhiteSpace(CustomerAddressStreetAddress);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}