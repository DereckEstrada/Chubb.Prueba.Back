using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chubb.Prueba.Entities.Customer;
using Chubb.Prueba.Entities.Result;
using Microsoft.AspNetCore.Http;
namespace Chubb.Prueba.BL.Customer
{
    public interface ICustomerServices
    {
        Task<ResultResponse> GetCustomerByCedulaRepresent(string cedula);
        Task<ResultResponse> UploadFile(IFormFile file);
        Task<ResultResponse> GetCustomersWithoutInsurance();
        Task<ResultResponse> GetCustomerByCedula(string cedula);
        Task<ResultResponse> PostCustomer(CustomerEntity customer);
        Task<ResultResponse> UpdateCustomer(CustomerEntity customer);
    }
}
