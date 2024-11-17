using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCWeb.Models;

namespace MVCWeb.Controllers
{
    public class ItemsController : Controller
    {
        private DevConn db = new DevConn();

        // GET: Items
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemID,ItemName,Size,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,ItemName,Size,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Filter()
        {
            ViewBag.Agents = new SelectList(db.Agents, "AgentID", "AgentName");
            return View();
        }

        [HttpPost]
        public ActionResult FilterResults(string startDate, string endDate,
    string agentId, string sortBy, string minPrice, string maxPrice)
        {
            var query = from od in db.OrderDetails
                        join o in db.Orders on od.OrderID equals o.OrderID
                        join i in db.Items on od.ItemID equals i.ItemID
                        join a in db.Agents on o.AgentID equals a.AgentID
                        select new OrderDetailsViewModel
                        {
                            ItemID = (int)od.ItemID,
                            ItemName = i.ItemName,
                            Size = i.Size,
                            AgentName = a.AgentName,
                            OrderDate = (DateTime)o.OrderDate,
                            Quantity = (int)od.Quantity,
                            UnitAmount = (decimal)od.UnitAmount,
                            TotalAmount = (decimal)(od.Quantity * od.UnitAmount),
                            AgentID = (int)a.AgentID
                        };

            DateTime parsedStartDate;
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                query = query.Where(x => x.OrderDate >= parsedStartDate);
            }

            DateTime parsedEndDate;
            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                query = query.Where(x => x.OrderDate <= parsedEndDate);
            }

            // Parse and apply agent filter
            int parsedAgentId;
            if (int.TryParse(agentId, out parsedAgentId))
            {
                query = query.Where(x => x.AgentID == parsedAgentId);
            }

            // Parse and apply price filters
            decimal parsedMinPrice;
            if (decimal.TryParse(minPrice, out parsedMinPrice))
            {
                query = query.Where(x => x.UnitAmount >= parsedMinPrice);
            }

            decimal parsedMaxPrice;
            if (decimal.TryParse(maxPrice, out parsedMaxPrice))
            {
                query = query.Where(x => x.UnitAmount <= parsedMaxPrice);
            }

            switch (sortBy)
            {
                case "mostPurchased":
                    query = query.OrderByDescending(x => x.Quantity);
                    break;
                case "highestPrice":
                    query = query.OrderByDescending(x => x.UnitAmount);
                    break;
                case "latest":
                    query = query.OrderByDescending(x => x.OrderDate);
                    break;
                default:
                    query = query.OrderByDescending(x => x.OrderDate);
                    break;
            }

            var results = query.ToList();
            return PartialView("_FilterResults", results);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
