using System;
using System.Diagnostics;

namespace Common.Library
{
    public class ExceptionManager : CommonBase
    {
        #region Publish Methods

        public virtual void Publish(Exception ex)
        {
            // TODO: Implement an exception publisher here
            Debug.WriteLine(ex.ToString());
        }

        #endregion

        #region Instance Property

        private static ExceptionManager _Instance;

        public static ExceptionManager Instance
        {
            get
            {
                if (_Instance == null) _Instance = new ExceptionManager();

                return _Instance;
            }
            set => _Instance = value;
        }

        #endregion
    }
}