using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courses_HW_5_6_Part_2.Database.Models
{
    public class Analysis
    {
        [Column("an_id")]      
        public int Id { get; set; }
        [Column("an_name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("an_cost")]
        public decimal Cost { get; set; }
        [Column("an_price")]
        public decimal Price { get; set; }
        [ForeignKey("an_group")]
        public Groups? Group { get; set; }
    }
}
