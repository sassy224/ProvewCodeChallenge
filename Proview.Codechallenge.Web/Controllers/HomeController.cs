using Proview.CodeChallenge.BLL;
using Proview.CodeChallenge.BLL.ControlObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proview.CodeChallenge.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Do nothing here because the display data will be loaded by another ajax call
            return View();
        }

        /// <summary>
        /// This method responses to the ajax call posted to /Home/GetUserInputs.
        /// It's used to get a list of UserInput objects with filter, sorting and paging
        /// </summary>
        /// <returns>Json object that holds the list of filtered object and total count of all objects</returns>
        [HttpPost]
        public ActionResult GetUserInputs()
        {
            //Get param from REQUEST
            var nextPage = Request["currentPage"] == "" ? 0 : Int32.Parse(Request["currentPage"]);
            var pageSize = Request["pageItems"] == "" ? 5 : Int32.Parse(Request["pageItems"]);
            var orderBy = Request["orderBy"] == "" ? "Id" : Request["orderBy"];
            var orderByReverse = Request["orderByReverse"] == "" ? "true" : Request["orderByReverse"];
            var searchText = Request["searchText"];

            PagingModel paging = new PagingModel();
            paging.PageSize = pageSize;
            paging.PageIndex = nextPage;
            paging.SortOn = orderBy;
            paging.SortReversed = orderByReverse;
            paging.SearchString = searchText;

            //Call class from business layer to handle the business. Controller only servers as an coordinator
            var data = new UserInputBL().GetUserInputs(paging);

            return Json(new
            {
                userInputs = data.Result,
                total = data.TotalSize
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// This method responses to the ajax call posted to /Home/AddUserInput.
        /// It's used to add an UserInput object to database
        /// </summary>
        /// <returns>Json object true if success, false otherwise</returns>
        [HttpPost]
        public ActionResult AddUserInput()
        {
            //Get param from REQUEST
            var expression = Request["expression"].ToString();
            var expressionResult = Request["expessionResult"].ToString();

            //Call class from business layer to handle the business. Controller only servers as an coordinator
            var result = new UserInputBL().AddUserInput(expression, expressionResult);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method responses to the ajax call posted to /Home/DeleteUserInput.
        /// It's used to delete an UserInput object from database
        /// </summary>
        /// <returns>Json object true if success, false otherwise</returns>
        [HttpPost]
        public ActionResult DeleteUserInput()
        {
            //Get param from REQUEST
            var uId = Request["uId"] == "" ? 0 : Int32.Parse(Request["uId"]);

            //Call class from business layer to handle the business. Controller only servers as an coordinator
            var result = new UserInputBL().DeleteUserInput(uId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}