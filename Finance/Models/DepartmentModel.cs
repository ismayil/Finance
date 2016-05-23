using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class DepartmentModel
    {        
        [Key]
        public int ID { get; set; }
        [Display(Name = "Department Code")]
        public int DepartmentCode { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

    }
}