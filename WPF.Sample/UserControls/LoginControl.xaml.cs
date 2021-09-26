using System;
using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        // Login view model class
        private LoginViewModel _viewModel = null;

        public LoginControl()
        {
            InitializeComponent();

            _viewModel = (LoginViewModel) this.Resources["viewModel"];
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Add the Password manually because data binding does not work
            _viewModel.Entity.Password = TxtPassword.Password;
            _viewModel.Login();
        }
    }
}
