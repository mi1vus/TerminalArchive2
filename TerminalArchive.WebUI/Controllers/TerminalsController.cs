﻿using System.Web.Mvc;
using TerminalArchive.Domain.DB;

namespace TerminalArchive.WebUI.Controllers
{
    public class TerminalApiController : Controller
    {
        //public TerminalsController()
        //{
        //}
        //[HttpGet]
        //[HttpPost]
        //public IEnumerable<Parameter> GetParameters(int id = 0)
        //{
        //    return DbHelper.GetParametersForUpdate("1", "2", "3");
        //}

        public ActionResult GetParameters(string HaspId, string User, string Pass)
        {
            if (User != "AutoAdmin")
                return View("Unauthorize");

            return Json(DbHelper.GetParametersForUpdate(HaspId, User, Pass));
        }

        public int UpdateSaveDate(int TId, int ParId, string User, string Pass)
        {
            if (User != "AutoAdmin")
                return 0;

            return DbHelper.UpdateSaveDate(TId, ParId, User, Pass);
        }

        public bool AddNewOrder(
            string RRN,
            string HaspId,
            int Fuel,
            int Pump,
            int Payment,
            int State,
            decimal PrePrice,
            decimal Price,
            decimal PreQuantity,
            decimal Quantity,
            decimal PreSumm,
            decimal Summ,
            string User, string Pass
            )
        {
            if (User != "AutoAdmin")
                return false;

            return DbHelper.AddNewOrder(RRN, HaspId, Fuel, Pump, Payment, State, PrePrice, Price, PreQuantity, Quantity, PreSumm, Summ, User,  Pass);
        }
    }
}