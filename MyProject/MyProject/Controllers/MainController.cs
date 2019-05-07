using System;
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
        public ActionResult ListOfEquipments(string type, string producer)
        {
            IQueryable<Equipment> equipments = db.Equipments;

            if (!String.IsNullOrEmpty(type) && !type.Equals("Все"))
            {
                equipments = equipments.Where(p => p.Type == type);
            }
            if (!String.IsNullOrEmpty(producer) && !producer.Equals("Все"))
            {
                equipments = equipments.Where(p => p.Producer == producer);
            }

            List<Equipment> equips = db.Equipments.ToList();
            equips.Insert(0, new Equipment { Type = "Все", Producer = "Все" });

            List<string> types = new List<string>();
            List<string> producers = new List<string>();
            foreach (var e in equips)
            {
                if (!types.Contains(e.Type)) types.Add(e.Type);
                if (!producers.Contains(e.Producer)) producers.Add(e.Producer);
            }

            EquipmentListViewModel elvm = new EquipmentListViewModel
            {
                Equipments = equipments.ToList(),
                Type = new SelectList(types),
                Producer = new SelectList(producers)
            };

            return View(elvm);
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
            List<Equipment> equips = db.Equipments.ToList();
            List<string> Types = new List<string>();
            foreach (var e in equips)
            {
                if (!Types.Contains(e.Type)) Types.Add(e.Type);
            }
            ViewBag.Equipments = Types;
            return View();
        }

        // POST: ProductionLines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProductionLine([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments, List<int> capacitys)
        {
            if (ModelState.IsValid)
            {
                productionLine.ProductionLineId = Guid.NewGuid();
                productionLine.DateOfCreation = DateTime.Now;
                productionLine.User = User.Identity.Name;

                List<string> equipmentsNotNull = new List<string>();
                foreach (string e in equipments)
                {
                    if (!String.IsNullOrEmpty(e)) equipmentsNotNull.Add(e);
                }
                productionLine.EquipmentContent = equipmentsNotNull;

                List<int> capacitysNotNull = new List<int>();
                foreach (int c in capacitys)
                {
                    if (c != 0) capacitysNotNull.Add(c);
                }
                productionLine.CapacityContent = capacitysNotNull;
                if(capacitysNotNull.Capacity != 0 && equipmentsNotNull.Capacity != 0) { 
                db.ProductionLines.Add(productionLine);
                db.SaveChanges();
                }
                return RedirectToAction("ListOfProductionLines");
            }

            return View(productionLine);
        }

        // GET: ProductLines/Edit/
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

            List<Equipment> equips = db.Equipments.ToList();
            List<string> Types = new List<string>();
            foreach (var e in equips)
            {
                if (!Types.Contains(e.Type)) Types.Add(e.Type);
            }
            ViewBag.Equipments = Types;
            return View(productLine);
        }

        // POST: ProductLines/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductLines([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments, List<int> capacitys)
        {
            if (ModelState.IsValid)
            {
                productionLine.DateOfCreation = DateTime.Now;

                List<string> equipmentsNotNull = new List<string>();
                foreach (string e in equipments)
                {
                    if (!String.IsNullOrEmpty(e)) equipmentsNotNull.Add(e);
                }
                productionLine.EquipmentContent = equipmentsNotNull;

                List<int> capacitysNotNull = new List<int>();
                foreach (int c in capacitys)
                {
                    if (c!=0) capacitysNotNull.Add(c);
                }
                productionLine.CapacityContent = capacitysNotNull;
                productionLine.CapacityContent = capacitysNotNull;

                if (capacitysNotNull.Capacity != 0 && equipmentsNotNull.Capacity != 0)
                {
                    db.Entry(productionLine).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ListOfProductionLines");
            }
            return View(productionLine);
        }

        // GET: ProductLines/Details
        public ActionResult ProductionLineDetails(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = db.ProductionLines.Find(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            return View(productionLine);
        }

        // GET: Equipments/Delete/5
        public ActionResult DeleteProductionLine(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionLine productionLine = db.ProductionLines.Find(id);
            if (productionLine == null)
            {
                return HttpNotFound();
            }
            return View(productionLine);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("DeleteProductionLine")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProductionLine(Guid id)
        {
            ProductionLine productionLine = db.ProductionLines.Find(id);
            db.ProductionLines.Remove(productionLine);
            db.SaveChanges();
            return RedirectToAction("ListOfProductionLines");
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindEquipment(string productname, string quantityname)
        {
            int quantity = Convert.ToInt32(quantityname);
            ViewBag.Productname = productname;
            ViewBag.Quantity = quantity;
            //Отдаем список продуктов для листа
            List<string> products = new List<string>();
            foreach (ProductionLine pl in db.ProductionLines)
            { products.Add(pl.Name); }
            ViewBag.Products = products;

            //получение даных с формы
            if (productname != null)
            {
                //ищем нашу линию в базе по названию продукта
                Guid id = new Guid();
                foreach (ProductionLine pl in db.ProductionLines)
                {
                    //сверяем название продукта, если наш, сохраняем идентификатор
                    if (pl.Name == productname) id = pl.ProductionLineId;
                }
                //Ищем линию по найденному идентификатору в базе
                ProductionLine prodline = db.ProductionLines.Find(id);
                //В этом списке будем сохранять отобраные идентификаторы каждого оборудования
                List<Guid> eqippmentsGuids = new List<Guid>();
                //Cписок времени работы оборудования
                List<Double> workTime = new List<double>();
                //Общее время работы линии
                double totalWorkTime = 0.0;
                //Сюда пишем ошибки подбора
                List<string> errors = new List<string>();
                //регулировка общего времени производства
                double adjustment = 3.0;
                //Желаемое время происзводства
                double wistTotalTime = 7.5;


                //обрабатываем каждый елемент списка оборудования в линии
                for (int i = 0; i < prodline.EquipmentContent.Capacity; i++)
                {
                    //балансировка текущего этапа
                    double smena8h = wistTotalTime/prodline.EquipmentContent.Capacity*i;
                    //Если производство тормозит
                    if (totalWorkTime > smena8h)
                    {
                        //повышаем регулировку (ищем мощнее оборудование)
                        if (adjustment > 1.0) { adjustment = adjustment - 0.5; }
                    }
                    
                    //временный список оборудования
                    List<Equipment> eQids = new List<Equipment>();
                    //после подбора по производительности тут перезатираем Гуйд  
                    Guid bestEquipmentId = new Guid();
                    //тут будем писать минимальную продуктивность
                    int currentMinProductivity = 0;

                    //перебераем все оборудование в базе
                    foreach (var eq in db.Equipments)
                    {
                        //если тип оборудования совпал с искомым из позиции в линии:
                        if (eq.Type == prodline.EquipmentContent.ElementAt(i))
                        {
                            //проверяем подходит ли он нам по мощности
                            if (quantity < eq.Productivity * adjustment)
                            {
                                //добавляем во временный список оборудовния 
                                eQids.Add(eq);
                                //обновим инфо о минимальной продуктивности
                                currentMinProductivity = eq.Productivity;
                                //Записываем идентификатор последнего удачного оборудования
                                bestEquipmentId = eq.EquipmentId;
                            }
                        }
                    }
                    //Пишем ошибку, если не нашли подходящего оборудования.
                    if (eQids.Count == 0)
                    {
                        int ii = i + 1;
                        errors.Add("Мы не смогли подобрать оборудования на позицию " + ii + " (" + prodline.EquipmentContent.ElementAt(i) + ")");
                    }

                    //отберем минимальную продуктивность
                    foreach (var Eqs in eQids)
                    {
                        //если выбраная меньше:
                        if (Eqs.Productivity < currentMinProductivity)
                        {
                            //записываем продуктивность
                            currentMinProductivity = Eqs.Productivity;
                            //сохраняем идентификатор такого оборудования. 
                            bestEquipmentId = Eqs.EquipmentId;
                        }
                    }
                    //отобрали лучшее оборудование для данной позиции из списка оборудований в линии. 
                    eqippmentsGuids.Add(bestEquipmentId);
                    //
                    if (currentMinProductivity != 0)
                    {
                        double tTime = (quantity * 1.0) / currentMinProductivity;
                        workTime.Add(tTime);
                        totalWorkTime += tTime;
                    }
                    //меняем обем сырья, что вышло с оборудования
                    quantity = quantity * prodline.CapacityContent.ElementAt(i) / 100;
                }

                ViewBag.Guids = eqippmentsGuids;
                ViewBag.Times = workTime;
                ViewBag.TotalWorkTime = totalWorkTime;
                ViewBag.Errors = errors;

            }
            return View(db.Equipments);
        }




        #endregion

    }
}
