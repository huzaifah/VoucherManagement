using Common.Library;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WPF.Sample.DataLayer;
using WPF.Sample.DataLayer.EntityClasses;

namespace WPF.Sample.ViewModelLayer
{
    public class VoucherMaintenanceListViewModel : ViewModelAddEditDeleteBase
    {
        private ObservableCollection<VoucherMaster> _vouchers = new ObservableCollection<VoucherMaster>();

        public ObservableCollection<VoucherMaster> Vouchers
        {
            get { return _vouchers; }
            set
            {
                _vouchers = value;
                RaisePropertyChanged("Vouchers");
            }
        }

        public virtual void LoadVouchers()
        {
            SampleDbContext db = null;

            try
            {
                db = new SampleDbContext();
                Vouchers = new ObservableCollection<VoucherMaster>(db.VoucherMaster.Include(v => v.PaymentDetails).ToList());
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.Publish(ex);
            }
        }

        public void GenerateSummary()
        {
            SampleDbContext db = null;

            try
            {
                db = new SampleDbContext();
                var vouchers = db.VoucherMaster.Include(v => v.PaymentDetails).ToList();

                var summaries = vouchers.ConvertAll(v => new SummaryViewModel
                {
                    PaymentDate = v.PaymentDate.ToString("dd MMM yyyy"),
                    VoucherNo = v.VoucherNo,
                    ExpenseType = v.ExpenseType,
                    PaymentTitle = v.PaymentDetails.Any() ? v.PaymentDetails.First().Title : "",
                    Amount = v.TotalAmount.ToString("N2"),
                    RecipientName = v.RecipientName,
                    PaymentType = v.PaymentType == PaymentType.Cash.ToString() ? "TUNAI" : "CEK",
                    ChequeNo = v.PaymentType == PaymentType.Cheque.ToString() ? v.ChequeNo : ""
                });

                string path = System.IO.Path.Combine(Environment.CurrentDirectory, "Summary");
                Directory.CreateDirectory(path);
                var filePath = Path.Combine(path, $"{DateTime.Now:yyyy_MMMM_dd_hhmmss}_Summary.csv");
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteHeader(typeof(SummaryViewModel));
                    csv.WriteRecords(summaries);
                }
                Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.Publish(ex);
            }
        }

        public IList<VoucherPaymentDetails> LoadVoucherPaymentDetails(int voucherMasterId)
        {
            SampleDbContext db = null;

            try
            {
                db = new SampleDbContext();
                return db.VoucherPaymentDetails.Where(v => v.VoucherMasterId == voucherMasterId).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.Publish(ex);
            }

            return new List<VoucherPaymentDetails>();
        }
    }
}
