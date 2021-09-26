using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Common.Library;
using WPF.Sample.DataLayer.EntityClasses;

namespace WPF.Sample.DataLayer
{
    public partial class SampleDbContext : DbContext
    {
//        public SampleDbContext() : base(@"Data Source=GAM_LAP_184\SQLEXPRESS;Initial Catalog=Voucher;Integrated Security=True")

        public SampleDbContext() : base("Samples")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }
        public virtual DbSet<VoucherMaster> VoucherMaster { get; set; }
        public virtual DbSet<VoucherPaymentDetails> VoucherPaymentDetails { get; set; }

        public List<ValidationMessage> CreateValidationMessages(DbEntityValidationException ex)
        {
            List<ValidationMessage> ret = new List<ValidationMessage>();

            // Retrieve the error messages from EF  
            foreach (DbValidationError error in ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors))
            {
                ret.Add(new ValidationMessage { Message = error.ErrorMessage, PropertyName = error.PropertyName });
            }

            return ret;
        }
    }
}
