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

        [Display(Name = "Дата добавления")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfCreation { get; set; }

        [Display(Name = "Марка")]
        public string Name { get; set; }

        [Display(Name = "Производитель")]
        public string Producer { get; set; }

        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Display(Name = "Производительность")]
        public int Productivity { get; set; }

        [Display(Name = "Характеристики")]
        public string Characteristics { get; set; }

        [Display(Name = "Картинка")]
        public byte[] imageData { get; set; }

        public Equipment()
        {
        }
    }
}