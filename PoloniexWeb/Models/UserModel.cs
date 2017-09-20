using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoloniexWeb.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string Password { get; set; }
    }
}