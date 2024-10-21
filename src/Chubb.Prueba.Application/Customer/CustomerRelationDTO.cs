using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.DTOs.Customer
{
    public class CustomerRelationDTO
    {
        public int? TypeInsuranceId{get;set;}
        public double? Prima{get;set;}
        public double? SumaAsegurada{get;set; }
        public string? CodeInsurance{get;set;}
        public string? NameInsurance{get;set;}
        public int? CustomerId{get;set;}
        public string? FirstName{get;set;}
        public string? LastName{get;set;}
        public string? Cedula{get;set;}
        public string? Telephone{get;set;}
        public string? Email{get;set;}
        public DateTime? DateBorn{get;set;}
        public DateTime? DateCreate{get;set;}
        public int? CustomerInsuranceId{get;set;}
    }
}
