namespace WPF.Sample.ViewModelLayer
{
    public class VoucherMaintenanceViewModel : VoucherMaintenanceDetailViewModel
    {
        private bool _IsOpen;
        public bool IsOpen
        {
            get { return _IsOpen; }
            set
            {
                if (_IsOpen == value) return;
                _IsOpen = value;
                RaisePropertyChanged("IsOpen");
            }
        }

        public VoucherMaintenanceViewModel() : base()
        {
            DisplayStatusMessage("Maintain vouchers");
        }
    }
}
