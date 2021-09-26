using Common.Library;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF.Sample.DataLayer.EntityClasses
{
    [Table("VoucherPaymentDetail")]
    public class VoucherPaymentDetails : CommonBase
    {
        private int _voucherPaymentId;
        private int _voucherMasterId;
        private int _rowNo;
        private string _title = string.Empty;
        private string _invoiceNo = string.Empty;
        private decimal _amount;
        private VoucherMaster _voucherMaster;

        [Required]
        [Key]
        public int VoucherPaymentId
        {
            get { return _voucherPaymentId; }
            set
            {
                _voucherPaymentId = value;
                RaisePropertyChanged("VoucherPaymentId");
            }
        }

        public int VoucherMasterId
        {
            get { return _voucherMasterId; }
            set
            {
                _voucherMasterId = value;
            }
        }

        public VoucherMaster VoucherMaster
        {
            get { return _voucherMaster; }
            set
            {
                _voucherMaster = value;
            }
        }

        public int RowNo
        {
            get { return _rowNo; }
            set
            {
                _rowNo = value;
                RaisePropertyChanged("RowNo");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("PaymentTitle");
            }
        }

        public string InvoiceNo
        {
            get { return _invoiceNo; }
            set
            {
                _invoiceNo = value;
                RaisePropertyChanged("InvoiceNo");
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged("Amount");
            }
        }
    }
}
