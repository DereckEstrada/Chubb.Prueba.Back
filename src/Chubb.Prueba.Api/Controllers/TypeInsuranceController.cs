using Chubb.Prueba.Api.Request;
using Chubb.Prueba.BL.TypeInsurance;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Entities.Result;
using Chubb.Prueba.Entities.TypeInsurance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Chubb.Prueba.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TypeInsuranceController : ControllerBase
    {
        private readonly ITypeInsuranceServices _services;
        public TypeInsuranceController(ITypeInsuranceServices services)
        {
            this._services = services;
        }
        [HttpPost]
        [Route("GetTypeInsurance")]
        public async Task<ResultResponse> GetTypeInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {

                result = await _services.GetTypeInsurance(bool.Parse(request.Data.ToString()));
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetInsuranceByCode")]
        public async Task<ResultResponse> GetInsuranceByCode(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                if (request.Data.ToString() != "")
                {
                    result = await _services.GetInsuranceByCode(request.Data.ToString());
                }
                else
                {
                    result.Code = CodeHttp.BadResponse;
                    result.Message = MessageResponse.BadMessage;
                }
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }


        [HttpPost]
        [Route("PostTypeInsurance")]
        public async Task<ResultResponse> PostTypeInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<TypeInsuranceEntity>(Convert.ToString(request.Data));
                result = await _services.PostTypeInsurance(data);
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }


        [HttpPost]
        [Route("UpdateTypeInsurance")]
        public async Task<ResultResponse> UpdateTypeInsurance(RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<TypeInsuranceEntity>(Convert.ToString(request.Data));
                result = await _services.UpdateTypeInsurance(data);
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
