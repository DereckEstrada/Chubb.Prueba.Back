using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chubb.Prueba.Entities.Customer;
using Chubb.Prueba.Entities.Result;
namespace Chubb.Prueba.BL.Customer
{
    public interface ICustomerRepository
    {
        Task<ResultResponse> GetCustomerByCedula(string cedula);
        Task<ResultResponse> GetCustomersWithoutInsurance();
        Task<ResultResponse> InsertCustomer(CustomerEntity customer);
        Task<ResultResponse> UpdateCustomer(CustomerEntity customer);
    }
}
