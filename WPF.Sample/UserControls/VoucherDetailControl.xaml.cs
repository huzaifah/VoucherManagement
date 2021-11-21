using System.Globalization;
using System.Linq;
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
        
        public int VoucherId
        {
            get { return (int)GetValue(VoucherIdProperty); }
            set { SetValue(VoucherIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VoucherId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VoucherIdProperty =
            DependencyProperty.Register(nameof(VoucherId), typeof(int), typeof(VoucherDetailControl), new PropertyMetadata(0));

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (VoucherMaintenanceViewModel)DataContext;

            if (VoucherId != 0)
            {
                _viewModel.LoadVouchers();
                _viewModel.Entity = _viewModel.Vouchers.Single(v => v.VoucherMasterId == VoucherId);
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelEdit();
            CloseWindow();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
            if (!_viewModel.IsValidationVisible)
                CloseWindow();
        }

        private void CloseWindow()
        {
            var voucherDetailWindow = Window.GetWindow(this);
            voucherDetailWindow?.Close();
        }
    }
}
