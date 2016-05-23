using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class ValueModel
    {
        [Key]
        public int ID { get; set; }
        public int DepartmentCode { get; set; }
        public int TitleCode { get; set; }
        public int DateId { get; set; }
        public int DebitKredit { get; set; }
        public double Value { get; set; }
    }
}