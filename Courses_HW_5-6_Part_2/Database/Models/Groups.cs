using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses_HW_5_6_Part_2.Database.Models
{
    public class Groups
    {
        [Column("gr_id")]
        public int Id { get; set; }
        [Column("gr_name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("gr_temp")]
        public decimal Temp { get; set; }
    }
}
