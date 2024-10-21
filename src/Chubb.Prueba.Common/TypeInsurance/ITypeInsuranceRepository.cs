using Chubb.Prueba.Entities.Result;
using Chubb.Prueba.Entities.TypeInsurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.BL.TypeInsurance
{
    public  interface ITypeInsuranceRepository
    {
        Task<ResultResponse> GetTypeInsurance(bool legalAge); 
        Task<ResultResponse> GetInsuranceByCode(string code);
        Task<ResultResponse> InsertTypeInsurance(TypeInsuranceEntity insurance); 
        Task<ResultResponse> UpdateTypeInsurance(TypeInsuranceEntity insurance);
    }
}
