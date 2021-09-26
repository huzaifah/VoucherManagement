using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;
using Xceed.Wpf.Toolkit;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for VoucherDetailControl.xaml
    /// </summary>
    public partial class VoucherDetailControl : UserControl
    {
        public VoucherDetailControl()
        {
            InitializeComponent();
            dtDatePicker.CultureInfo = CultureInfo.InvariantCulture;
        }

        private VoucherMaintenanceViewModel _viewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (VoucherMaintenanceViewModel)this.DataContext;
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelEdit();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
        }
    }
}
