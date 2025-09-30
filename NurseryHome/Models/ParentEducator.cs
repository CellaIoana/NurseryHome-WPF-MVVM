using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseryHome.Models
{
    public class ParentEducator
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int EducatorId { get; set; }
    }

}
