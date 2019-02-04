using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class DbWraper
    {
        private ProjectContext db;

        public DbWraper()
        {
            db = new ProjectContext();
        }

        public void AddProductionLine(ProductionLine line)
        {

        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
