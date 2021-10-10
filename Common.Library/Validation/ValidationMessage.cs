namespace Common.Library
{
    public class ValidationMessage : CommonBase
    {
        #region Private Properties

        private string _PropertyName;
        private string _Message;

        #endregion

        #region Public Properties

        public string PropertyName
        {
            get => _PropertyName;
            set
            {
                _PropertyName = value;
                RaisePropertyChanged("PropertyName");
            }
        }

        public string Message
        {
            get => _Message;
            set
            {
                _Message = value;
                RaisePropertyChanged("Message");
            }
        }

        #endregion
    }
}