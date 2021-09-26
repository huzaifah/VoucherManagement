using Common.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WPF.Sample.DataLayer;
using WPF.Sample.DataLayer.EntityClasses;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace WPF.Sample.ViewModelLayer
{
    public class VoucherMaintenanceDetailViewModel : VoucherMaintenanceListViewModel
    {
        private VoucherMaster _entity = new VoucherMaster();
        private VoucherMaster _originalEntity = new VoucherMaster();

        public VoucherMaster Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;
                RaisePropertyChanged("Entity");
            }
        }
        
        private ICommand _DeletePaymentDetailCommand;
        public ICommand DeletePaymentDetailCommand
        {
            get => _DeletePaymentDetailCommand ?? (_DeletePaymentDetailCommand = new DeletePaymentDetailCommand(this));
            set => _DeletePaymentDetailCommand = value;
        }

        public override void LoadVouchers()
        {
            // Load all users  
            base.LoadVouchers();

            // Set default entity  
            if (Vouchers.Any() && Entity == default)
            {
                Entity = Vouchers[0];
            }
        }

        public override void BeginEdit(bool isAddMode = false)
        {
            // Create a copy in case user wants undo their changes  
            base.Clone<VoucherMaster>(Entity, _originalEntity);

            if (isAddMode)
            {
                Entity = new VoucherMaster();
                Entity.PaymentDate = DateTime.Today;
                Entity.VoucherNo = "NEWVOUCHER";
            }

            base.BeginEdit(isAddMode);
        }

        public override void CancelEdit()
        {
            base.CancelEdit();

            // Clone Original to Entity object so each RaisePropertyChanged event fires  
            base.Clone<VoucherMaster>(_originalEntity, Entity);
        }

        public override bool Save()
        {
            bool ret = false;
            SampleDbContext db = null;

            try
            {
                db = new SampleDbContext();

                if (IsAddMode)
                {
                    // Generate a random password      
                    Entity.CreatedOn = DateTime.Now;
                    Entity.CreatedBy = Environment.UserName;
                    Entity.VoucherNo = GenerateVoucherNo(Entity.PaymentDate.Month, Entity.PaymentDate.Year);

                    db.VoucherMaster.Add(Entity);
                }
                else
                {
                    var voucher = db.VoucherMaster.SingleOrDefault(v => v.VoucherMasterId == Entity.VoucherMasterId);

                    voucher.TotalAmount = Entity.TotalAmount;
                    voucher.ChequeNo = Entity.PaymentType == "Cash" ? null : Entity.ChequeNo;
                    voucher.PaymentType = Entity.PaymentType;
                    voucher.PaymentDate = Entity.PaymentDate;
                    voucher.RecipientName = Entity.RecipientName;
                    voucher.ExpenseType = Entity.ExpenseType;

                    db.Entry(voucher).State = EntityState.Modified;

                    var existingPaymentDetails =
                        db.VoucherPaymentDetails.Where(v => v.VoucherMasterId == Entity.VoucherMasterId).ToList();

                    // Delete payment details
                    foreach (var existingPaymentDetail in existingPaymentDetails)
                    {
                        if (Entity.PaymentDetails.All(c => c.VoucherPaymentId != existingPaymentDetail.VoucherPaymentId))
                            db.VoucherPaymentDetails.Remove(existingPaymentDetail);
                    }

                    // Update and Insert children
                    foreach (var paymentDetail in Entity.PaymentDetails)
                    {
                        var existingPayment = existingPaymentDetails
                            .SingleOrDefault(c => c.VoucherPaymentId == paymentDetail.VoucherPaymentId && paymentDetail.VoucherPaymentId != default(int));

                        if (existingPayment != null)
                        {
                            db.Entry(existingPayment).CurrentValues.SetValues(paymentDetail);
                            db.Entry(existingPayment).State = EntityState.Modified;
                        }
                        else
                        {
                            paymentDetail.VoucherMasterId = Entity.VoucherMasterId;
                            db.VoucherPaymentDetails.Add(paymentDetail);
                        }
                    }
                }

                db.SaveChanges();

                ret = true;

                // Set Original Entity equal to changed entity    
                _originalEntity = Entity;

                // If new entity, add to view model Users collection    
                if (IsAddMode)
                {
                    Vouchers.Add(Entity);    
                }

                // Set mode back to normal display    
                CancelEdit();
            }
            catch (DbEntityValidationException ex)
            {
                ValidationMessages = new ObservableCollection<ValidationMessage>(db.CreateValidationMessages(ex));
                IsValidationVisible = true;
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
            }

            return ret;
        }

        public override bool Delete()
        {
            bool ret = false;
            int index;
            SampleDbContext db;
            VoucherMaster entity = new VoucherMaster();

            try
            {
                db = new SampleDbContext();

                // Find entity in EF Users collection    
                entity = db.VoucherMaster.Find(Entity.VoucherMasterId);

                if (entity != null)
                {
                    // Find index where this entity is located      
                    index = db.VoucherMaster.ToList().IndexOf(entity);

                    // Remove entity from EF collection      
                    db.VoucherMaster.Remove(entity);

                    // Save changes to database      
                    db.SaveChanges();

                    ret = true;

                    // Remove from view model collection      
                    Vouchers.Remove(Entity);

                    // Calculate the selected entity after deleting      
                    if (Vouchers.Any())
                    {
                        index++;
                        if (index > Vouchers.Count)
                        {
                            index = Vouchers.Count - 1;
                        }

                        Entity = Vouchers[index];
                    }
                    else
                    {
                        Entity = null;
                    }
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
            }

            return ret;
        }

        public void GenerateInvoice()
        {
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, "App_Data");
            string outputFolder = System.IO.Path.Combine(path, "Output");
            
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string outputPath = System.IO.Path.Combine(outputFolder,
                $"{Entity.VoucherNo.Replace('/','_')}-{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.docx");

            using (var document = DocX.Load(System.IO.Path.Combine(path, "Template.docx")))
            {
                // Check if some of the replace patterns are used in the loaded document.
                if (document.FindUniqueByPattern(@"<[\w \=]{4,}>", RegexOptions.IgnoreCase).Count > 0)
                {
                    // Do the replacement of all the found tags and with green bold strings.
                    document.ReplaceText("<(.*?)>", ReplaceTextInTemplate, false, RegexOptions.IgnoreCase, new Formatting() { Bold = true, FontColor = System.Drawing.Color.Blue, Size = 12 });
                    
                    var paymentDetailsTable = document.Tables.FirstOrDefault(t => t.TableCaption == "PAYMENT_LIST");
                    if (paymentDetailsTable == null)
                    {
                        Console.WriteLine("\tError, couldn't find table with caption PAYMENT_LIST in current document.");
                    }
                    else
                    {
                        if (paymentDetailsTable.RowCount > 1)
                        {
                            // Get the row pattern of the second row.
                            var rowPattern = paymentDetailsTable.Rows[1];

                            AddItemToTable(paymentDetailsTable, rowPattern);
                            // Remove the pattern row.
                            rowPattern.Remove();
                        }
                    }

                    // Save this document to disk.
                    document.SaveAs(outputPath);
                }
            }

            var wordProcess = new Process {StartInfo = {FileName = outputPath, UseShellExecute = true}};
            wordProcess.Start();
        }

        private void AddItemToTable(Table table, Row rowPattern)
        {
            foreach (var payment in Entity.PaymentDetails)
            {
                // Insert a copy of the rowPattern at the last index in the table.
                var newItem = table.InsertRow(rowPattern, table.RowCount - 1);

                // Replace the default values of the newly inserted row.
                newItem.ReplaceText("%ROW_NO%", payment.RowNo.ToString());
                newItem.ReplaceText("%PAYMENT_TITLE%", payment.Title.ToUpper());
                newItem.ReplaceText("%INVOICE_NO%", payment.InvoiceNo.ToUpper());
                newItem.ReplaceText("%AMOUNT_1%", $"{payment.Amount:N0}");
                newItem.ReplaceText("%AMT_2%", $"{(int)(payment.Amount % 1 * 100):00}");
            }
        }

        private string ReplaceTextInTemplate(string stringToFind)
        {
            var replacePatterns = new Dictionary<string, string>()
            {
                { "VOUCHER_NO", Entity.VoucherNo },
                { "PAYMENT_DATE", Entity.PaymentDate.ToString("dd/MM/yyyy") },
                { "RECIPIENT_NAME", Entity.RecipientName.ToUpper() },
                { "PAYMENT_TYPE", Entity.PaymentType.ToUpper() == "CASH" ? "TUNAI" : $"CEK (No: {Entity.ChequeNo})" },
                { "TOTAL_AMOUNT", $"RM {Entity.TotalAmount:N}" }
            };

            if (replacePatterns.ContainsKey(stringToFind))
            {
                return replacePatterns[stringToFind];
            }
            return stringToFind;
        }

        private string GenerateVoucherNo(int monthInNumeric, int year)
        {
            var db = new SampleDbContext();

            // get vouchers from the current month
            var lastVoucherInTheGivenMonth = db.VoucherMaster.Where(v => v.PaymentDate.Month == monthInNumeric)
                .OrderByDescending(v => v.CreatedOn).FirstOrDefault();

            if (lastVoucherInTheGivenMonth == null)
                return $"BK/{year}/{monthInNumeric}/1";

            var lastNumberIssued = lastVoucherInTheGivenMonth.VoucherNo.Split('/')[3];
            var newNumber = Convert.ToInt32(lastNumberIssued) + 1;
            return $"BK/{year}/{monthInNumeric}/{newNumber}";
        }
    }
}
