using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoloniexWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]  
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}