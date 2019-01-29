using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class MyImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
    }
}