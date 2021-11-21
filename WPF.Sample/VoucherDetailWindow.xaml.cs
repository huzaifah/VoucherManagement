using System.Windows;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample
{
    /// <summary>
    /// Interaction logic for VoucherDetailWindow.xaml
    /// </summary>
    public partial class VoucherDetailWindow
    {
        private VoucherMaintenanceViewModel _viewModel;

        public VoucherDetailWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (VoucherMaintenanceViewModel)DataContext;
        }

        private void VoucherDetailWindow_Closed(object sender, System.EventArgs e)
        {
            _viewModel.LoadVouchers();
        }
    }
}
