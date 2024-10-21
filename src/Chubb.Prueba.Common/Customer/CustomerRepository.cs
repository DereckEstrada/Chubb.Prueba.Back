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
using Chubb.Prueba.DTOs.Customer;

namespace Chubb.Prueba.Entities.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        public CustomerRepository(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ResultResponse> GetCustomerByCedulaRepresent(string cedula)
        {
            var result = new ResultResponse();
            var customerIdRepresent = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getAllCustomer, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.Cedula, cedula);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customerIdRepresent = reader["CustomerId"] != DBNull.Value ? (int)reader["CustomerId"]:default;                                
                            }
                            result.Code = customerIdRepresent==0? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data = customerIdRepresent;
                            result.Message = MessageResponse.GoodMessage;
                        }
                    };
                };
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> GetCustomerByCedula(string cedula)
        {
            var result = new ResultResponse();
            var customerList = new List<CustomerDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getCustomerByCedula, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.Cedula, cedula);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customerList.Add(new CustomerDTO
                                {


                                    CustomerId = reader["CustomerId"] != DBNull.Value ? (int)reader["CustomerId"] : default,
                                    FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : string.Empty,
                                    LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : string.Empty,
                                    Cedula = reader["Cedula"] != DBNull.Value ? reader["Cedula"].ToString() : string.Empty,
                                    Telephone = reader["Telephone"] != DBNull.Value ? reader["Telephone"].ToString() : string.Empty,
                                    DateBorn = reader["DateBorn"] != DBNull.Value ? (DateTime)reader["DateBorn"] : DateTime.MinValue,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                                    StatusId = reader["StatusId"] != DBNull.Value ? (int)reader["StatusId"]: default,
                                    DateCreate = reader["DateCreate"] != DBNull.Value ?(DateTime) reader["DateCreate"]:DateTime.MinValue,
                                    DateModificate = reader["DateModificate"] != DBNull.Value ? (DateTime)reader["DateModificate"] : DateTime.MinValue

                                });
                            }
                            result.Code = customerList.Count()==0 ? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data = customerList;
                            result.Message = MessageResponse.GoodMessage;
                        }
                    };
                };
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ResultResponse> GetCustomersWithoutInsurance()
        {
            var result = new ResultResponse();
            var customerList=new List<CustomerEntity>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.GetCustomersWithoutInsurance, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                customerList.Add(new CustomerEntity
                                {
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? (int)reader["CustomerId"]:default,
                                    DateBorn = reader["DateBorn"] != DBNull.Value ? (DateTime)reader["DateBorn"]:DateTime.MinValue,
                                });
                            }
                            result.Code = customerList.Count() == 0 ? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data = customerList;
                            result.Message = MessageResponse.GoodMessage;
                        }
                    };
                };
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<ResultResponse> InsertCustomer(CustomerEntity customer)
        {
            var result = new ResultResponse();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction()) { 
                        try
                        {
                            {
                                using (SqlCommand command = new SqlCommand(PostProcedure.InsertCustomer, connection, transaction))
                                {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue(VariableCustomer.FirstName,customer.FirstName);
                                        command.Parameters.AddWithValue(VariableCustomer.LastName,customer.LastName);
                                        command.Parameters.AddWithValue(VariableCustomer.Cedula,customer.Cedula);
                                        command.Parameters.AddWithValue(VariableCustomer.Telephone ,customer.Telephone );
                                        command.Parameters.AddWithValue(VariableCustomer.DateBorn,customer.DateBorn);
                                        command.Parameters.AddWithValue(VariableCustomer.Email,customer.Email);
                                        command.Parameters.AddWithValue(VariableCustomer.StatusId,customer.StatusId);
                                        command.Parameters.AddWithValue(VariableCustomer.DateCreate, customer.DateCreate);
                                    SqlParameter errorParam = new SqlParameter(VariableProcedure.ErrorMessage, SqlDbType.NVarChar, 4000)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    command.Parameters.Add(errorParam);
                                    await command.ExecuteNonQueryAsync();
                                }
                                transaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }

                }
                result.Code = CodeHttp.GoodResponseNoContent;
                result.Message = MessageResponse.GoodMessage;
            }
            catch (Exception ex)
            {
               
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> UpdateCustomer(CustomerEntity customer)
        {
            var result = new ResultResponse();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            {
                                using (SqlCommand command = new SqlCommand(UpdateProcedures.UpdateCustomer, connection, transaction))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue(VariableCustomer.CustomerId, customer.CustomerId);
                                    command.Parameters.AddWithValue(VariableCustomer.FirstName, customer.FirstName);
                                    command.Parameters.AddWithValue(VariableCustomer.LastName, customer.LastName);
                                    command.Parameters.AddWithValue(VariableCustomer.Cedula, customer.Cedula);
                                    command.Parameters.AddWithValue(VariableCustomer.Telephone, customer.Telephone);
                                    command.Parameters.AddWithValue(VariableCustomer.DateBorn, customer.DateBorn);
                                    command.Parameters.AddWithValue(VariableCustomer.Email, customer.Email);
                                    command.Parameters.AddWithValue(VariableCustomer.StatusId, customer.StatusId);
                                    command.Parameters.AddWithValue(VariableCustomer.DateModificate, customer.DateModificate);
                                    SqlParameter errorParam = new SqlParameter(VariableProcedure.ErrorMessage, SqlDbType.NVarChar, 4000)
                                    {
                                        Direction = ParameterDirection.Output
                                    };
                                    command.Parameters.Add(errorParam);
                                    await command.ExecuteNonQueryAsync();
                                }
                                transaction.Commit();
                            }
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                result.Code = CodeHttp.GoodResponseNoContent;
                result.Message = MessageResponse.GoodMessage;
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
