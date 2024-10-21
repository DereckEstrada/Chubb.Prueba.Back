using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.Entities.TypeInsurance
{
    public class TypeInsuranceEntity
    {
        public int? TypeInsuranceId { get; set; }
        public string? CodeInsurance { get; set; }
        public string? NameInsurance { get; set; }
        public string? Description { get; set; }
        public double? SumaAsegurada { get; set; }
        public double? Prima { get; set; }
        public bool? LegalAge {  get; set; }    
        public int? StatusId { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserModificateId { get; set; }
        public DateTime? DateModificate { get; set; }
    }
}
