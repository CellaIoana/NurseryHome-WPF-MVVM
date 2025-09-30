using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseryHome.Models
{
    public class Program
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateTime Date { get; set; }
        public string ActivityName { get; set; } = string.Empty;
    }
}

