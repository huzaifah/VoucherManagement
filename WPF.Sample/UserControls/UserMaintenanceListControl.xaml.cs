using System;
using System.Windows;
using System.Windows.Controls;
using WPF.Sample.DataLayer;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for UserMaintenanceListControl.xaml
    /// </summary>
    public partial class UserMaintenanceListControl : UserControl
    {
        public UserMaintenanceListControl()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Set selected item
            _viewModel.Entity = (User)((Button)sender).Tag;

            // Delete user
            DeleteUser();
        }

        public void DeleteUser()
        {
            // Ask if the user wants to delete this user  
            if (MessageBox.Show("Delete User " + _viewModel.Entity.LastName + ", " + _viewModel.Entity.FirstName + "?", "Delete?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _viewModel.Delete();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Set selected item
            _viewModel.Entity = (User)((Button)sender).Tag;

            // Go into Edit mode  
            _viewModel.BeginEdit(false);
        }

        private UserMaintenanceViewModel _viewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (UserMaintenanceViewModel)this.DataContext;
        }
    }
}
