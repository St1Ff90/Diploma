﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyProject.Models;
using System.IO;

namespace MyProject.Controllers
{
    public class MainController : Controller
    {
        private ProjectContext db = new ProjectContext();

        #region Equipment
        // GET: Equipments
        public ActionResult ListOfEquipments()
        {
            return View(db.Equipments);
        }

        // GET: Equipments/Details/5
        public ActionResult EquipmentDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Create
        public ActionResult CreateEquipment()
        {
            return View();
        }

        // GET: Equipments/Edit/5
        public ActionResult EditEquipment(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public ActionResult DeleteEquipment(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipments.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEquipment([Bind(Include = "EquipmentId,DateOfCreation,Name,Producer,Productivity,Characteristics,imageData,Type")] Equipment equipment, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    equipment.imageData = imageData;
                }
                equipment.EquipmentId = Guid.NewGuid();
                equipment.DateOfCreation = DateTime.Now;
                db.Equipments.Add(equipment);
                db.SaveChanges();
                return RedirectToAction("ListOfEquipments");
            }
            return View(equipment);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEquipment([Bind(Include = "EquipmentId,DateOfCreation,Name,Producer,Productivity,Characteristics,imageData,Type")] Equipment equipment, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                equipment.DateOfCreation = DateTime.Now;

                if (uploadImage != null)  //Если добавили или обновили картинку
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    equipment.imageData = imageData; //запись массива битов
                }

                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListOfEquipments");
            }
            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("DeleteEquipment")]
        [ValidateAntiForgeryToken] 
        public ActionResult DeleteConfirmed(Guid id)
        {
            Equipment equipment = db.Equipments.Find(id);
            db.Equipments.Remove(equipment);
            db.SaveChanges();
            return RedirectToAction("ListOfEquipments");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ProductionLine
        //List of Production Lines
        public ActionResult ListOfProductionLines()
        {
            return View(db.ProductionLines);
        }
        
        //GET Create new Line 
        public ActionResult CreateProductionLine()
        {
            return View();
        }

        // POST: ProductionLines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProductionLine([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments)
        {
            if (ModelState.IsValid)
            {
                productionLine.ProductionLineId = Guid.NewGuid();
                productionLine.DateOfCreation = DateTime.Now;
                productionLine.EquipmentContent = equipments;
                db.ProductionLines.Add(productionLine);
                db.SaveChanges();
                return RedirectToAction("ListOfProductionLines");
            }

            return View(productionLine);
        }

        // GET: ProductLines/Edit/5
        public ActionResult EditProductLines(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productLine = db.ProductionLines.Find(id);
            if (productLine == null)
            {
                return HttpNotFound();
            }
            return View(productLine);
        }

        // POST: ProductLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductLines([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments)
        {
            if (ModelState.IsValid)
            {
                productionLine.DateOfCreation = DateTime.Now;
                productionLine.EquipmentContent = equipments;
               

                db.Entry(productionLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListOfProductionLines");
            }
            return View(productionLine);
        }

        #endregion

    }
}
