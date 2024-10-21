using Chubb.Prueba.BL.TypeInsurance;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.Const;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Const.VariableProcedure;
using Chubb.Prueba.Const.Variables;
using Chubb.Prueba.Entities.Customer;
using Chubb.Prueba.Entities.Result;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubb.Prueba.Entities.TypeInsurance
{
    public class TypeInsuranceServices : ITypeInsuranceServices
    {
        private ITypeInsuranceRepository _repositorio;
        public TypeInsuranceServices(ITypeInsuranceRepository respositorio)
        {
            this._repositorio = respositorio;
        }
        public async Task<ResultResponse> GetTypeInsurance(bool legalAge)
        {
            var result = new ResultResponse();
            try
            {
                result=await _repositorio.GetTypeInsurance(legalAge);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> GetInsuranceByCode(string code)
        {
            var result = new ResultResponse();
            try
            {
                result = await _repositorio.GetInsuranceByCode(code);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> PostTypeInsurance(TypeInsuranceEntity insurance)
        {
            var result = new ResultResponse();
            try
            {
                insurance.StatusId = Variables.ValorEstadoActivo;
                insurance.DateCreate= DateTime.Now;

                result = await _repositorio.InsertTypeInsurance(insurance);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> UpdateTypeInsurance(TypeInsuranceEntity insurance)
        {
            var result = new ResultResponse();
            try
            {
                insurance.DateModificate = DateTime.Now;
                result = await _repositorio.UpdateTypeInsurance(insurance);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
