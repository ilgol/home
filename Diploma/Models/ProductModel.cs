using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public int Quantity{ get; set; }
        public int? DefaultValue { get; set; }
    }
}