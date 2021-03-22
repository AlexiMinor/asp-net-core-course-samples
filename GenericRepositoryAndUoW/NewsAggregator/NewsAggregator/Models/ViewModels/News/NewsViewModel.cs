using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.Models.ViewModels
{
    public class NewsViewModel
    {
        public Guid Id { get; set; } 
        public string Article { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Body2 { get; set; }

        public Guid? RssSourseId { get; set; } 
        
        public string RssSourseName { get; set; }
    }
}
