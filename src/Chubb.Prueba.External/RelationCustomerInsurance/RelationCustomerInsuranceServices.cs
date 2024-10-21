using Chubb.Prueba.BL.RelationCustomerInsurance;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.Const;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Entities.Result;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Chubb.Prueba.DTOs.Customer;
using Chubb.Prueba.Const.VariableProcedure;
using Chubb.Prueba.DTOs.TypeInsurance;
using Chubb.Prueba.BL.Customer;
using Chubb.Prueba.Const.Variables;

namespace Chubb.Prueba.Entities.RelationCustomerInsurance
{
    public class RelationCustomerInsuranceServices : IRelationCustomerInsuranceServices
    {
        private readonly IRelationCustomerInsuranceRepository _repositorio;
        private readonly ICustomerRepository _customerRepository;
        public RelationCustomerInsuranceServices(IRelationCustomerInsuranceRepository repositorio, ICustomerRepository customerRepository)
        {
            this._repositorio = repositorio;
            this._customerRepository = customerRepository;
        }
        public async Task<ResultResponse> GetCustomerForCodeInsurance(string code)
        {
            var result = new ResultResponse();
            try 
            { 
                result=await _repositorio.GetCustomerForCodeInsurance(code);
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> GetInsuranceForCedula(string cedula)
        {
            var result = new ResultResponse();
            try
            {
                result = await _repositorio.GetInsuranceForCedula(cedula);
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> PostRelationCustomerInsurance(RelationCustomerInsuranceDTO relation)
        {
            var result = new ResultResponse();
            var relationEntity = new RelationCustomerInsuranceEntity();
            try
            {
                
                if (relation.CustomerCedula != null)
                {
                    var Customer = await _customerRepository.GetCustomerByCedula(relation.CustomerCedula);
                    var idCustomer = Customer.Data;
                   relationEntity.CustomerId = idCustomer[0].CustomerId;
                }
                else
                {
                    relationEntity.CustomerId=relation.CustomerId;
                }
                relationEntity.TypeInsuranceId = relation.TypeInsuranceId;    
                relationEntity.DateCreate = DateTime.Now;
                relationEntity.StatusId = Variables.ValorEstadoActivo;
                result = await _repositorio.InsertRelationCustomerInsurance(relationEntity);
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        
        public async Task<ResultResponse> UpdateRelationCustomerInsurance(RelationCustomerInsuranceEntity relation)
        {
            var result = new ResultResponse();
            try
            {
                relation.DateModificate= DateTime.Now;
                result = await _repositorio.UpdateRelationCustomerInsurance(relation);
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
       
    }
}
