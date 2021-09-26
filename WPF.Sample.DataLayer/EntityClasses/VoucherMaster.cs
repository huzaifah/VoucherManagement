using Common.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WPF.Sample.DataLayer.EntityClasses
{
    [Table("VoucherMaster")]
    public class VoucherMaster : CommonBase
    {
        private int _voucherMasterId;
        private string _voucherNo = string.Empty;
        private string _recipientName = string.Empty;
        private DateTime _paymentDate;
        private DateTime _createdOn = DateTime.Now;
        private string _createdBy = string.Empty;
        private string _paymentType;
        private string _expenseType;
        private string _chequeNo = string.Empty;
        private decimal _totalAmount;
        private ICollection<VoucherPaymentDetails> _paymentDetails = new List<VoucherPaymentDetails>();
        
        [Required]
        [Key]
        public int VoucherMasterId
        {
            get { return _voucherMasterId; }
            set
            {
                _voucherMasterId = value;
                RaisePropertyChanged("VoucherMasterId");
            }
        }

        public string VoucherNo
        {
            get { return _voucherNo; }
            set
            {
                _voucherNo = value;
                RaisePropertyChanged("VoucherNo");
            }
        }

        [ForeignKey("VoucherMasterId")]
        public ICollection<VoucherPaymentDetails> PaymentDetails
        {
            get { return _paymentDetails; }
            set
            {
                _paymentDetails = value;
                RaisePropertyChanged("PaymentDetails");
            }
        }

        [Required(ErrorMessage = "Recipient Name must be filled in.")]
        public string RecipientName
        {
            get { return _recipientName; }
            set
            {
                _recipientName = value;
                RaisePropertyChanged("RecipientName");
            }
        }

        [Required(ErrorMessage = "Payment Date must be filled in.")]
        public DateTime PaymentDate
        {
            get { return _paymentDate; }
            set
            {
                _paymentDate = value;
                RaisePropertyChanged("PaymentDate");
            }
        }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                _createdOn = value;
                RaisePropertyChanged("CreatedOn");
            }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
            set
            {
                _createdBy = value;
                RaisePropertyChanged("CreatedBy");
            }
        }

        [Required(ErrorMessage = "Payment Type must be filled in.")]
        public string PaymentType
        {
            get { return _paymentType; }
            set
            {
                _paymentType = value;
                RaisePropertyChanged("PaymentType");
            }
        }

        [Required(ErrorMessage = "Expense Type must be filled in.")]
        public string ExpenseType
        {
            get { return _expenseType; }
            set
            {
                _expenseType = value;
                RaisePropertyChanged("ExpenseType");
            }
        }

        public string ChequeNo
        {
            get { return _chequeNo; }
            set
            {
                _chequeNo = value;
                RaisePropertyChanged("ChequeNo");
            }
        }

        public decimal TotalAmount
        {
            get
            {
                _totalAmount = _paymentDetails.Sum(p => p.Amount);
                return _totalAmount;
            }
            set
            {
                _totalAmount = value;
                RaisePropertyChanged("TotalAmount");
            }
        }
    }

    public enum PaymentType
    {
        Cash,
        Cheque
    }
}
