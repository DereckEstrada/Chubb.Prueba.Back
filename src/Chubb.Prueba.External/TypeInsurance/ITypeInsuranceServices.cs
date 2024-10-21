using Chubb.Prueba.Entities.Result;
using Chubb.Prueba.Entities.TypeInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.BL.TypeInsurance
{
    public  interface ITypeInsuranceServices
    {
        Task<ResultResponse> GetTypeInsurance(bool legalAge);
        Task<ResultResponse> GetInsuranceByCode(string code);
        Task<ResultResponse> PostTypeInsurance(TypeInsuranceEntity insurance); 
        Task<ResultResponse> UpdateTypeInsurance(TypeInsuranceEntity insurance);
    }
}
