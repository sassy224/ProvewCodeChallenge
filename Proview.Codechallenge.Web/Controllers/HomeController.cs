using Proview.CodeChallenge.BLL;
using Proview.CodeChallenge.BLL.ControlObject;
using Proview.CodeChallenge.Web.ViewModels;
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
            return View();
        }

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

            var data = new UserInputBL().GetUserInputs(paging);

            return Json(new
            {
                userInputs = data.Result,
                total = data.TotalSize
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddUserInput()
        {
            var expression = Request["expression"].ToString();
            var expressionResult = Request["expessionResult"].ToString();
            var result = new UserInputBL().AddUserInput(expression, expressionResult);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUserInput()
        {
            var uId = Request["uId"] == "" ? 0 : Int32.Parse(Request["uId"]);

            var result = new UserInputBL().DeleteUserInput(uId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}