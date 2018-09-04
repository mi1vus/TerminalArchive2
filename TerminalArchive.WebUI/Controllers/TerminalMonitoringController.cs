using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TerminalArchive.Domain.Abstract;
using TerminalArchive.Domain.DB;
using TerminalArchive.Domain.Models;
using TerminalArchive.WebUI.Models;

namespace TerminalArchive.WebUI.Controllers
{
    public class TerminalMonitoringController : Controller
    {
        private readonly ITerminalRepository _repository;
        public int PageSize = 10;
        public static List<string> TerminalNames = new List<string>();

        public TerminalMonitoringController()
        {
            _repository = new TerminalRepository { UserName = User?.Identity?.Name };
        }

        public TerminalMonitoringController(ITerminalRepository repo)
        {
            _repository = repo;
            _repository.UserName = User?.Identity?.Name;
        }

        [Authorize]
        public ViewResult List(int page = 1, int FilterTerminalGroup = 0, string FilterTerminalName = "")
        {
            _repository.UserName = User?.Identity?.Name;

            var groupsAll = DbHelper.GetAllGroups(_repository.UserName);
            var groups = new List<Group> { new Group { Id = -1, Name = "None" } };
            groups.AddRange(groupsAll.Values);
            ViewBag.Groups = groups;

            var terminalsModel = new TerminalsListViewModel
            {
                Terminals = new List<ViewTerminal>(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = 1,
                    ItemsPerPage = PageSize,
                    TotalItems = 0
                }
            };

            //if (!DbHelper.UserInRole(_repository.UserName, Constants.RightReadName, null))
            //    return View(terminalsModel);

            int maxPages;
            var terminals = _repository.Terminals as IList<Terminal> ?? _repository.Terminals.ToList();
            TerminalNames = terminals.Select(t => t.Name).ToList();
            bool filter = FilterTerminalGroup > 0 || !string.IsNullOrWhiteSpace(FilterTerminalName);
            Func<Terminal, bool> PredicatGroup = t => true;
            if (FilterTerminalGroup > 0)
                PredicatGroup = t => t.IdGroup == FilterTerminalGroup;

            Func<Terminal, bool> PredicatName = t => true;
            if (!string.IsNullOrWhiteSpace(FilterTerminalName))
                PredicatName = t => t.Name == FilterTerminalName;



            var totalItems = terminals.Count(t => PredicatGroup(t) && PredicatName(t));
            if (totalItems <= 0)
                maxPages = 1;
            else
                maxPages = (int)Math.Ceiling((decimal)totalItems / PageSize);

            terminalsModel.Terminals =
            from terminal in terminals.Where(t => PredicatGroup(t) && PredicatName(t)).OrderBy(t => t.Name).Skip((page - 1) * PageSize).Take(PageSize)
            select new ViewTerminal(terminal)
            {
                GroupsIdsString = terminal.IdGroup > 0 ? terminal.IdGroup.ToString() : " - ",
                GroupsNamesString = terminal.Group?.Name ?? " - "
            };
            terminalsModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page > maxPages ? maxPages : page,
                ItemsPerPage = PageSize,
                TotalItems = totalItems
            };

            ViewBag.CurrentController = GetType().ToString();
            return View(terminalsModel);
        }
        //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        //public static string[] GetCompletionList(string prefixText, int count, string contextKey)
        //{
        //    // Create array of movies  
        //    string[] movies = { "Star Wars", "Star Trek", "Superman", "Memento", "Shrek", "Shrek II" };

        //    // Return matching movies  
        //    return (from m in movies where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m).Take(count).ToArray();
        //}


        public JsonResult GetListForAutoComplite(string prefixText, int maxRows)
        {
            // Create array of movies  
            //string[] movies = { "Star Wars", "Star Trek", "Superman", "Memento", "Shrek", "Shrek II" };

            // Return matching movies  
            return Json(from m in TerminalNames where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select m);


            //var company = Json(from t in context.Company
            //                   where t.Name.ToUpper().Contains(q.ToUpper())
            //                   select new { t.CompanyID, t.Name });


            //return company;
        }
    }
}