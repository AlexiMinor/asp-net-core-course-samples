using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Models;

namespace NewsAggregator.HtmlHelpers
{
    public static class NewsListHelper
    {
        public static HtmlString CreateListNews(this IHtmlHelper html,
            IEnumerable<NewsDto> news)
        {
            var sb = new StringBuilder();

            foreach (var newsDto in news)
            {
                sb.Append($"<div><h2>{newsDto.Article}</h2>");

                /* GENERATE HTML 
                 * <div>
                    <h2>@Model.Article</h2>
                    <div>
                        @Model.Body
                    </div>
                    <div>
                        <a asp-action="Details" asp-route-id="@Model.Id">Читать на аггрегаторе</a>
                    </div>
                    <div>
                        <a href="@Model.Url">Читать в источнике</a>
                    </div>
                </div>
                <hr/>

                 */
            }

            return new HtmlString(sb.ToString());
        }
    }
}
