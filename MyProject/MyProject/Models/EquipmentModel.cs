using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; //проверка достоверности
using System.Drawing;
using System.IO;

namespace MyProject.Models
{
    public class EquipmentModel
    {
        public Guid EquipmentId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfCreation { get; set; }

        public string Name { get; set; }

        public string Producer { get; set; }

        public string Characteristics { get; set; }

        public byte[] myImage { get; set; }

        public System.Drawing.Image GetBitmap()
        {
            using (var stream = new MemoryStream(myImage))
            {
                return System.Drawing.Image.FromStream(stream);
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public EquipmentModel(string name, string prodr, string charact, string imagePath)
        {
            this.EquipmentId = Guid.NewGuid();
            this.DateOfCreation = DateTime.Now;
            this.Name = name;
            this.Producer = prodr;
            this.Characteristics = charact;
            this.myImage = ImageToByteArray(System.Drawing.Image.FromFile(imagePath));
        }
    }
}