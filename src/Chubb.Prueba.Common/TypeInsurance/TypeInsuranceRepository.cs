using Chubb.Prueba.BL.TypeInsurance;
using Chubb.Prueba.Const.CodeHttp;
using Chubb.Prueba.Const.Const;
using Chubb.Prueba.Const.MessageResponse;
using Chubb.Prueba.Const.VariableProcedure;
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
    public class TypeInsuranceRepository : ITypeInsuranceRepository
    {
        private readonly string _connectionString;
        public TypeInsuranceRepository(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ResultResponse> GetTypeInsurance(bool legalAge)
        {
            var result = new ResultResponse();
            var typeInsuranceList = new List<TypeInsuranceEntity>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getAllTypeInsurance, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.LegalAge, legalAge);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                typeInsuranceList.Add(new TypeInsuranceEntity
                                {
                                    TypeInsuranceId=reader["TypeInsuranceId"] != DBNull.Value ? (int)reader["TypeInsuranceId"]:default,
                                    NameInsurance=reader["NameInsurance"] != DBNull.Value ? reader["NameInsurance"].ToString():string.Empty,
                                    Prima= reader["Prima"] !=DBNull.Value?(double)reader["Prima"]:default,
                                    SumaAsegurada = reader["SumaAsegurada"] !=DBNull.Value?(double)reader["SumaAsegurada"]:default,
                                    CodeInsurance = reader["CodeInsurance"]!=DBNull.Value? reader["CodeInsurance"].ToString():string.Empty

                                }   
                                    );  
                            }
                            result.Code = typeInsuranceList.Count() == 0 ? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data= typeInsuranceList;
                            result.Message=MessageResponse.GoodMessage;
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

        public async Task<ResultResponse> GetInsuranceByCode(string code)
        {
            var result = new ResultResponse();
            var insuranceList = new List<TypeInsuranceEntity>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(GetProcedures.getInsuranceByCode, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue(VariableProcedure.InsuranceCode, code);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var p =reader["CodeInsurance"].ToString();

                                insuranceList.Add(new TypeInsuranceEntity
                                {
                                    TypeInsuranceId=reader["TypeInsuranceId"]!= DBNull.Value? (int)reader["TypeInsuranceId"]:default,
                                    CodeInsurance =reader["CodeInsurance"] !=DBNull.Value? (string)reader["CodeInsurance"] :string.Empty,
                                    NameInsurance=reader["NameInsurance"]!=DBNull.Value? (string)reader["NameInsurance"]: string.Empty,
                                    Description=reader["Description"]!=DBNull.Value? (string)reader["Description"]: string.Empty,
                                    SumaAsegurada=reader["SumaAsegurada"]!=DBNull.Value? (double)reader["SumaAsegurada"]:default,
                                    Prima=reader["Prima"]!=DBNull.Value? (double)reader["Prima"]:default,
                                    LegalAge=reader["LegalAge"]!=DBNull.Value? (bool)reader["LegalAge"]:false,
                                    StatusId=reader["StatusId"]!=DBNull.Value? (int)reader["StatusId"]:default,
                                }
                                    );
                            }
                            result.Code = insuranceList.Count() == 0 ? CodeHttp.GoodResponseNoContent : CodeHttp.GoodResponseGet;
                            result.Data = insuranceList;
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

        public async Task<ResultResponse> InsertTypeInsurance(TypeInsuranceEntity insurance)
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
                            using (SqlCommand command = new SqlCommand(PostProcedure.InsertTypeInsurance, connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue(VariableInsurance.CodeInsurance, insurance.CodeInsurance);
                                command.Parameters.AddWithValue(VariableInsurance.NameInsurance, insurance.NameInsurance);
                                command.Parameters.AddWithValue(VariableInsurance.Description, insurance.Description);
                                command.Parameters.AddWithValue(VariableInsurance.SumaAsegurada, insurance.SumaAsegurada);
                                command.Parameters.AddWithValue(VariableInsurance.Prima, insurance.Prima);
                                command.Parameters.AddWithValue(VariableInsurance.LegalAge, insurance.LegalAge);
                                command.Parameters.AddWithValue(VariableInsurance.StatusId, insurance.StatusId);
                                command.Parameters.AddWithValue(VariableInsurance.DateCreate, insurance.DateCreate);
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
                result.Message = MessageResponse.GoodMessage;
            }
            catch (Exception ex)
            {
                result.Code = CodeHttp.BadResponse;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultResponse> UpdateTypeInsurance(TypeInsuranceEntity insurance)
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
                            using (SqlCommand command = new SqlCommand(UpdateProcedures.UpdateTypeInsurance, connection, transaction))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue(VariableInsurance.TypeInsuranceId, insurance.TypeInsuranceId);
                                command.Parameters.AddWithValue(VariableInsurance.CodeInsurance,insurance.CodeInsurance);
                                command.Parameters.AddWithValue(VariableInsurance.NameInsurance,insurance.NameInsurance);
                                command.Parameters.AddWithValue(VariableInsurance.Description,insurance.Description);
                                command.Parameters.AddWithValue(VariableInsurance.SumaAsegurada,insurance.SumaAsegurada);
                                command.Parameters.AddWithValue(VariableInsurance.Prima,insurance.Prima);
                                command.Parameters.AddWithValue(VariableInsurance.LegalAge,insurance.LegalAge);
                                command.Parameters.AddWithValue(VariableInsurance.StatusId,insurance.StatusId);
                                command.Parameters.AddWithValue(VariableInsurance.DateModificate,insurance.DateModificate);
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
