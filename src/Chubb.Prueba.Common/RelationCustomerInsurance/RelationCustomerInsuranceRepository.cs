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
using System.Xml.XPath;
using System.Dynamic;
using Chubb.Prueba.Const.Variables;

namespace Chubb.Prueba.Entities.RelationCustomerInsurance
{
    public class RelationCustomerInsuranceRepository : IRelationCustomerInsuranceRepository
    {
        private readonly string _connectionString;
        public RelationCustomerInsuranceRepository(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ResultResponse> GetCustomerForCodeInsurance(string code)
        {
            var result = new ResultResponse();
            var typeInsuranceList = new List<TypeInsuranceRelationDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getCustomerForCodeInsurance, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.InsuranceCode, code);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var legalAge = this.CalculateAge((DateTime)reader["DateBorn"]);
                                typeInsuranceList.Add(new TypeInsuranceRelationDTO
                                {
                                    Cedula = reader["Cedula"] != DBNull.Value ? reader["Cedula"].ToString() : string.Empty,
                                    FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : string.Empty,
                                    LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : string.Empty,
                                    DateBorn = reader["DateBorn"] != DBNull.Value ? (DateTime)reader["DateBorn"] : DateTime.MinValue,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                                    TypeInsuranceId = reader["TypeInsuranceId"] != DBNull.Value ? (int)reader["TypeInsuranceId"] : default,
                                    CodeInsurance = reader["CodeInsurance"] != DBNull.Value ? reader["CodeInsurance"].ToString() : string.Empty,
                                    NameInsurance = reader["NameInsurance"] != DBNull.Value ? reader["NameInsurance"].ToString() : string.Empty,
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                    SumaAsegurada = reader["SumaAsegurada"] != DBNull.Value ? (double)reader["SumaAsegurada"] : default,
                                    Prima = reader["Prima"] != DBNull.Value ? (double)reader["Prima"] : default,
                                    LegalAge = reader["LegalAge"] != DBNull.Value ? (bool)reader["LegalAge"] : false,
                                    CustomerInsuranceId = reader["CustomerInsuranceId"] != DBNull.Value ? (int)reader["CustomerInsuranceId"] : default,

                                });
                            }
                            result.Code = typeInsuranceList.Count() == 0 ? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data = typeInsuranceList;
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

        public async Task<ResultResponse> GetInsuranceForCedula(string cedula)
        {
            var result = new ResultResponse();
            var customerList = new List<CustomerRelationDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getInsuranceForCedula, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.Cedula, cedula);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var legalAge = this.CalculateAge((DateTime)reader["DateBorn"]);
                                customerList.Add(new CustomerRelationDTO
                                {
                                    TypeInsuranceId = reader["TypeInsuranceId"] != DBNull.Value ? (int)reader["TypeInsuranceId"] : default,
                                    Prima = reader["Prima"]!=DBNull.Value?(double)reader["Prima"]:default,
                                    SumaAsegurada = reader["SumaAsegurada"] !=DBNull.Value?(double)reader["SumaAsegurada"] :default,
                                    CodeInsurance = reader["CodeInsurance"].ToString(),
                                    NameInsurance = reader["NameInsurance"].ToString(),
                                    CustomerId = reader["CustomerId"] != DBNull.Value ? (int)reader["CustomerId"] : default,
                                    FirstName = reader["FirstName"]?.ToString() ?? string.Empty, 
                                    LastName = reader["LastName"]?.ToString() ?? string.Empty,
                                    Cedula = reader["Cedula"]?.ToString() ?? string.Empty,
                                    Telephone = reader["Telephone"]?.ToString() ?? string.Empty,
                                    Email = reader["Email"]?.ToString() ?? string.Empty,
                                    DateBorn = reader["DateBorn"] != DBNull.Value ? (DateTime)reader["DateBorn"] : DateTime.MinValue,
                                    DateCreate = reader["DateCreate"] != DBNull.Value ? (DateTime)reader["DateCreate"] : DateTime.MinValue,
                                    CustomerInsuranceId = reader["CustomerInsuranceId"] != DBNull.Value ? (int)reader["CustomerInsuranceId"] : default,
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

        public async Task<ResultResponse> InsertRelationCustomerInsurance(RelationCustomerInsuranceEntity relation)
        {
            var result = new ResultResponse();  
            try
            {
                using(SqlConnection connection=new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction()) {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(PostProcedure.InsertRelationCustomerInsurance, connection, transaction))
                            {

                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue(VariableRelation.CustomerId, relation.CustomerId);
                                command.Parameters.AddWithValue(VariableRelation.TypeInsuranceId,relation.TypeInsuranceId );
                                command.Parameters.AddWithValue(VariableRelation.StatusId,relation.StatusId );
                                command.Parameters.AddWithValue(VariableRelation.DateCreate,relation.DateCreate );
                                SqlParameter errorParam = new SqlParameter(VariableProcedure.ErrorMessage, SqlDbType.NVarChar, 4000)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                command.Parameters.Add(errorParam);
                                await command.ExecuteNonQueryAsync();
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                result.Code = CodeHttp.GoodResponseNoContent;
                result.Message= MessageResponse.GoodMessage;
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
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(UpdateProcedures.UpdateRelationCustomerInsurance, connection, transaction))
                            {

                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue(VariableRelation.CustomerInsuranceId, relation.CustomerInsuranceId);
                                command.Parameters.AddWithValue(VariableRelation.StatusId, relation.StatusId);
                                command.Parameters.AddWithValue(VariableRelation.DateModificate, relation.DateModificate);
                                SqlParameter errorParam = new SqlParameter(VariableProcedure.ErrorMessage, SqlDbType.NVarChar, 4000)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                command.Parameters.Add(errorParam);
                                await command.ExecuteNonQueryAsync();
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
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
        private bool CalculateAge(DateTime dateBorn)
        {
            bool legalAge = false;
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
    }
}
