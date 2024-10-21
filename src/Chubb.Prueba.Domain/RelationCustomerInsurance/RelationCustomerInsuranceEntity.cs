using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.Entities.RelationCustomerInsurance
{
    public class RelationCustomerInsuranceEntity
    {
        public int? CustomerInsuranceId { get; set; }
        public int? CustomerId { get; set; }
        public int? TypeInsuranceId { get; set; }
        public int? StatusId { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? UserModificateId { get; set; }
        public DateTime? DateModificate { get; set; }
    }
}

