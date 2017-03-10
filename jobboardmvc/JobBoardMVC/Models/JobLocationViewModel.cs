using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobBoardMVC.Models
{
    public class JobLocationViewModel
    {
        public List<Job> jobs;
        public SelectList locations;
        public string jobLocation { get; set; }
    }
}