using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses_HW_5_6_Part_2.Database.Models
{
    public class Orders
    {
        [Column("ord_id")]
        public int Id { get; set; }
        [Column("ord_datetime", TypeName = "Date")]
        public DateTime Date { get; set; }
        [ForeignKey("ord_an")]
        public Analysis? Analyis { get; set; }
    }
}
