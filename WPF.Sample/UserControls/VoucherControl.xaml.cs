﻿using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for VoucherControl.xaml
    /// </summary>
    public partial class VoucherControl : UserControl
    {
        private VoucherMaintenanceViewModel _viewModel = null;

        public VoucherControl()
        {
            InitializeComponent();

            // Connect to instance of the view model
            _viewModel = (VoucherMaintenanceViewModel)this.Resources["viewModel"];
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close(true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadVouchers();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.BeginEdit(true);
            _viewModel.IsOpen = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.BeginEdit(false);
            _viewModel.IsOpen = true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            listControl.DeleteVoucher();
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelEdit();
            _viewModel.IsOpen = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
            if (!_viewModel.IsValidationVisible)
                _viewModel.IsOpen = false;
        }

        private void GenerateSummaryButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateSummary();
        }
    }
}
