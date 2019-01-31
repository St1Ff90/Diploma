using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class ProductionLine
    {
        [Key]
        public Guid ProductionLineId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfCreation { get; set; }

        public string Name { get; set; }

        public string User { get; set; }

        public LinkedList<string> EquipmentContent { get; set; }

        public ProductionLine()
        {
        }

    }
}