using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finance.Helpers
{
    public class ValuePostHelper
    {
        public int DateId { get; set; }
        public int DepartmentCode { get; set; }
        public int DebitKredit { get; set; }
        public List<ValuesList> ValueList { get; set; }
    }
    public class ValuesList
    {
        public int TitleCode { get; set; }
        public double Value { get; set; }
       

    }
}