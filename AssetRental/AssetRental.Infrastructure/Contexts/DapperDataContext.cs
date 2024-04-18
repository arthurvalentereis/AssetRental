using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using AssetRental.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AssetRental.Infrastructure.Contexts
{
    public interface IDapper : IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType);
        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType);
        int Execute(string sp, DynamicParameters parms, CommandType commandType);
        T Insert<T>(string sp);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType);
        T Update<T>(string sp);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType);
        DataTable ObterDataTable(string sql);
        DataTable ObterDataTable(string sql, DynamicParameters parms);
        DataTable ObterDataTable(DataTable dt, string sql, DynamicParameters parms);
    }
    public class DapperDataContext
    {
        private readonly IConfiguration _config;
        //private string Connectionstring = "localDatabase";
        private string ConnectionString { get; }
        private string DataSource { get; }
        private const int TimeoutPadrãoSegundos = 300;

        public DapperDataContext(IOptions<DapperSettings>? dapperSettings)
        {
            ConnectionString = dapperSettings.Value.SqlServer;
            var valoresConnection = ConnectionString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(
                x => x.Split('=')[0].ToLower().Replace(" ", "_"),
                x => x.Split('=')[1]);
            DataSource = valoresConnection["data_source"];
            //_config = config;
        }
        public void Dispose()
        {

        }
        public int Execute(string sp, DynamicParameters parms, CommandType commandType)
        {
            throw new NotImplementedException();
        }
        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }
        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }
        public List<T> GetAllTeste<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }


        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var list = await db.QueryAsync<T>(sp, parms, commandType: commandType);
            return list.ToList();
        }

        public T Insert<T>(string sp)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                try
                {
                    result = db.Query<T>(sp).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
        public T Update<T>(string sp)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                try
                {
                    result = db.Query<T>(sp).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
        public DataTable ObterDataTable(string sql)
        {
            DataTable dt = null;
            using var connection = ObterConexaoRaiz();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var comando = new SqlCommand(sql, connection, transaction) { CommandTimeout = 0 };

            using (SqlDataReader reader = comando.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                }
            }
            transaction.Commit();
            return dt;
        }

        public DataTable ObterDataTable(string sql, DynamicParameters parms)
        {
            DataTable dt = new DataTable();
            using var connection = ObterConexaoRaiz();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            dt.Load(connection.ExecuteReader(sql, (object)parms, transaction: transaction));

            transaction.Commit();
            return dt;

            //DataTable dt = new DataTable();
            //using IDbConnection db = ObterConexaoRaiz();

            ////connection.Open();
            ////using var transaction = connection.BeginTransaction();

            //dt.Load(db.QueryAsync<DataTable>(sql, parms, CommandType.Text));

            //return dt;

            //DataTable dt = null;
            //using var connection = ObterConexaoRaiz();
            //connection.Open();
            //using var transaction = connection.BeginTransaction();
            //using var comando = new SqlCommand(sql, connection, transaction) { CommandTimeout = 0 };

            //using (SqlDataReader reader = comando.ExecuteReader())
            //{
            //    if (reader.HasRows)
            //    {
            //        dt = new DataTable();
            //        dt.Load(reader);
            //    }
            //}
            //transaction.Commit();
            //return dt;
        }
        public DataTable ObterDataTable(DataTable dt, string sql, DynamicParameters parms)
        {
            using var connection = ObterConexaoRaiz();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            dt.Load(connection.ExecuteReader(sql, (object)parms, transaction: transaction));

            transaction.Commit();
            return dt;

            //DataTable dt = new DataTable();
            //using IDbConnection db = ObterConexaoRaiz();

            ////connection.Open();
            ////using var transaction = connection.BeginTransaction();

            //dt.Load(db.QueryAsync<DataTable>(sql, parms, CommandType.Text));

            //return dt;

            //DataTable dt = null;
            //using var connection = ObterConexaoRaiz();
            //connection.Open();
            //using var transaction = connection.BeginTransaction();
            //using var comando = new SqlCommand(sql, connection, transaction) { CommandTimeout = 0 };

            //using (SqlDataReader reader = comando.ExecuteReader())
            //{
            //    if (reader.HasRows)
            //    {
            //        dt = new DataTable();
            //        dt.Load(reader);
            //    }
            //}
            //transaction.Commit();
            //return dt;
        }
        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_config.GetConnectionString(ConnectionString));
        }
        public SqlConnection ObterConexaoRaiz()
        {
            return new SqlConnection(ConnectionString);
        }
        public SqlConnection ObterConexao(string NameDB)
        {
            if (string.IsNullOrWhiteSpace(NameDB))
                throw new Exception("Nome de banco de dados inválido!");
            var connectionString = MontarNovaConnectionString(NameDB);
            return new SqlConnection(connectionString);
        }
        private string MontarNovaConnectionString(string nomeBanco) =>
            $"Data Source={DataSource};Initial Catalog={nomeBanco};Integrated Security=true;Connect Timeout={TimeoutPadrãoSegundos};TrustServerCertificate=True;";

    }
}
