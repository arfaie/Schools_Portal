using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.ViewModels
{
    public class SMSViewModel
    {
        public class Return
        {
            public int Status { get; set; }
            public string Message { get; set; }
        }

        public class Entry
        {
            public int Messageid { get; set; }
            public string Message { get; set; }
            public int Status { get; set; }
            public string Statustext { get; set; }
            public string Sender { get; set; }
            public string Receptor { get; set; }
            public int Date { get; set; }
            public int Cost { get; set; }
        }

        public class RootObject
        {
            public Return Return { get; set; }
            public List<Entry> Entries { get; set; }
        }
    }
}
