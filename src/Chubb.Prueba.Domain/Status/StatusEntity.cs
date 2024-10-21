using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.Entities.Status
{
    public class StatusEntity
    {
        public int StatusId { get; set; }
        public string? Description { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserModificateId { get; set; }
        public DateTime? DateModificate { get; set; }
    }
}
