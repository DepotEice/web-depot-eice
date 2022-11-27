using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    public class ScheduleModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int ModuleId { get; set; }

        public override string ToString()
        {
            return $"{Id}\n{Title}\n{Details}\n{StartAt}\n{EndAt}\n{ModuleId}";
        }
    }
}
