using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

public class CustomerViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Customer> _customerRepository;

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

    public CustomerViewModel(IRepository<Customer> repository)
    {
        _customerRepository = repository;
        _searchResults = _customerRepository.GetAll().ToList();

        Search = new RelayCommand(() =>
        {
            if (SearchCustomerNumber != null)
            {
                _searchResults = _customerRepository.GetAllWhere(x => x.CustomerNumber == SearchCustomerNumber).ToList();
            }
            else
            {
                _searchResults = _customerRepository.GetAll().ToList();
            }
        });
    }

    public List<Customer> SearchResults => _searchResults;

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
    public ICommand Add { get; }
    public ICommand Update { get; }
    public ICommand Remove { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}