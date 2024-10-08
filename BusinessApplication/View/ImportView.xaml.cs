﻿using Autofac;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using BusinessApplication.ViewModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace BusinessApplication.View
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class ImportView : Window
    {
        public ImportView()
        {
            InitializeComponent();
            DataContext = new ImportViewModel(App.Container.Resolve<IRepository<Customer>>(), App.Container.Resolve<ILogger>(), new RelayCommand(this.Close));
        }
    }
}
