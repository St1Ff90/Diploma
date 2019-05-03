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


        [Display(Name = "Продукт")]
        public string Name { get; set; }
        [Display(Name = "Кто добавил")]
        public string User { get; set; }


        private List<String> _strings { get; set; }
        private List<int> _ints { get; set; }

        public List<string> EquipmentContent
        {
            get { return _strings; }
            set { _strings = value; }
        }

        public List<int> CapacityContent
        {
            get { return _ints; }
            set { _ints = value; }
        }

        public string StringsAsString
        {
            get { return String.Join(",", _strings); }
            set { _strings = value.Split(',').ToList(); }
        }

        public string IntsAsString
        {
            get { return String.Join(",", _ints); }
            set { _ints = value.Split(',').Select(Int32.Parse).ToList(); }
        }


        public ProductionLine()
        {
        }

    }
}