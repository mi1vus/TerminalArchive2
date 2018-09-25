using System;
using System.Text;
using System.Web.Mvc;
using TerminalArchive.WebUI.Models;

namespace TerminalArchive.WebUI.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            bool goToFirst = pagingInfo.CurrentPage > 4;
            bool goToLast = pagingInfo.CurrentPage < pagingInfo.TotalPages - 3;
            int left = goToFirst ? pagingInfo.CurrentPage - 3 : 1;
            int right = goToLast ? pagingInfo.CurrentPage + 3 : pagingInfo.TotalPages;

            if (goToFirst)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(1));
                tag.InnerHtml = "<<";
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            for (int i = left; i <= right; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            if (goToLast)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                tag.InnerHtml = ">>";
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}