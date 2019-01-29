using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; //проверка достоверности
using System.Drawing;
using System.IO;

namespace MyProject.Models
{
    public class Equipment
    {
        [Key]
        public Guid EquipmentId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfCreation { get; set; }

        public string Name { get; set; }

        public string Producer { get; set; }

        public int Productivity { get; set; }

        public string Characteristics { get; set; }

        public byte[] imageData { get; set; }

        public Equipment()
        {
        }
    }
}