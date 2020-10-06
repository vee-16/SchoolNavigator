using Grid02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

/**
* The School Navigator
* This is the home floor controller
*
* @author  Vaishnavi Sinha
* @version 1.0
* @Date   05-01-2019 
*/

namespace Grid02.Controllers
{
    /**
   * This is the main method that defines the home controller 
   */
    public class HomeController : Controller
    {
        // Field db is used to create a new entity from the database
        private SchoolNavDBEntities db = new SchoolNavDBEntities();

        // Connection between database and program is created
        string connectionstr = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=SchoolNavDB;Integrated Security=True";

        /**
        * This method returns the home page
        * @return View
        */
        public ActionResult Index()
        {
            return View();
        }

        /**
        * This method returns the login page for admin
        * @return View
        */
        public ActionResult Administrator()
        {
            return View();
        }

        /**
        * This method gets the search input from user
        * @return RedirectToAction This returns the SearchView
        */

        [HttpGet]
        public ActionResult SearchAction(string search)
        {

            return RedirectToAction("SearchView");
         
        }


        /**
        * This method is to validate admin input to check whether duplicate entry exists in Ground floor
        * @param Row This is the first parameter to IsNameExistinG method
        * @param Col This is the second parameter to IsNameExistinG method
        * @return result This returns a boolean value, True if location already exists or False if location doesn't exist
        */

        public bool IsNameExistinG(int Row, int Col)
        {
            var result = db.Ground_Floor.Any(c => c.Row == Row && c.Col == Col);
            return result;
        }

        /**
        * This method is to validate admin input to check whether duplicate entry exists in First floor
        * @param Row This is the first parameter to IsNameExistinF method
        * @param Col This is the second parameter to IsNameExistinF method
        * @return result This returns a boolean value, True if location already exists or False if location doesn't exist
        */

        public bool IsNameExistinF(int Row, int Col)
        {
            var result = db.First_Floor.Any(c => c.Row == Row && c.Col == Col);
            return result;
        }

        /**
        * This method is to validate admin input to check whether duplicate entry exists in Second floor
        * @param Row This is the first parameter to IsNameExistinS method
        * @param Col This is the second parameter to IsNameExistinS method
        * @return result This returns a boolean value, True if location already exists or False if location doesn't exist
        */

        public bool IsNameExistinS(int Row, int Col)
        {
            var result = db.Second_Floor.Any(c => c.Row == Row && c.Col == Col);
            return result;
        }

        /**
        * This method is to validate admin input to check whether duplicate entry exists in Mapped floor
        * @param Row This is the first parameter to IsNameExistInMapped method
        * @param Col This is the second parameter to IsNameExistInMapped method
        * @return result This returns a boolean value, True if location already exists or False if location doesn't exist
        */

        public bool IsNameExistInMapped(int? Row, int? Col)
        {
            var result = db.Mapped_Floor.Any(c => c.Row == Row && c.Col == Col);
            return result;
        }

        /**
        * This method is to set the start and end location selected from drop down list
        * @return View
        */

        public ActionResult Search()
        {

            Session["SearchView"] = false;

            int i = 0;

            if (ModelState.IsValid)
            {
                i = 1;
            }
            if (i > 0)
            {
                //return RedirectToAction("SearchView");
            }

            string sql = "Select Room from Mapped_Floor";
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            #region TempData  
            List<SelectListItem> listItems = new List<SelectListItem>();


            foreach (DataRow dr in dt.Rows)
            {
                String roomName = dr[0].ToString();
                listItems.Add(new SelectListItem
                {

                    Text = roomName,
                    Value = roomName,


                });
            }

            TempData["StartLoc"] = listItems;
            TempData["EndLoc"] = listItems;
         

            #endregion

            return View();
        }

        /**
        * This method is to set the start location selected from drop down list
        * @return View
        */

