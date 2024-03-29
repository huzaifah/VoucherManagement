﻿namespace Common.Library
{
    public class ViewModelAddEditDeleteBase : ViewModelBase
    {
        #region BeginEdit Method

        public virtual void BeginEdit(bool isAddMode = false)
        {
            IsListEnabled = false;
            IsDetailEnabled = true;
            IsAddMode = isAddMode;
        }

        #endregion

        #region CancelEdit Method

        public virtual void CancelEdit()
        {
            base.Clear();

            IsListEnabled = true;
            IsDetailEnabled = false;
            IsAddMode = false;
        }

        #endregion

        #region Save Method

        public virtual bool Save()
        {
            return true;
        }

        #endregion

        #region Delete Method

        public virtual bool Delete()
        {
            return true;
        }

        #endregion

        #region Private Variables

        private bool _IsListEnabled = true;
        private bool _IsDetailEnabled;
        private bool _IsAddMode;

        #endregion

        #region Public Properties

        public bool IsListEnabled
        {
            get => _IsListEnabled;
            set
            {
                _IsListEnabled = value;
                RaisePropertyChanged("IsListEnabled");
            }
        }

        public bool IsDetailEnabled
        {
            get => _IsDetailEnabled;
            set
            {
                _IsDetailEnabled = value;
                RaisePropertyChanged("IsDetailEnabled");
            }
        }

        public bool IsAddMode
        {
            get => _IsAddMode;
            set
            {
                _IsAddMode = value;
                RaisePropertyChanged("IsAddMode");
            }
        }

        #endregion
    }
}