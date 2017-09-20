using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoloniexWeb.Models
{
    public class Result
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
        public double Last { get; set; }
    }

    public class CurrencyModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
    }
}