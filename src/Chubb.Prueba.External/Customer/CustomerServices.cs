using Chubb.Prueba.BL.Customer;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.Const;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Entities.Result;
using Chubb.Prueba.Entities.TypeInsurance;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chubb.Prueba.Const.VariableProcedure;
using Chubb.Prueba.Entities.RelationCustomerInsurance;
using Chubb.Prueba.BL.TypeInsurance;
using Chubb.Prueba.BL.RelationCustomerInsurance;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Chubb.Prueba.Const.Variables;

namespace Chubb.Prueba.Entities.Customer
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _repositorio;
        private readonly ITypeInsuranceServices _insuranceServices;
        private readonly IRelationCustomerInsuranceServices _relationServices;
        public CustomerServices(ICustomerRepository repositorio, ITypeInsuranceServices insuranceServices, IRelationCustomerInsuranceServices relationServices)
        {
            this._repositorio = repositorio;
            this._insuranceServices = insuranceServices;
            this._relationServices = relationServices;
        }
        public async Task<ResultResponse> GetCustomerByCedulaRepresent(string cedula)
        {
            var result = new ResultResponse();
            try
            {
                result = await _repositorio.GetCustomerByCedulaRepresent(cedula);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ResultResponse> GetCustomersWithoutInsurance()
        {
            var result = new ResultResponse();
            try
            {
                result = await _repositorio.GetCustomersWithoutInsurance();
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ResultResponse> GetCustomerByCedula(string cedula)
        {
            var result = new ResultResponse();
            try
            {
                result = await _repositorio.GetCustomerByCedula(cedula);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> PostCustomer(CustomerEntity customer)
        {
            var result = new ResultResponse();
            
            try
            {

                var verificate = await this.VerificarCedula(customer.Cedula);
                if (verificate)
                {
                    throw new Exception("Cedula: " + customer.Cedula + " ya existente");
                }

                customer.StatusId = Variables.ValorEstadoActivo;
                customer.DateCreate= DateTime.Now;
                result =await _repositorio.InsertCustomer(customer);

            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> UpdateCustomer(CustomerEntity customer)
        {
            var result = new ResultResponse();
            try
            {
                customer.DateModificate= DateTime.Now;  
                result = await _repositorio.UpdateCustomer(customer);
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message= ex.Message;
            }
            return result;
        }

        private bool CalculateAge(DateTime dateBorn)
        {
            bool legalAge=false;    
            DateTime fechaActual = DateTime.Now;
            int edad = fechaActual.Year - dateBorn.Year;

            if (fechaActual < dateBorn.AddYears(edad))
            {
                edad--;
            }
            if (edad >= 20)
            {
               legalAge = true;
            }
            return legalAge;
        }
        private async Task<ResultResponse> AsignationInsurance(CustomerEntity customer)
        {
            var result=new ResultResponse();
            var relationList = new List<RelationCustomerInsuranceEntity>();
            var idNoLegalInsurance = await _insuranceServices.GetTypeInsurance(false);
            var idLegalInsurance = await _insuranceServices.GetTypeInsurance(true);

                if (this.CalculateAge((DateTime)customer.DateBorn))
                {
                    foreach (var idLegal in idLegalInsurance.Data)
                    {
                        var relation=(new RelationCustomerInsuranceDTO()
                        {
                            CustomerId = customer.CustomerId,
                            TypeInsuranceId = idLegal.TypeInsuranceId,
                            StatusId = Variables.ValorEstadoActivo,
                            UserCreateId = Variables.ValorEstadoActivo,
                            DateCreate = DateTime.Now,
                        });
                    result = await _relationServices.PostRelationCustomerInsurance(relation);
                }
            }
                else
                {
                var listIdIlegal = idNoLegalInsurance.Data;
                    foreach (var idNoLegal in idNoLegalInsurance.Data)
                    {

                    var relation =(new RelationCustomerInsuranceDTO()
                        {
                            CustomerId = customer.CustomerId,
                            TypeInsuranceId = idNoLegal.TypeInsuranceId,
                            StatusId = Variables.ValorEstadoActivo,
                            UserCreateId = Variables.ValorEstadoActivo,
                            DateCreate = DateTime.Now,
                        });
                    result = await _relationServices.PostRelationCustomerInsurance(relation);
                       
                    }
                }
            return result;
        }

        public async Task<ResultResponse> UploadFile(IFormFile file)
        {
            var result = new ResultResponse();
            var customerList = new List<CustomerEntity>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream); 

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; 

                        for (int row = 2; row <= worksheet.Dimension.End.Row; row++) 
                        {
                            var customer = new CustomerEntity();
                            customer.FirstName = worksheet.Cells[row, 1].Value?.ToString();
                            customer.LastName = worksheet.Cells[row, 2].Value?.ToString();
                            customer.Cedula = worksheet.Cells[row, 3].Value?.ToString();
                            customer.Telephone = worksheet.Cells[row, 4].Value?.ToString();
                            customer.DateBorn = DateTime.Parse(worksheet.Cells[row, 5].Value?.ToString());
                            customer.Email = worksheet.Cells[row, 6].Value?.ToString();
                            var verificate = await this.VerificarCedula(customer.Cedula);
                            if (verificate)
                            {
                                throw new Exception("Cedula: " + customer.Cedula + " ya existente");
                            }

                            customerList.Add(customer);
                        }
                        foreach (var customer in customerList)
                        {
                            await this.PostCustomer(customer);
                        }
                    }
                    var resultCustomerWithOutInsurance = await this.GetCustomersWithoutInsurance();

                    if (resultCustomerWithOutInsurance.Code.Equals(CodeHttp.GoodResponseGet))
                    {
                        var customers = resultCustomerWithOutInsurance.Data;
                        foreach (var customerWithOut in customers)
                        {
                            result = await this.AsignationInsurance(customerWithOut);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Data = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        private async Task<bool> VerificarCedula(string cedula)
        {
            bool exist=false;
            var verificate = await this.GetCustomerByCedula(cedula);
            if (verificate.Code == CodeHttp.GoodResponseGet)
            {
                exist=true;
            }
            return exist;
            
        }
    }
}
