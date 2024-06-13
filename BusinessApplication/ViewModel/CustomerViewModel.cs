using BusinessApplication;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.ViewModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using System.Xml.Serialization;

public class CustomerViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Customer> _customerRepository;
    ILogger _logger;

    private List<Customer> _searchResults;
    private Customer? _selectedCustomer;

    private string? _searchCustomerNumber;
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
        _searchResults = _customerRepository.GetAll().ToList();

        Search = new RelayCommand(ExecuteSearch);
        ClearSelection = new RelayCommand(() => SelectedCustomer = null);
        ExportJson = new RelayCommand(ExecuteExportJson);
        ExportXml = new RelayCommand(ExecuteExportXml);
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

    public string? SearchCustomerNumber
    {
        get { return _searchCustomerNumber; }
        set
        {
            _searchCustomerNumber = value;
            OnPropertyChanged(nameof(SearchCustomerNumber));
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
            _customerAddressCountry != null
            && _customerAddressZipCode != null
            && _customerAddressCity != null
            && _customerAddressStreetAddress != null
        )
        {
            if (_customerAddress != null)
            {
                _customerAddress.Country = _customerAddressCountry;
                _customerAddress.ZipCode = _customerAddressZipCode;
                _customerAddress.City = _customerAddressCity;
                _customerAddress.StreetAddress = _customerAddressStreetAddress;
            }
            else
            {
                _customerAddress = new Address
                {
                    Country = _customerAddressCountry,
                    ZipCode = _customerAddressZipCode,
                    City = _customerAddressCity,
                    StreetAddress = _customerAddressStreetAddress
                };
            }
        }
    }

    public ICommand Search { get; }
    public ICommand ClearSelection { get; }
    public ICommand ExportJson { get; }
    public ICommand ExportXml { get; }
    public ICommand Add { get; }
    public ICommand Update { get; }
    public ICommand Remove { get; }

    private void ExecuteSearch()
    {
        if (SearchCustomerNumber != null)
        {
            SearchResults = _customerRepository.GetAllWhere(x => x.CustomerNumber.Contains(SearchCustomerNumber)).ToList();
        }
        else
        {
            SearchResults = _customerRepository.GetAll().ToList();
        }
    }

    private void ExecuteExportJson()
    {
        if (_searchResults.Count <= 0) _logger.LogMessage("Nothing to export.");
        else
        {
            var json = Serializer.ToJson(_searchResults);
            _logger.LogMessage(json);
        }
    }

    private void ExecuteExportXml()
    {
        if (_searchResults.Count <= 0) _logger.LogMessage("Nothing to export.");
        else
        {
            var xml = Serializer.ToXml(_searchResults);
            _logger.LogMessage(xml);
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
                    Country = _customerAddressCountry,
                    ZipCode = _customerAddressZipCode,
                    City = _customerAddressCity,
                    StreetAddress = _customerAddressStreetAddress
                },
                CustomerNumber = _customerNumber,
                FirstName = _firstName,
                LastName = _lastName,
                Email = _email,
                Website = _website,
                PasswordHash = _password
            };

            await _customerRepository.AddAsync(newCustomer);
            ExecuteSearch();
        }
    }

    private void ExecuteUpdate()
    {
        if (ValidateInput())
        {
            SelectedCustomer.CustomerNumber = _customerNumber;
            SelectedCustomer.FirstName = _firstName;
            SelectedCustomer.LastName = _lastName;
            SelectedCustomer.Email = _email;
            SelectedCustomer.Website = _website;
            SelectedCustomer.PasswordHash = _password;

            SelectedCustomer.CustomerAddress.Country = _customerAddressCountry;
            SelectedCustomer.CustomerAddress.ZipCode = _customerAddressZipCode;
            SelectedCustomer.CustomerAddress.City = _customerAddressCity;
            SelectedCustomer.CustomerAddress.StreetAddress = _customerAddressStreetAddress;

            _customerRepository.Update(SelectedCustomer);
            ExecuteSearch();
        }
    }

    private void ExecuteRemove()
    {
        if (_selectedCustomer != null)
        {
            _customerRepository.Remove(_selectedCustomer);
        }

        ExecuteSearch();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(_customerNumber))
        {
            _logger.LogError("Customer number must be defined.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(_firstName))
        {
            _logger.LogError("First name must be defined.");
            return false;
        }
        if (string.IsNullOrWhiteSpace(_lastName))
        {
            _logger.LogError("Last name must be defined.");
            return false;
        }
        if (!ValidateAddressInput())
        {
            _logger.LogError("Address must be completely defined.");
            return false;
        }

        return true;
    }

    private bool ValidateAddressInput()
    {
        return !string.IsNullOrWhiteSpace(_customerAddressCountry) &&
               !string.IsNullOrWhiteSpace(_customerAddressZipCode) &&
               !string.IsNullOrWhiteSpace(_customerAddressCity) &&
               !string.IsNullOrWhiteSpace(_customerAddressStreetAddress);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}