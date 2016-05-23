using System.ComponentModel.DataAnnotations;

namespace Finance.Models
{
    public class DateModel
    {
        [Key]
        public int ID { get; set; }
        public string Dates { get; set; }
        public string Description { get; set; }    
    }
}