        public ActionResult SearchEmergency()
        {
            Session["SearchView"] = false;

            int i = 0;
            if (ModelState.IsValid)
            {
                i = 1;
            }
            if (i > 0)
            {
                //return RedirectToAction("SearchView");
            }

            string sql = "Select Room from Mapped_Floor";
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            #region TempData  
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (DataRow dr in dt.Rows)
            {
                String roomName = dr[0].ToString();
                listItems.Add(new SelectListItem
                {
                    Text = roomName,
                    Value = roomName
                });
            }

            TempData["CurrentLoc"] = listItems;

            #endregion

            return View();
        }

        /**
        * This method handles HTTP POST request
        * @return SearchView
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search1()
        {
            return RedirectToAction("SearchView");
        }

        /**
        * This method gets the HttpRequestBase object for start and end location from database 
        * then performs the Short Path algorithm to return is as a View to the user
        * @return matrix 
        */

        public ActionResult SearchView()

        {

            int i = 0;
           
            string StartselectedValue = Request["StartLocation"];
            string EndselectedValue = Request["EndLocation"];
      


            // Get table values for start location
            string sql = "Select [Row] AS RowValue, [Col] AS ColumnValue, Nearest_stairX, Nearest_stairY, Floor from Mapped_Floor " +
            " Where Room Like  '" + StartselectedValue + "'";

            

            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }

            int StartRoomRow = 0;
            int StartRoomColumn = 0;
            int StartNearestRoomRow = 0;
            int StartNearestRoomColumn = 0;
            String StartFloor = String.Empty;
            if (dt.Rows.Count > 0)
            {
                StartRoomRow = int.Parse(dt.Rows[0]["RowValue"].ToString());
                StartRoomColumn = int.Parse(dt.Rows[0]["ColumnValue"].ToString());
                StartNearestRoomRow = int.Parse(dt.Rows[0]["Nearest_stairX"].ToString());
                StartNearestRoomColumn = int.Parse(dt.Rows[0]["Nearest_stairY"].ToString());
                StartFloor = dt.Rows[0]["Floor"].ToString();
            }

            // To get table values for end location
            sql = "Select [Row] AS RowValue, [Col] AS ColumnValue, Nearest_stairX, Nearest_stairY, Floor from Mapped_Floor " +
            " Where Room Like  '" + EndselectedValue + "'";

            dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }

            int EndRoomRow = 0;
            int EndRoomColumn = 0;
            int EndNearestRoomRow = 0;
            int EndNearestRoomColumn = 0;
            String EndFloor = String.Empty;

            if (dt.Rows.Count > 0)
            {
                EndRoomRow = int.Parse(dt.Rows[0]["RowValue"].ToString());
                EndRoomColumn = int.Parse(dt.Rows[0]["ColumnValue"].ToString());
                EndNearestRoomRow = int.Parse(dt.Rows[0]["Nearest_stairX"].ToString());
                EndNearestRoomColumn = int.Parse(dt.Rows[0]["Nearest_stairY"].ToString());
                EndFloor = dt.Rows[0]["Floor"].ToString();
            }


            // To get the values from DB
      
            ViewBag.FirstDisplay = false;
            ViewBag.SecondDisplay = false;
            ViewBag.ThirdDisplay = false;
            ViewBag.FirstDisplayTitle = "";
            ViewBag.SecondDisplayTitle = "";
            ViewBag.ThirdDisplayTitle = "";

            ViewModels.Matrix matrix = new ViewModels.Matrix();
            ViewModels.Matrix matrix1 = new ViewModels.Matrix();
            ViewModels.Matrix matrix2 = new ViewModels.Matrix();

