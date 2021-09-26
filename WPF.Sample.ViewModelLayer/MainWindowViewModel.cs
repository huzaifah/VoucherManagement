using System;
using System.Timers;
using Common.Library;
using WPF.Sample.DataLayer;

namespace WPF.Sample.ViewModelLayer
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Variables
        
        private string _loginMenuHeader = "Login";
        private string _statusMessage;
        private bool _isInfoMessageVisible = true;
        private string _infoMessageTitle = string.Empty;
        private string _infoMessage = string.Empty;
        private const int SECONDS = 500;
        private Timer _infoMessageTimer = null;
        private int _infoMessageTimeout = 1500;

        #endregion

        #region Public Properties

        public int InfoMessageTimeout
        {
            get { return _infoMessageTimeout; }
            set
            {
                _infoMessageTimeout = value;
                RaisePropertyChanged("InfoMessageTimeout");
            }
        }

        public bool IsInfoMessageVisible
        {
            get { return _isInfoMessageVisible; }
            set
            {
                _isInfoMessageVisible = value;
                RaisePropertyChanged("IsInfoMessageVisible");
            }
        }

        public string InfoMessage
        {
            get { return _infoMessage; }
            set
            {
                _infoMessage = value;
                RaisePropertyChanged("InfoMessage");
            }
        }

        public string InfoMessageTitle
        {
            get { return _infoMessageTitle; }
            set
            {
                _infoMessageTitle = value;
                RaisePropertyChanged("InfoMessageTitle");
            }
        }

        public string LoginMenuHeader
        {
            get { return _loginMenuHeader; }
            set
            {
                _loginMenuHeader = value;
                RaisePropertyChanged("LoginMenuHeader");
            }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged("StatusMessage");
            }
        }

        private User _UserEntity = new User();

        public User UserEntity
        {
            get { return _UserEntity; }
            set
            {
                _UserEntity = value;
                RaisePropertyChanged("UserEntity");
            }
        }


        public void LoadStateCodes()
        {
            // TODO: Write code to load state codes
            System.Threading.Thread.Sleep(SECONDS);
        }

        public void LoadCountryCodes()
        {
            // TODO: Write code to load country codes
            System.Threading.Thread.Sleep(SECONDS);
        }

        public void LoadEmployeeTypes()
        {
            // TODO: Write code to load employee types
            System.Threading.Thread.Sleep(SECONDS);
        }

        public void ClearInfoMessages()
        {
            InfoMessage = string.Empty;
            InfoMessageTitle = string.Empty;
            IsInfoMessageVisible = false;
        }

        public virtual void CreateInfoMessageTimer()
        {
            if (_infoMessageTimer == null)
            {
                // Create informational message timer
                _infoMessageTimer = new Timer(_infoMessageTimeout);

                // Connect to an Elapsed event
                _infoMessageTimer.Elapsed += MessageTimer_Elapsed;
            }

            _infoMessageTimer.AutoReset = false;
            _infoMessageTimer.Enabled = true;
            IsInfoMessageVisible = true;
        }

        private void MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsInfoMessageVisible = false;
        }

        #endregion
    }
}
