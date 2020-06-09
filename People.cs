using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Core
{
    public class People
    {
        public string Fam { get; set; }
        public string Im { get; set; }
        public string Ot { get; set; }
        public string FullName => Fam + " " + Im + " " + Ot;
        public int Age { get; set; }
        [Required]
        public int Id { get; set; }
    }
}
