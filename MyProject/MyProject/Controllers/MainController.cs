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
        public ActionResult CreateProductionLine([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments, List<int> capacitys, List<double> delays)
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

                int itemsCount = equipmentsNotNull.Count;
                List<int> capacitysNotNull = new List<int>();
                List<double> delaysAll = new List<double>();

                for (int i = 0; i < itemsCount; i++)
                {
                    if (capacitys.ElementAt(i) != 0) capacitysNotNull.Add(capacitys.ElementAt(i));
                }

                for (int i = 0; i < itemsCount; i++)
                {
                    delaysAll.Add(delays.ElementAt(i));
                }


                productionLine.CapacityContent = capacitysNotNull;
                productionLine.DelayContent = delaysAll;

                if (capacitysNotNull.Count != 0 && equipmentsNotNull.Count != 0 && delaysAll.Count != 0) { 
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
        public ActionResult EditProductLines([Bind(Include = "ProductionLineId,DateOfCreation,Name,User,EquipmentContent")] ProductionLine productionLine, List<string> equipments, List<int> capacitys, List<double> delays)
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

                int itemsCount = equipmentsNotNull.Count;
                List<int> capacitysNotNull = new List<int>();
                List<double> delaysAll = new List<double>();

                for (int i = 0; i < itemsCount; i++)
                {
                    if (capacitys.ElementAt(i) != 0) capacitysNotNull.Add(capacitys.ElementAt(i));
                }

                for (int i = 0; i < itemsCount; i++)
                {
                    delaysAll.Add(delays.ElementAt(i));
                }

                
                productionLine.CapacityContent = capacitysNotNull;
                productionLine.DelayContent = delaysAll;


                if (capacitysNotNull.Count != 0 && equipmentsNotNull.Count != 0)
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

        public ActionResult FindEquipment(string productname, string quantityname, string adj)
        {
            int quantityOfMilk = Convert.ToInt32(quantityname);

            ViewBag.Adj = adj;
            ViewBag.Productname = productname;
            ViewBag.Quantity = quantityOfMilk;
            //тут будем писать минимальную продуктивность
            int currentMinProductivity = 0;
            int previousEqProductivity = 0;
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

                List<int> curEqProductivity = new List<int>();
                //Cписок времени работы оборудования
                List<Double> workTimeForEachEq = new List<double>();

                List<Double> delaysList = new List<double>();
                //Общее время работы линии
                double totalWorkTime = 0.75;
                //Сюда пишем ошибки подбора
                List<string> errors = new List<string>();
                //регулировка общего времени производства
                
                double adjustment = 1.0;
                if (adj != null) { 
                if (adj == "Высокая") adjustment = 1.0;
                if (adj == "Средняя") adjustment = 2.0;
                if (adj == "Низкая") adjustment = 3.0;
                }



                double timeOfPreviousEqWork = 0.0;
                //обрабатываем каждый елемент списка оборудования в линии
                for (int i = 0; i < prodline.EquipmentContent.Capacity; i++)
                {                   
                    //временный список оборудования
                    List<Equipment> equipmentsOfOneType = new List<Equipment>();
                    //после подбора по производительности тут перезатираем Гуйд  
                    Guid bestEquipmentId = new Guid();
                    int productivityOnTheStageShoulBe = 0;
                    if (productivityOnTheStageShoulBe == 0) productivityOnTheStageShoulBe = Convert.ToInt32(Convert.ToDouble(quantityOfMilk)/adjustment);
                    //перебераем все оборудование в базе
                    foreach (var eq in db.Equipments)
                    {
                        //если тип оборудования совпал с искомым из позиции в линии:
                        if (eq.Type == prodline.EquipmentContent.ElementAt(i))
                        {
                           //если это первый подбор, будем отсчитывать от входяжено сырья

                           //проверяем подходит ли он нам по мощности
                            if (productivityOnTheStageShoulBe <= eq.Productivity)
                            {
                                //добавляем во временный список оборудовния 
                                equipmentsOfOneType.Add(eq);
                                //обновим инфо о минимальной продуктивности
                                currentMinProductivity = eq.Productivity;
                                
                                //Записываем идентификатор последнего удачного оборудования
                                bestEquipmentId = eq.EquipmentId;
                            }
                        }
                    }
                    //Пишем ошибку, если не нашли подходящего оборудования.
                    if (equipmentsOfOneType.Count == 0)
                    {
                        int ii = i + 1;
                        errors.Add("Мы не смогли подобрать оборудования на позицию " + ii + " (" + prodline.EquipmentContent.ElementAt(i) + ")");
                    }
                    //отберем минимальную продуктивность
                    foreach (var Eqs in equipmentsOfOneType)
                    {
                        //если выбраная меньше:
                        if (Eqs.Productivity < currentMinProductivity)
                        {
                            //записываем продуктивность
                            currentMinProductivity = Eqs.Productivity;
                            //записываем id
                            bestEquipmentId = Eqs.EquipmentId;
                        }
                    }
                    //отобрали лучшее оборудование для данной позиции из списка оборудований в линии. 
                    eqippmentsGuids.Add(bestEquipmentId);
                    if (previousEqProductivity == 0) previousEqProductivity = currentMinProductivity;
                    if (currentMinProductivity > previousEqProductivity)
                    {
                        currentMinProductivity = previousEqProductivity;
                    }
                    if (previousEqProductivity > currentMinProductivity)
                    { 
                    previousEqProductivity = currentMinProductivity;
                    }
                    int exactProductivity = Convert.ToInt32(Convert.ToDouble(quantityOfMilk) / adjustment);
                    curEqProductivity.Add(exactProductivity);
                    //будем считать время работы оборудования
                    if (equipmentsOfOneType.Count != 0)
                    {
                        double tTime = (quantityOfMilk * 1.0) / exactProductivity;
                        double temtWorkTime = tTime + (prodline.DelayContent.ElementAt(i) / 60);
                        workTimeForEachEq.Add(temtWorkTime);
                        delaysList.Add(prodline.DelayContent.ElementAt(i) / 60);
                        totalWorkTime += temtWorkTime - timeOfPreviousEqWork;
                        timeOfPreviousEqWork = tTime;
                    }
                    //меняем обем сырья, что вышло с оборудования
                    quantityOfMilk = quantityOfMilk * prodline.CapacityContent.ElementAt(i) / 100;
                }
                ViewBag.Guids = eqippmentsGuids;
                ViewBag.Times = workTimeForEachEq;
                ViewBag.TotalWorkTime = totalWorkTime;
                ViewBag.Errors = errors;
                ViewBag.CurEqProductivity = curEqProductivity;
                ViewBag.DelaysList = delaysList;
            }
            return View(db.Equipments);
        }
        #endregion

    }
}
