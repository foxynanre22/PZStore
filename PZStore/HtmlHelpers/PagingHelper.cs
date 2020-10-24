using Microsoft.Ajax.Utilities;
using PZStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PZStore.HtmlHelpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper Html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder link = new TagBuilder("a");
                link.MergeAttribute("href", pageUrl(i));
                link.InnerHtml = i.ToString();

                if(i == pagingInfo.CurrentPage)
                {
                    link.AddCssClass("selected");
                    link.AddCssClass("btn btn-info");
                }
                link.AddCssClass("btn btn-default");
                result.Append(link.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}