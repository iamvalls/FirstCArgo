using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstCargoApp.Helper
{
    public class NotificationMessage
    {
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error,
            RecordSuccess,
            EditRecordSuccess,
            DeleteRecordSuccess,
            NoEntryFound,
            PrintOrderSuccess
        }
    }
}