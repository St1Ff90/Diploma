using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations; //проверка достоверности


namespace MyProject.Models
{
    public class EquipmentListViewModel
    {
        public IEnumerable<Equipment> Equipments { get; set; }

        public SelectList Type { get; set; }
        public SelectList Producer { get; set; }
        public SelectList Date { get; set; }

    }
}