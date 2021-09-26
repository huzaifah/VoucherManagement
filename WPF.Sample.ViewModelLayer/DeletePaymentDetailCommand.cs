using System;
using System.Linq;
using System.Windows.Input;
using WPF.Sample.DataLayer.EntityClasses;

namespace WPF.Sample.ViewModelLayer
{
    public class DeletePaymentDetailCommand : ICommand
    {
        private readonly VoucherMaintenanceDetailViewModel _ViewModel;
        public DeletePaymentDetailCommand(VoucherMaintenanceDetailViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var paymentToDelete = parameter as VoucherPaymentDetails;

            if (paymentToDelete == null)
                return;

            VoucherPaymentDetails paymentDetail = _ViewModel.Entity.PaymentDetails.FirstOrDefault(p => p.RowNo == paymentToDelete.RowNo);

            if (paymentDetail != null)
            {
                _ViewModel.Entity.PaymentDetails.Remove(paymentDetail);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
