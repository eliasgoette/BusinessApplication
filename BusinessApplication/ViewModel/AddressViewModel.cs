using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

public class AddressViewModel : INotifyPropertyChanged
{
    private readonly IRepository<Address> _addressRepository;

    private string? _currentAddressCountry;
    private string? _currentAddressZipCode;
    private string? _currentAddressCity;
    private string? _currentAddressStreetAddress;

    private Address? _selectedAddress;

    public AddressViewModel(IRepository<Address> repository)
    {
        _addressRepository = repository;

        ClearSelection = new RelayCommand(() => SelectedAddress = null);
        AddAddress = new RelayCommand(AddAddressExecute);
        UpdateAddress = new RelayCommand(UpdateAddressExecute);
        RemoveAddress = new RelayCommand(RemoveAddressExecute);
    }

    public List<Address> Addresses => _addressRepository.GetAll().ToList();

    public string? CurrentAddressCountry
    {
        get => _currentAddressCountry;
        set
        {
            _currentAddressCountry = value;
            OnPropertyChanged(nameof(CurrentAddressCountry));
        }
    }

    public string? CurrentAddressZipCode
    {
        get => _currentAddressZipCode;
        set
        {
            _currentAddressZipCode = value;
            OnPropertyChanged(nameof(CurrentAddressZipCode));
        }
    }

    public string? CurrentAddressCity
    {
        get => _currentAddressCity;
        set
        {
            _currentAddressCity = value;
            OnPropertyChanged(nameof(CurrentAddressCity));
        }
    }

    public string? CurrentAddressStreetAddress
    {
        get => _currentAddressStreetAddress;
        set
        {
            _currentAddressStreetAddress = value;
            OnPropertyChanged(nameof(CurrentAddressStreetAddress));
        }
    }

    public Address? SelectedAddress
    {
        get => _selectedAddress;
        set
        {
            _selectedAddress = value;
            OnPropertyChanged(nameof(SelectedAddress));

            CurrentAddressCountry = value?.Country;
            CurrentAddressZipCode = value?.ZipCode;
            CurrentAddressCity = value?.City;
            CurrentAddressStreetAddress = value?.StreetAddress;
        }
    }

    public ICommand ClearSelection { get; }
    public ICommand AddAddress { get; }
    public ICommand UpdateAddress { get; }
    public ICommand RemoveAddress { get; }

    private async void AddAddressExecute()
    {
        if (CurrentAddressIsValid())
        {
            var currentAddress = new Address
            {
                Country = _currentAddressCountry,
                ZipCode = _currentAddressZipCode,
                City = _currentAddressCity,
                StreetAddress = _currentAddressStreetAddress
            };
            await _addressRepository.AddAsync(currentAddress);
            OnPropertyChanged(nameof(Addresses));
        }
    }

    private void UpdateAddressExecute()
    {
        if (CurrentAddressIsValid() && _selectedAddress != null)
        {
            _selectedAddress.Country = _currentAddressCountry;
            _selectedAddress.ZipCode = _currentAddressZipCode;
            _selectedAddress.City = _currentAddressCity;
            _selectedAddress.StreetAddress = _currentAddressStreetAddress;

            _addressRepository.Update(_selectedAddress);
            OnPropertyChanged(nameof(Addresses));
        }
    }

    private void RemoveAddressExecute()
    {
        if (_selectedAddress != null)
        {
            _addressRepository.Remove(_selectedAddress);
            OnPropertyChanged(nameof(Addresses));
        }
    }

    private bool CurrentAddressIsValid()
    {
        return !string.IsNullOrWhiteSpace(_currentAddressCountry) &&
               !string.IsNullOrWhiteSpace(_currentAddressZipCode) &&
               !string.IsNullOrWhiteSpace(_currentAddressCity) &&
               !string.IsNullOrWhiteSpace(_currentAddressStreetAddress);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}