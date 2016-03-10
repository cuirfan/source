using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Utils.Classes
{
    public class MyResult
    {
        public int MessageCode { get; set; }
        public bool IsTrue { get; set; }
        public string Message { get; set; }
        public string MessageCaption { get; set; }
        public EventLogEntryType MessageIcon { get; set; }

        public MyResult()
        {
            // 1111 for not found OrderItemAttributes
            MessageCode = 0; 
            IsTrue = true;
            Message = "";
            MessageCaption = "GPOOH";
            MessageIcon = System.Diagnostics.EventLogEntryType.Information;
        }
    }
}
