using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using SmartLocker.Software.Backend.Repositories.Interfaces;

namespace SmartLocker.Software.Backend.Repositories.Implements
{
    public class BaseRepository : IBaseRepository
    {

        private readonly string _ConnectionString;

        public BaseRepository(string ConnectionString)
        {
            this._ConnectionString = ConnectionString;
        }

        //Method Connect to Sql Server
        private T WithConnection<T>(Func<IDbConnection, T> getData)
        {
            try
            {
                using (var connection = new SqlConnection(_ConnectionString))
                {
                    connection.Open();
                    return getData(connection);
                }
            }
            catch (SqlException e)
            {

                throw e;
            }
        }

        //Method Stored procedure
        public int ExecuteStoreProcedure<T>(string spName, DynamicParameters dynamic)
        {
            return WithConnection(x => x.Execute(spName, dynamic, commandType: CommandType.StoredProcedure));
        }

        public IEnumerable<T> QueryStoreProcedure<T>(string spName, DynamicParameters dynamic)
        {
            return WithConnection(x => x.Query<T>(spName, dynamic, commandType: CommandType.StoredProcedure));
        }

        //Method Query string
        public int ExecuteString<T>(string sql)
        {
            return WithConnection(c => c.Execute(sql));
        }

        public IEnumerable<T> QueryString<T>(string sql)
        {
            return WithConnection(c => c.Query<T>(sql));
        }

       
    }
}