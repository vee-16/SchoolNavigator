using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grid02.Controllers;
using Grid02.Models;

/**
* The School Navigator
* This is the mapped floor controller
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/

namespace Grid02.Views
{
    /**
    * This is the main method that defines the mapped floor controller 
    */

    public class Mapped_FloorController : Controller
    {
        // Field db is used to create a new entity from the database
        private SchoolNavDBEntities db = new SchoolNavDBEntities();


        /**
       * @return View() This returns a ViewResult oject which renders a view to the response
       */
        // GET: Mapped_Floor
        public ActionResult Index()
        {
            return View(db.Mapped_Floor.ToList());
        }

        /**
        * This method is run when the admin clicks on 'Detail' while they are logged in as Adminstrator
        * @param (int? id) This is the first parameter to Details method
        * @return View(first_floor)  This returns the line of row for selected data in table
        */
        // GET: Mapped_Floor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapped_Floor mapped_Floor = db.Mapped_Floor.Find(id);
            if (mapped_Floor == null)
            {
                return HttpNotFound();
            }
            return View(mapped_Floor);
        }

        /**
        * This method is only available to admin
        * @return View() This returns a ViewResult boject which renders a view to the response
        */
        // GET: Mapped_Floor/Create
        public ActionResult Create()
        {
            return View();
        }

        /**
        * This is a Post method - the method validates the new location details entered by the admin, 
        * then adds it on to the table in the database(database is updated through website). 
        * The new location is added in the correct order.
        * The method also rejects duplicate data.
        * @param (mapped_Floor) This is the parameter to Create method
        * @return RedirectToAction("ViewAll") This is returned if all data entered is validated (or)
        *         View This is returned if data entered is invalid or duplicate
        */
        // POST: Mapped_Floor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Row,Col,Room,Nearest_stairX,Nearest_stairY,Floor,Nearest_EmergencyStairX,Nearest_EmergencyStairY")] Mapped_Floor mapped_Floor)
        {
            HomeController nameExist = new HomeController();

            if (nameExist.IsNameExistInMapped(mapped_Floor.Row, mapped_Floor.Col))
            {
                ModelState.AddModelError("Row", "This location already exists.");
                ModelState.AddModelError("Col", "This location already exists.");
                ModelState.AddModelError("Room", "This location already exists.");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Row.ToString()))
            {
                ModelState.AddModelError("Row", "Enter the Row Number");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Col.ToString()))
            {
                ModelState.AddModelError("Col", "Enter the Col Number");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Room.ToString())) 
            {
                ModelState.AddModelError("Room", "Enter the Room");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_stairX.ToString()))
            {
                ModelState.AddModelError("Nearest_stairX", "Enter row of nearest stair");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_stairY.ToString()))
            {
                ModelState.AddModelError("Nearest_stairY", "Enter column of nearest stair");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Floor.ToString()))
            {
                ModelState.AddModelError("Floor", "Enter correct floor name");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_EmergencyStairX.ToString()))
            {
                ModelState.AddModelError("Nearest_EmergencyStairX", "Enter row of nearest emergency stairs");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_EmergencyStairY.ToString()))
            {
                ModelState.AddModelError("Nearest_EmergencyStairY", "Enter column of nearest emergency stairs");
            }
            if (ModelState.IsValid)
            {
                db.Mapped_Floor.Add(mapped_Floor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mapped_Floor);
        }

        /**
        * This method is available to admin only 
        * @param (int? id) This is the first parameter to Edit method
        * @return View This is the return for selected data
        */
        // GET: Mapped_Floor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapped_Floor mapped_Floor = db.Mapped_Floor.Find(id);
            if (mapped_Floor == null)
            {
                return HttpNotFound();
            }
            return View(mapped_Floor);
        }

        /**
        * This is a Post method - the method validates the modification of location details entered by the admin, 
        * then updates the table in the database 
        * @param (mapped_Floor) This is the parameter to Edit method
        * @return RedirectToAction("ViewAll") This is returned if all data entered is validated (or) 
        *         View This is returned if data entered is invalid or duplicate
        */
        // POST: Mapped_Floor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Row,Col,Room,Nearest_stairX,Nearest_stairY,Floor,Nearest_EmergencyStairX,Nearest_EmergencyStairY")] Mapped_Floor mapped_Floor)
        {
            if (string.IsNullOrEmpty(mapped_Floor.Row.ToString()))
            {
                ModelState.AddModelError("Row", "Enter the Row Number");
            }

            if (string.IsNullOrEmpty(mapped_Floor.Col.ToString()))
            {
                ModelState.AddModelError("Col", "Enter the Col Number");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Room.ToString()))
            {
                ModelState.AddModelError("Room", "Enter the Value");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_stairX.ToString()))
            {
                ModelState.AddModelError("Nearest_stairX", "Enter the Value");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_stairY.ToString()))
            {
                ModelState.AddModelError("Nearest_stairY", "Enter the Value");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Floor.ToString()))
            {
                ModelState.AddModelError("Floor", "Enter the Value");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_EmergencyStairX.ToString()))
            {
                ModelState.AddModelError("Nearest_EmergencyStairX", "Enter the Value");
            }
            if (string.IsNullOrEmpty(mapped_Floor.Nearest_EmergencyStairY.ToString()))
            {
                ModelState.AddModelError("Nearest_EmergencyStairY", "Enter the Value");
            }
            if (ModelState.IsValid)
            {
                db.Entry(mapped_Floor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mapped_Floor);
        }


        /**
        * This method is available to admin only 
        * @param (int? id) This is the first parameter to Delete method
        * @return View This is the return for selected data
        */
        // GET: Mapped_Floor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapped_Floor mapped_Floor = db.Mapped_Floor.Find(id);
            if (mapped_Floor == null)
            {
                return HttpNotFound();
            }
            return View(mapped_Floor);
        }

        /**
        * This is a Post method - the method first checks if the admin is sure that they want to delete selected entry. 
        * The entry is then removed from table and the database is updated
        * @param (int id) This is the parameter to Delete method
        * @return RedirectToAction("ViewAll") This is returned after database is updated
        */
        // POST: Mapped_Floor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mapped_Floor mapped_Floor = db.Mapped_Floor.Find(id);
            db.Mapped_Floor.Remove(mapped_Floor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        /**
        * This method releases unmanaged resources and optinally releases managed resources.
        * @param (bool disposing) This is the parameter to Dispose method
        * @return void There is no return for this method
        */
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
