using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.DepotEice.BLL.Models
{
    public class ScheduleFileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int ScheduleId { get; set; }
        public int FileId { get; set; }
    }
}
