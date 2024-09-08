using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using BusinessApplication.View;
using BusinessApplication.ViewModel;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Media.Media3D;

public class CustomerViewModel : INotifyPropertyChanged, IDataErrorInfo
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Address> _addressRepository;
    ILogger _logger;

    private List<Customer> _searchResults;
    private Customer? _selectedCustomer;

    private string? _searchCustomerNumber;

    private string? _customerNumber;
    private string? _firstName;
    private string? _lastName;
    private string? _email;
    private string? _website;
    private string? _passwordHash;

    private Address? _customerAddress;
    private string? _customerAddressCountry;
    private string? _customerAddressZipCode;
    private string? _customerAddressCity;
    private string? _customerAddressStreetAddress;


    public CustomerViewModel(IRepository<Customer> customerRepository, IRepository<Address> addressRepository, ILogger logger)
    {
        _customerRepository = customerRepository;
        _addressRepository = addressRepository;
        _logger = logger;

        SearchResults = _customerRepository.GetAll().ToList();

        Search = new RelayCommand(ExecuteSearch);
        ClearSelection = new RelayCommand(() => SelectedCustomer = null);
        Export = new RelayCommand(ExecuteExport);
        Import = new RelayCommand(ExecuteImport);
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
            PasswordHash = value?.PasswordHash;

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

    public string? PasswordHash
    {
        get { return _passwordHash; }
        set
        {
            _passwordHash = value;
            OnPropertyChanged(nameof(PasswordHash));
        }
    }

    public string? NewPassword
    {
        get
        {
            return null;
        }
        set
        {
            if (value != null)
            {
                PasswordHash = PasswordEncryption.HashPassword(value);
            }
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
    public ICommand Import { get; }
    public ICommand Add { get; }
    public ICommand Update { get; }
    public ICommand Remove { get; }


    private void ExecuteSearch()
    {
        SearchResults = _customerRepository.GetAllWhere(x => x.CustomerNumber.Contains(SearchCustomerNumber ?? "")).ToList();
    }

    private void ExecuteExport()
    {
        var exportWindow = new ExportView();
        exportWindow.Show();
    }

    private void ExecuteImport()
    {
        var importWindow = new ImportView();
        importWindow.Show();
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
                PasswordHash = PasswordHash
            };

            await _customerRepository.AddAsync(newCustomer);
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
            SelectedCustomer.PasswordHash = PasswordHash;

            SelectedCustomer.CustomerAddress.Country = CustomerAddressCountry;
            SelectedCustomer.CustomerAddress.ZipCode = CustomerAddressZipCode;
            SelectedCustomer.CustomerAddress.City = CustomerAddressCity;
            SelectedCustomer.CustomerAddress.StreetAddress = CustomerAddressStreetAddress;

            _customerRepository.Update(SelectedCustomer);
            ExecuteSearch();
        }
    }

    private void ExecuteRemove()
    {
        if (SelectedCustomer != null)
        {
            _customerRepository.Remove(SelectedCustomer);
            _addressRepository.Remove(SelectedCustomer.CustomerAddress);
        }
        else
        {
            _logger.LogMessage("No item selected.");
        }

        ExecuteSearch();
    }

    private bool ValidateCustomerNumber()
    {
        if (string.IsNullOrWhiteSpace(_customerNumber))
            return false;

        var regex = new Regex(@"^CU\d{5}$");
        return regex.IsMatch(_customerNumber);
    }

    private bool ValidateEmail()
    {
        if (string.IsNullOrWhiteSpace(_email))
            return false;
        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return regex.IsMatch(_email);

        /* 
         * Begrenzungen der RegEx-Validierung für E-Mail Adressen:
         * Lange Domainnamen und TLDs
         * Quoted Strings
         * Internationalisierte E-Mail-Adressen
         * Kommentare und Sonderzeichen  
         */

    }

    private bool ValidateWebsite()
    {
        if (string.IsNullOrWhiteSpace(_website))
            return false;

        var regex = new Regex(@"^(https?:\/\/)?(www\.)?[a-zA-Z0-9-]+(\.[a-zA-Z]{2,})+([\/?].*)?$");
        return regex.IsMatch(_website);
    }

    private bool ValidatePassword()
    {
        if (string.IsNullOrWhiteSpace(PasswordHash))
            return false;

        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(PasswordHash);
    }
    private bool ValidateNewPassword()
    {
        return !string.IsNullOrWhiteSpace(NewPassword) &&
               Regex.IsMatch(NewPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
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

        return (
            ValidateCustomerNumber()
            && !string.IsNullOrWhiteSpace(_firstName)
            && !string.IsNullOrWhiteSpace(_lastName)
            && ValidateEmail()
            && ValidateWebsite()
            && ValidatePassword()
            && ValidateAddressInput()
        );
    }

    private bool ValidateAddressInput()
    {
        return !string.IsNullOrWhiteSpace(CustomerAddressCountry) &&
               !string.IsNullOrWhiteSpace(CustomerAddressZipCode) &&
               !string.IsNullOrWhiteSpace(CustomerAddressCity) &&
               !string.IsNullOrWhiteSpace(CustomerAddressStreetAddress);
    }

    public string this[string columnName]
    {
        get
        {
            string error = string.Empty;

            switch (columnName)
            {
                case nameof(CustomerNumber):
                    if (!ValidateCustomerNumber())
                        error = "Customer number is invalid. Format: CU#####";
                    break;

                case nameof(Email):
                    if (!ValidateEmail())
                        error = "Email is invalid.";
                    break;

                case nameof(Website):
                    if (!ValidateWebsite())
                        error = "Website is invalid.";
                    break;

                case nameof(NewPassword):
                    if (!ValidatePassword())
                        error = "Password must be at least 8 characters, with a mix of upper/lower case letters and digits.";
                    break;

                case nameof(CustomerAddressCountry):
                case nameof(CustomerAddressZipCode):
                case nameof(CustomerAddressCity):
                case nameof(CustomerAddressStreetAddress):
                    if (!ValidateAddressInput())
                        error = "Address fields must not be empty.";
                    break;
            }

            return error;
        }
    }

    public string Error => null;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}