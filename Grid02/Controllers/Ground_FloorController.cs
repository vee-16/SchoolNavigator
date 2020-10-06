using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Grid02.Models;

/**
* The School Navigator
* This is the ground floor controller
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/

namespace Grid02.Controllers
{

    /**
    * This is the main method that defines the ground floor controller 
    */

    public class Ground_FloorController : Controller
    {

        // Field db is used to create a new entity from the database
        private SchoolNavDBEntities db = new SchoolNavDBEntities();

        /**
        * This method is only available to the admin
        * @return View This returns an ordered list from the table in the database(from entity db created) to the View function
        */
        // ViewAll
        public ActionResult ViewAll()
        {
            return View(db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList());
        }


        /**
        * @return View() This returns a ViewResult oject which renders a view to the response
        */
        // GET: Ground_Floor
        public ActionResult Index()
        {
            return View();
        }

        /**
        * This method is run when the admin clicks on 'Detail' while they are logged in as Adminstrator
        * @param (int? id) This is the first parameter to Details method
        * @return View(ground_floor)  This returns the line of row for selected data in table
        */
        // GET: Ground_Floor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ground_Floor ground_Floor = db.Ground_Floor.Find(id);
            if (ground_Floor == null)
            {
                return HttpNotFound();
            }
            return View(ground_Floor);
        }


        /**
        * This method is only available to admin
        * @return View() This returns a ViewResult boject which renders a view to the response
        */
        // GET: Ground_Floor/Create
        public ActionResult Create()
        {
            return View();
        }


        /**
        * This is a Post method - the method validates the new location details entered by the admin, 
        * then adds it on to the table in the database(database is updated through website). 
        * The new location is added in the correct order.
        * The method also rejects duplicate data.
        * @param (ground_Floor) This is the parameter to Create method
        * @return RedirectToAction("ViewAll") This is returned if all data entered is validated (or)
        *         View This is returned if data entered is invalid or duplicate
        */

        // POST: Ground_Floor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "ID,Row,Col,Value,Name,PathOrNot")] Ground_Floor ground_Floor)
        {
            HomeController nameExist = new HomeController();

            if (nameExist.IsNameExistinG(ground_Floor.Row, ground_Floor.Col))
            {
                ModelState.AddModelError("Row", "This location already exists.");
                ModelState.AddModelError("Col", "This location already exists.");
            }

            if (string.IsNullOrEmpty(ground_Floor.Row.ToString()))
            {
                ModelState.AddModelError("Row", "Enter the Row Number");
            }

            if (string.IsNullOrEmpty(ground_Floor.Col.ToString()))
            {
                ModelState.AddModelError("Col", "Enter the Col Number");
            }
            if (string.IsNullOrEmpty(ground_Floor.Value.ToString()))
            {
                ModelState.AddModelError("Value", "Enter the Value");
            }
            if (string.IsNullOrEmpty(ground_Floor.PathOrNot.ToString()))
            {
                ModelState.AddModelError("Value", "Enter the Value");
            }
            
            {
                ModelState.AddModelError("Value", "Enter the Value");
            }

            if (ModelState.IsValid)
            {
                
                db.Ground_Floor.Add(ground_Floor);
                db.SaveChanges();
                return RedirectToAction("ViewAll");
            }

            return View(ground_Floor);
        }

        /**
        * This method is available to admin only 
        * @param (int? id) This is the first parameter to Edit method
        * @return View This is the return for selected data
        */
        // GET: Ground_Floor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ground_Floor ground_Floor = db.Ground_Floor.Find(id);
            if (ground_Floor == null)
            {
                return HttpNotFound();
            }
            return View(ground_Floor);
        }


        /**
        * This is a Post method - the method validates the modification of location details 
        * 
        * 
        * ed by the admin, 
        * then updates the table in the database 
        * @param (ground_Floor) This is the parameter to Edit method
        * @return RedirectToAction("ViewAll") This is returned if all data entered is validated (or) 
        *         View This is returned if data entered is invalid or duplicate
        */
        // POST: Ground_Floor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Row,Col,Value,Name,PathOrNot")] Ground_Floor ground_Floor)
        {
         

            if (string.IsNullOrEmpty(ground_Floor.Row.ToString()))
            {
                ModelState.AddModelError("Row", "Enter the Row Number");
            }

            if (string.IsNullOrEmpty(ground_Floor.Col.ToString()))
            {
                ModelState.AddModelError("Col", "Enter the Col Number");
            }
            if (string.IsNullOrEmpty(ground_Floor.Value.ToString()))
            {
                ModelState.AddModelError("Value", "Enter the Value");
            }
            if (string.IsNullOrEmpty(ground_Floor.PathOrNot.ToString()))
            {
                ModelState.AddModelError("Value", "Enter the Value");
            }
   
                    
            if (ModelState.IsValid)
            {
                db.Entry(ground_Floor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAll");
            }
            return View(ground_Floor);
        }


        /**
        * This method is available to admin only 
        * @param (int? id) This is the first parameter to Delete method
        * @return View This is the return for selected data
        */
        // GET: Ground_Floor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ground_Floor ground_Floor = db.Ground_Floor.Find(id);
            if (ground_Floor == null)
            {
                return HttpNotFound();
            }
            return View(ground_Floor);
        }


        /**
        * This is a Post method - the method first checks if the admin is sure that they want to delete selected entry. 
        * The entry is then removed from table and the database is updated.
        * @param (int id) This is the parameter to Delete method
        * @return RedirectToAction("ViewAll") This is returned after database is updated
        */

        // POST: Ground_Floor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ground_Floor ground_Floor = db.Ground_Floor.Find(id);
            db.Ground_Floor.Remove(ground_Floor);
            db.SaveChanges();
            return RedirectToAction("ViewAll");
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
