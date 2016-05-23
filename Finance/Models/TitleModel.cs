using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Finance.Models
{
    public class TitleModel
    {
        [Key] 
        public int ID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]      
        [Display(Name = "Title")]
        public string Title { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public string type { get; set; }
        public int typeInt { get; set; }
    }
}