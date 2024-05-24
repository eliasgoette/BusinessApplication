using BusinessApplication.View;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace BusinessApplication.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private CustomerView _customerView = new CustomerView();
        private ArticleView _articleView = new ArticleView();
        private UserControl _currentView;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            CurrentView = _customerView;
            SetCustomerView = new RelayCommand(() => CurrentView = _customerView);
            SetArticleView = new RelayCommand(() => CurrentView = _articleView);
        }

        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
            }
        }

        public ICommand SetCustomerView { get; }
        public ICommand SetArticleView { get; }
    }
}
