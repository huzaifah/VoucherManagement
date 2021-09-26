using System;
using System.Linq;
using Common.Library;
using WPF.Sample.DataLayer;

namespace WPF.Sample.ViewModelLayer
{
    public class LoginViewModel : ViewModelBase
    {
        private User _Entity;

        public User Entity
        {
            get { return _Entity; }
            set
            {
                _Entity = value;
                RaisePropertyChanged("Entity");
            }
        }

        public LoginViewModel() : base()
        {
            DisplayStatusMessage("Login to Application");
            Entity = new User { UserName = Environment.UserName };
        }

        public bool Login()
        {
            if (!Validate())
            {
                return false;
            }

            // Check Credentials in User Table
            if (!ValidateCredentials())
            {
                return false;
            }

            // Mark as logged in
            Entity.IsLoggedIn = true;
            // Send message that login was successful
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.LOGIN_SUCCESS, Entity);
            // Close the user control
            Close(false);
            return true;
        }

        public bool ValidateCredentials()
        {
            bool ret = false;
            SampleDbContext db = null;

            try
            {
                db = new SampleDbContext();

                if (db.Users.Any(u => u.UserName == Entity.UserName))
                {
                    Environment.SetEnvironmentVariable("UserName", Entity.UserName);
                    ret = true;
                }
                else
                {
                    AddValidationMessage("LoginFailed", "Invalid User Name and/or Password.");
                }
            }

            catch (Exception ex)
            {
                PublishException(ex);
            }

            return ret;
        }



        public override void Close(bool wasCancelled = true)
        {
            if (wasCancelled)
            {
                // Display Informational Message
                MessageBroker.Instance.SendMessage(MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE, "User NOT Logged In.");
            }
            base.Close(wasCancelled);
        }

        public bool Validate()
        {
            bool ret = false;

            Entity.IsLoggedIn = false;
            ValidationMessages.Clear();

            if (string.IsNullOrEmpty(Entity.UserName))
            {
                AddValidationMessage("UserName", "User Name Must Be Filled In");
            }
            if (string.IsNullOrEmpty(Entity.Password))
            {
                AddValidationMessage("Password", "Password Must Be Filled In");
            }

            ret = ValidationMessages.Count == 0;
            return ret;
        }

    }
}
