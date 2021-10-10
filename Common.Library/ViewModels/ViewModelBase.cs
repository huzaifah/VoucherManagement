using System;
using System.Collections.ObjectModel;

namespace Common.Library
{
    public class ViewModelBase : CommonBase
    {
        #region AddBusinessRuleMessage Method

        public virtual void AddValidationMessage(string propertyName, string msg)
        {
            _ValidationMessages.Add(new ValidationMessage {Message = msg, PropertyName = propertyName});
            IsValidationVisible = true;
        }

        #endregion

        #region Clear Method

        public virtual void Clear()
        {
            ValidationMessages.Clear();
            IsValidationVisible = false;
        }

        #endregion

        #region DisplayStatusMessage Method

        public virtual void DisplayStatusMessage(string msg = "")
        {
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.DISPLAY_STATUS_MESSAGE, msg);
        }

        #endregion

        #region PublishException Method

        public void PublishException(Exception ex)
        {
            // Publish Exception
            ExceptionManager.Instance.Publish(ex);
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.SHOW_ERROR_MESSAGE, ex.ToString());
        }

        #endregion

        #region Close Method

        public virtual void Close(bool wasCancelled = true)
        {
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.CLOSE_USER_CONTROL, wasCancelled);
        }

        #endregion

        #region Dispose Method

        public virtual void Dispose()
        {
        }

        #endregion

        #region Private Variables

        private ObservableCollection<ValidationMessage> _ValidationMessages =
            new ObservableCollection<ValidationMessage>();

        private bool _IsValidationVisible;

        #endregion

        #region Public Properties

        public ObservableCollection<ValidationMessage> ValidationMessages
        {
            get => _ValidationMessages;
            set
            {
                _ValidationMessages = value;
                RaisePropertyChanged("ValidationMessages");
            }
        }

        public bool IsValidationVisible
        {
            get => _IsValidationVisible;
            set
            {
                _IsValidationVisible = value;
                RaisePropertyChanged("IsValidationVisible");
            }
        }

        #endregion
    }
}