            int r = 0;
            int c = 0;
            if (StartFloor == EndFloor)
            {
                if (StartFloor.Equals("G"))
                {
                    foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Ground Floor";
                }
                else if (StartFloor.Equals("F"))
                {
                    foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "First Floor";
                }
                else if (StartFloor.Equals("S"))
                {
                    foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Second Floor";
                }
                ViewBag.FirstDisplay = true;
            }
            else if (((StartFloor.Equals("G"))|| (StartFloor.Equals("S"))) && ((EndFloor.Equals("G")) || (EndFloor.Equals("S"))))
            {
                // The first display data
                if (StartFloor.Equals("G"))
                {
                    foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Ground Floor";
                }
                else if (StartFloor.Equals("S"))
                {
                    foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Second Floor";
                }


                // The second display data - always First Floor data
                foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix1.grid[r, c] = (int)item.Value;
                    matrix1.label[r, c] = item.Name;
                    matrix1.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.SecondDisplayTitle = "First Floor";
                // The third display data
                if (EndFloor.Equals("G"))
                {
                    foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix2.grid[r, c] = (int)item.Value;
                        matrix2.label[r, c] = item.Name;
                        matrix2.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.ThirdDisplayTitle = "Ground Floor";
                }
                else if (EndFloor.Equals("S"))
                {
                    foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix2.grid[r, c] = (int)item.Value;
                        matrix2.label[r, c] = item.Name;
                        matrix2.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.ThirdDisplayTitle = "Second Floor";
                }
                ViewBag.FirstDisplay = true;
                ViewBag.SecondDisplay = true;
                ViewBag.ThirdDisplay = true;
            }
            else
            {

                // The first display data
                if (StartFloor.Equals("G"))
                {
                    foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Ground Floor";
                }
                else if (StartFloor.Equals("F"))
                {
                    foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "First Floor";
                }
                else if (StartFloor.Equals("S"))
                {
                    foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix.grid[r, c] = (int)item.Value;
                        matrix.label[r, c] = item.Name;
                        matrix.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.FirstDisplayTitle = "Second Floor";
                }

                // The Second display data
                if (EndFloor.Equals("G"))
                {
                    foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix1.grid[r, c] = (int)item.Value;
                        matrix1.label[r, c] = item.Name;
                        matrix1.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.SecondDisplayTitle = "Ground Floor";
                }
                else if (EndFloor.Equals("F"))
                {
                    foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix1.grid[r, c] = (int)item.Value;
                        matrix1.label[r, c] = item.Name;
                        matrix1.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.SecondDisplayTitle = "First Floor";
                }
                else if (EndFloor.Equals("S"))
                {
                    foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                    {
                        r = (int)item.Row;
                        c = (int)item.Col;
                        matrix1.grid[r, c] = (int)item.Value;
                        matrix1.label[r, c] = item.Name;
                        matrix1.path[r, c] = (int)item.PathOrNot;
                    }
                    ViewBag.SecondDisplayTitle = "Second Floor";
                }

                ViewBag.FirstDisplay = true;
                ViewBag.SecondDisplay = true;
            }


