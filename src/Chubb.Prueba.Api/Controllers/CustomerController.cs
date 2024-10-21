using Chubb.Prueba.Api.Request;
using Chubb.Prueba.BL.Customer;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Entities.Customer;
using Chubb.Prueba.Entities.Result;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Chubb.Prueba.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _services;
        public CustomerController(ICustomerServices services)
        {
            this._services = services;  
        }


        [HttpPost]
        [Route("GetCustomersWithoutInsurance")]
        public async Task<ResultResponse> GetCustomersWithoutInsurance([FromBody] RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                result = await _services.GetCustomersWithoutInsurance();
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("GetCustomerByCedula")]
        public async Task<ResultResponse> GetCustomerByCedula([FromBody] RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                if (request.Data.ToString()!="")
                {
                result = await _services.GetCustomerByCedula(request.Data.ToString());
                }
                else
                {
                   result.Code=CodeHttp.BadResponse;
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
        [Route("PostCustomer")]
        public async Task<ResultResponse> PostCustomer([FromBody] RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<CustomerEntity>(Convert.ToString(request.Data));
                result = await _services.PostCustomer(data);
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("UpdateCustomer")]
        public async Task<ResultResponse> UpdateCustomer([FromBody] RequestData request)
        {
            var result = new ResultResponse();
            try
            {
                var data = JsonConvert.DeserializeObject<CustomerEntity>(Convert.ToString(request.Data));
                result = await _services.UpdateCustomer(data);
            }
            catch (Exception ex)
            {

                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("UploadFile")]
        public async Task<ResultResponse> UploadFile([FromForm]IFormFile file)
        {
            var result = new ResultResponse();
            try
            {
                //var data = JsonConvert.DeserializeObject<CustomerEntity>(Convert.ToString(request.Data));
                result = await _services.UploadFile(file);
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
