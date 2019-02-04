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


        private List<String> _strings { get; set; }

        public List<string> EquipmentContent
        {
            get { return _strings; }
            set { _strings = value; }
        }

        public string StringsAsString
        {
            get { return String.Join(",", _strings); }
            set { _strings = value.Split(',').ToList(); }
        }



        public ProductionLine()
        {
        }

    }
}