            var m = new ShortPath(matrix.path);
            var n = new ShortPath(matrix1.path);
            var p = new ShortPath(matrix2.path);

         
            if (StartFloor == EndFloor)
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, EndRoomRow, EndRoomColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

            }
            else if (((StartFloor.Equals("G")) || (StartFloor.Equals("S"))) && ((EndFloor.Equals("G")) || (EndFloor.Equals("S"))))
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, StartNearestRoomRow, StartNearestRoomColumn);
                var y = n.FindPath(StartNearestRoomRow, StartNearestRoomColumn, StartNearestRoomRow, StartNearestRoomColumn);
                var z = p.FindPath(StartNearestRoomRow, StartNearestRoomColumn, EndRoomRow, EndRoomColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

                int Row1 = matrix1.grid.GetLength(0);
                int Col1 = matrix1.grid.GetLength(1);

                for (int row = 0; row < Row1; row++)
                {
                    for (int col = 0; col < Col1; col++)
                    {
                        if (y[row, col] == 7) matrix1.grid[row, col] = 7;
                    }
                }

                int Row2 = matrix2.grid.GetLength(0);
                int Col2 = matrix2.grid.GetLength(1);

                for (int row = 0; row < Row2; row++)
                {
                    for (int col = 0; col < Col2; col++)
                    {
                        if (z[row, col] == 7) matrix2.grid[row, col] = 7;
                    }
                }

            }
            else
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, StartNearestRoomRow, StartNearestRoomColumn);
                var y = n.FindPath(StartNearestRoomRow, StartNearestRoomColumn, EndRoomRow, EndRoomColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

                int Row1 = matrix1.grid.GetLength(0);
                int Col1 = matrix1.grid.GetLength(1);

                for (int row = 0; row < Row1; row++)
                {
                    for (int col = 0; col < Col1; col++)
                    {
                        if (y[row, col] == 7) matrix1.grid[row, col] = 7;
                    }
                }

            }

            ViewData["first"] = matrix;
            ViewData["second"] = matrix1;
            ViewData["third"] = matrix2;

            return View(matrix);
        }

        /**
        * This method gets the HttpRequestBase object for start location from database 
        * then performs the Short Path algorithm to return the path to nearest emergency exitis as a View to the user
        * @return matrix 
        */

        public ActionResult SearchEmergencyView()
        {

            int i = 0;

            string CurrentselectedValue = Request["CurrentLocation"];

            // Get table values for start location
            string sql = "Select [Row] AS RowValue, [Col] AS ColumnValue, Nearest_EmergencyStairX, Nearest_EmergencyStairY, Floor from Mapped_Floor " +
            " Where Room Like  '" + CurrentselectedValue + "'";

            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }

            int StartRoomRow = 0;
            int StartRoomColumn = 0;
            int NearestExitRow = 0;
            int NearestExitColumn = 0;
            String StartFloor = String.Empty;
            if (dt.Rows.Count > 0)
            {
                StartRoomRow = int.Parse(dt.Rows[0]["RowValue"].ToString());
                StartRoomColumn = int.Parse(dt.Rows[0]["ColumnValue"].ToString());
                NearestExitRow = int.Parse(dt.Rows[0]["Nearest_EmergencyStairX"].ToString());
                NearestExitColumn = int.Parse(dt.Rows[0]["Nearest_EmergencyStairY"].ToString());
                StartFloor = dt.Rows[0]["Floor"].ToString();
            }


            // To get the values from DB
            ViewBag.FirstDisplay = false;
            ViewBag.SecondDisplay = false;
            ViewBag.ThirdDisplay = false;
            ViewBag.FirstDisplayTitle = "";
            ViewBag.SecondDisplayTitle = "";
            ViewBag.ThirdDisplayTitle = "";

            ViewModels.Matrix matrix = new ViewModels.Matrix();
            ViewModels.Matrix matrix1 = new ViewModels.Matrix();
            ViewModels.Matrix matrix2 = new ViewModels.Matrix();

            int r = 0;
            int c = 0;

            if (StartFloor.Equals("S"))
            {
                // The first display data - Second Floor 
                foreach (var item in db.Second_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix.grid[r, c] = (int)item.Value;
                    matrix.label[r, c] = item.Name;
                    matrix.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.FirstDisplayTitle = "Second Floor";

                // The second display data - First Floor 
                foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix1.grid[r, c] = (int)item.Value;
                    matrix1.label[r, c] = item.Name;
                    matrix1.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.SecondDisplayTitle = "First Floor";
  
                // The third display data - Ground Floor
                foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix2.grid[r, c] = (int)item.Value;
                    matrix2.label[r, c] = item.Name;
                    matrix2.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.ThirdDisplayTitle = "Ground Floor";

                ViewBag.FirstDisplay = true;
                ViewBag.SecondDisplay = true;
                ViewBag.ThirdDisplay = true;

            }
            else if (StartFloor.Equals("F"))
            {

                // The first display data - First Floor 
                foreach (var item in db.First_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix.grid[r, c] = (int)item.Value;
                    matrix.label[r, c] = item.Name;
                    matrix.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.FirstDisplayTitle = "First Floor";

                // The second display data - Ground Floor
                foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix1.grid[r, c] = (int)item.Value;
                    matrix1.label[r, c] = item.Name;
                    matrix1.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.SecondDisplayTitle = "Ground Floor";

                ViewBag.FirstDisplay = true;
                ViewBag.SecondDisplay = true;
            }
            else if (StartFloor.Equals("G"))
            {
                // The first display data - Ground Floor
                foreach (var item in db.Ground_Floor.OrderBy(o => new { o.Row, o.Col }).ToList())
                {
                    r = (int)item.Row;
                    c = (int)item.Col;
                    matrix.grid[r, c] = (int)item.Value;
                    matrix.label[r, c] = item.Name;
                    matrix.path[r, c] = (int)item.PathOrNot;
                }
                ViewBag.FirstDisplayTitle = "Ground Floor";

                ViewBag.FirstDisplay = true;
            }

            var m = new ShortPath(matrix.path);
            var n = new ShortPath(matrix1.path);
            var p = new ShortPath(matrix2.path);

            
            if (StartFloor.Equals("S"))
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, NearestExitRow, NearestExitColumn);
                var y = n.FindPath(NearestExitRow, NearestExitColumn, NearestExitRow, NearestExitColumn);
                var z = p.FindPath(NearestExitRow, NearestExitColumn, NearestExitRow, NearestExitColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

                int Row1 = matrix1.grid.GetLength(0);
                int Col1 = matrix1.grid.GetLength(1);

                for (int row = 0; row < Row1; row++)
                {
                    for (int col = 0; col < Col1; col++)
                    {
                        if (y[row, col] == 7) matrix1.grid[row, col] = 7;
                    }
                }

                int Row2 = matrix2.grid.GetLength(0);
                int Col2 = matrix2.grid.GetLength(1);

                for (int row = 0; row < Row2; row++)
                {
                    for (int col = 0; col < Col2; col++)
                    {
                        if (z[row, col] == 7) matrix2.grid[row, col] = 7;
                    }
                }

            }
            else if (StartFloor.Equals("F"))
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, NearestExitRow, NearestExitColumn);
                var y = n.FindPath(NearestExitRow, NearestExitColumn, NearestExitRow, NearestExitColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

                int Row1 = matrix1.grid.GetLength(0);
                int Col1 = matrix1.grid.GetLength(1);

                for (int row = 0; row < Row1; row++)
                {
                    for (int col = 0; col < Col1; col++)
                    {
                        if (y[row, col] == 7) matrix1.grid[row, col] = 7;
                    }
                }

            }
            else if (StartFloor.Equals("G"))
            {
                var x = m.FindPath(StartRoomRow, StartRoomColumn, NearestExitRow, NearestExitColumn);

                int Row = matrix.grid.GetLength(0);
                int Col = matrix.grid.GetLength(1);

                for (int row = 0; row < Row; row++)
                {
                    for (int col = 0; col < Col; col++)
                    {
                        if (x[row, col] == 7) matrix.grid[row, col] = 7;
                    }
                }

            }

            ViewData["first"] = matrix;
            ViewData["second"] = matrix1;
            ViewData["third"] = matrix2;

            return View(matrix);
        }


        /**
        * This method validates the admin username and password entered by checking it against stored value in database
        * @return RedirectToAction("Index") This returns the home page of login view to admin
        */

        public ActionResult LoginView()
        {
            string usernameValue = Request["UserName"];
            string passwordValue = Request["Password"];

            // Get table values for start location
            string sql = "Select Count(ID) AS RecordCount From AdminUser " +
            " Where Name = '"+ usernameValue +"' AND password = '" + passwordValue + "'";

            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionstr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }

            int recordCount = 0;
            if (dt.Rows.Count > 0)
            {
                recordCount = int.Parse(dt.Rows[0]["RecordCount"].ToString());
            }

            if (recordCount > 0)
            {
                // Valid Admin login
                Session["UserType"] = "Admin";
                Session["errorMessage"] = "";
            }

            else
            {
                // Display error message
                Session["errorMessage"] = "Incorrect details entered. Please try again.";
                return RedirectToAction("Administrator");
            }
            return RedirectToAction("Index");
        }

       /**
       * This method removes all keys and values from the session-state collection (logs admin out of their session)
       * @return RedirectToAction("Index") This returns the home page without login view
       */

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

    }
}