using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class LockStatusModel
    {
        [Key]
        public int ID { get; set; }
        public bool LocalStatus { get; set; }
        public bool RemoteStatus { get; set; }
        public int DepartmentCode { get; set; }
        public int DateId { get; set; }
    }
}