using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;

/**
* The School Navigator
* This is the search controller
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/

namespace Grid02.Controllers
{
    /**
    * This is the main method that defines the search controller 
    */

    public class SearchController : Controller
    {

        /**
        * @return View() This returns a ViewResult oject which renders a view to the response
        */
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }


        /**
        * This method is executed when the search button is clicked by the user
        * @return RedirectToAction This returns the SearchView
        */

        public ActionResult SearchView()
        {
            return RedirectToAction("SearchView");
        }

        }
    }
