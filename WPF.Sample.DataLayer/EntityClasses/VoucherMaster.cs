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
        private string _status = string.Empty;
        private string _paymentType;
        private string _expenseType;
        private string _tabungType = "Tidak Berkenaan";
        private string _chequeNo = string.Empty;
        private decimal _totalAmount;
        private decimal? _tabungAmount = 0;
        private string _amountInText = string.Empty;
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

        [NotMapped]
        public string CashChequeInfo
        {
            get
            {
                return GetPaymentTypeText();
            }
        }

        private string GetPaymentTypeText()
        {
            switch (_paymentType)
            {
                case "Cash":
                    return "Tunai";
                case "OnlineTransfer":
                    return "Online Transfer";
                case "Cheque":
                    return _chequeNo;
                default:
                    return string.Empty;
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

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        [Required(ErrorMessage = "Payment Type must be filled in.")]
        public string PaymentType
        {
            get { return _paymentType; }
            set
            {
                if (value == null)
                    return;

                _paymentType = value;
                RaisePropertyChanged("PaymentType");
                RaisePropertyChanged("CashChequeInfo");
            }
        }

        [Required(ErrorMessage = "Expense Type must be filled in.")]
        public string ExpenseType
        {
            get { return _expenseType; }
            set
            {
                if (value == null)
                    return;

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
                RaisePropertyChanged("CashChequeInfo");
            }
        }

        [Required(ErrorMessage = "Tabung must be filled in.")]
        public string TabungType
        {
            get { return _tabungType; }
            set
            {
                if (value == null)
                    return;

                _tabungType = value;
                RaisePropertyChanged("TabungType");
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

        public decimal? TabungAmount
        {
            get
            {
                return _tabungAmount;
            }
            set
            {
                _tabungAmount = value;
                RaisePropertyChanged("TabungAmount");
            }
        }

        [NotMapped]
        public string AmountInText
        {
            get
            {
                var ringgit = (int)_totalAmount;
                var cent = (int)Math.Round((_totalAmount - ringgit) * 100);

                _amountInText = NumberToWords(ringgit);

                if (cent > 0)
                    _amountInText = _amountInText.Trim() + " dan " + NumberToWords(cent) + " sen";

                return _amountInText;
            }
            set
            {
                _amountInText = value;
                RaisePropertyChanged("AmountInText");
            }
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "sifar";

            if (number < 0)
                return "negative " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " juta ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " ribu ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " ratus ";
                number %= 100;
            }

            if (number > 0)
            {
                var unitsMap = new[] { "sifar", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "lapan", "sembilan", "sepuluh", "sebelas", "dua belas", "tiga belas", "empat belas", "lima belas", "enam belas", "tujuh belas", "lapan belas", "sembilan belas" };
                var tensMap = new[] { "sifar", "sepuluh", "dua puluh", "tiga puluh", "empat puluh", "lima puluh", "enam puluh", "tujuh puluh", "lapan puluh", "sembilan puluh" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
