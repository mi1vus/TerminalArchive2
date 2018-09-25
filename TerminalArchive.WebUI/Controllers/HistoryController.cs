using System;
using System.Linq;
using System.Web.Mvc;
using TerminalArchive.Domain.Abstract;
using TerminalArchive.Domain.DB;
using TerminalArchive.WebUI.Models;

namespace TerminalArchive.WebUI.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ITerminalRepository _repository;
        public int PageSize = 2;

        public HistoryController()
        {
            _repository = new TerminalRepository { UserName = User?.Identity?.Name };
        }

        public bool AddHistory(
            string HaspId, string RRN,
            string Trace, string Msg, int? ErrorLevel, string Date,
            string User, string Pass
        )
        {
            if (User != "AutoAdmin")
                return false;

            return DbHelper.AddHistory(HaspId, RRN,Trace , Msg, ErrorLevel, Date, User, Pass);
        }

        [Authorize]
        public ViewResult List(int id, int page = 1, string rrn = "")
        {
            _repository.UserName = User?.Identity?.Name;

            var groups = DbHelper.GetUserGroups(_repository.UserName, Constants.RightReadName);
            var terminal = _repository.GetTerminal(id);
            if (groups == null || terminal == null || (groups.Any() && groups.All(g => g.Id != terminal.IdGroup)))
                return View("Unauthorize");

            if (!string.IsNullOrWhiteSpace(rrn))
            {
                var newTerminals = terminal.Orders.Where(o => o.Value.Rnn == rrn);
                terminal.Orders = terminal.Orders.Where(o => o.Value.Rnn == rrn).Select(o => o.Value).ToDictionary(o => o.Id);
            }
            var history = DbHelper.GetHistory(_repository.UserName, id, rrn, page, PageSize);
            var maxPages = 0;
            int totalItems = DbHelper.HistoryCount(_repository.UserName, id, rrn);
            if (totalItems <= 0)
                maxPages = 1;
            else
                maxPages = (int)Math.Ceiling((decimal)totalItems / PageSize);

            var terminalsModel = new HistoryViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page > maxPages ? maxPages : page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                },
                History = history,
                Terminal = new ViewTerminal(terminal)
                {
                    GroupsIdsString = terminal.IdGroup.ToString(),
                    GroupsNamesString = terminal.Group?.Name
                }
            };

            return View(terminalsModel);
        }
    }
}
