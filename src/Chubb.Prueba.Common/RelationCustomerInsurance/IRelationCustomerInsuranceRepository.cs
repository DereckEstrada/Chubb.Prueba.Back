using Chubb.Prueba.Entities.RelationCustomerInsurance;
using Chubb.Prueba.Entities.Result;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.BL.RelationCustomerInsurance
{
    public interface IRelationCustomerInsuranceRepository
    {
        Task<ResultResponse> GetInsuranceForCedula(string cedula);        
        Task<ResultResponse> GetCustomerForCodeInsurance(string code);        
        Task<ResultResponse> InsertRelationCustomerInsurance(RelationCustomerInsuranceEntity relation);
        Task<ResultResponse> UpdateRelationCustomerInsurance(RelationCustomerInsuranceEntity relation);
    }
}
