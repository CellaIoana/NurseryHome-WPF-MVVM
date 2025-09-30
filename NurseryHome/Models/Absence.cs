using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseryHome.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}

