using Chubb.Prueba.Api.Request;
using Chubb.Prueba.BL.RelationCustomerInsurance;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Entities.RelationCustomerInsurance;
using Chubb.Prueba.Entities.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Chubb.Prueba.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RelationController : ControllerBase
    {
        private readonly IRelationCustomerInsuranceServices _services;
        public RelationController(IRelationCustomerInsuranceServices services)
        {
            this._services = services;
        }
        [HttpPost]
        [Route("GetInsuranceForCedula")]
        public async Task<ResultResponse> GetInsuranceForCedula(RequestData  request) 
        {
            var result = new ResultResponse();
            try
            {
                result = await _services.GetInsuranceForCedula(request.Data.ToString() );
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetCustomerForCodeInsurance")]
        public async Task<ResultResponse> GetCustomerForCodeInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                result = await _services.GetCustomerForCodeInsurance(request.Data.ToString());
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("PostRelationCustomerInsurance")]
        public async Task<ResultResponse> PostRelationCustomerInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<RelationCustomerInsuranceDTO>(Convert.ToString(request.Data));
                result = await _services.PostRelationCustomerInsurance(data);
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("UpdateRelationCustomerInsurance")]
        public async Task<ResultResponse> UpdateRelationCustomerInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<RelationCustomerInsuranceEntity>(Convert.ToString(request.Data));
                result = await _services.UpdateRelationCustomerInsurance(data);
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
