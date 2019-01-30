using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace DatabaseComponent
{
    public class DatabaseAccessLayer
    {
        private static string _connectionString;
        private List<MySqlParameter> _mysqlParameters;

        public DatabaseAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
            _mysqlParameters = new List<MySqlParameter>();
        }
   
        //Execute normal query with or without params
        public DataSet runSQLDataSet(string _query)
        {
            DataSet _dataSet = new DataSet();
            if (validateSQLSatement(_query))
            {
                MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString);
                MySqlCommand _mysqlCommand = new MySqlCommand(_query, _mysqlConnection);
                MySqlDataAdapter _mysqldataAdapter = new MySqlDataAdapter(_mysqlCommand);
                if (_mysqlParameters.Count != 0)
                {
                    foreach (var item in _mysqlParameters)
                    {
                        _mysqlCommand.Parameters.Add(item.ParameterName, item.MySqlDbType).Value = item.Value;
                    }
                }
                try
                {
                    _mysqldataAdapter.Fill(_dataSet);
                    return _dataSet;
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    _mysqlParameters.Clear();
                    _mysqlConnection.Dispose();
                }

            }
            return null;
        }

        //Execute SP with or without params
        public DataSet runSPDataSet(string _storedProcedure)
        {
            DataSet _dataSet = new DataSet();
            if (validateSP(_storedProcedure))
            {
                MySqlConnection _mysqlConnection = new MySqlConnection(_connectionString);
                MySqlCommand _mysqlCommand = new MySqlCommand(_storedProcedure, _mysqlConnection);
                _mysqlCommand.CommandType = CommandType.StoredProcedure;
                if (_mysqlParameters.Count != 0)
                {
                    foreach (var item in _mysqlParameters)
                    {
                        _mysqlCommand.Parameters.Add(item.ParameterName, item.MySqlDbType).Value = item.Value;
                    }
                }
                MySqlDataAdapter _dataAdapter = new MySqlDataAdapter(_mysqlCommand);
                try
                {
                    _mysqlConnection.Open();
                    _dataAdapter.Fill(_dataSet);
                    return _dataSet;
                }
                catch (MySqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    _mysqlParameters.Clear();
                    _mysqlConnection.Dispose();
                }
            }
            return null;
        }

        //AddParams
        public MySqlParameter AddParameter(string _parameterKey, object _parameterValue, MySqlDbType _mysqlDbType)
        {
            MySqlParameter _parameter = new MySqlParameter(_parameterKey, _parameterValue);
            _parameter.MySqlDbType = _mysqlDbType;
            _mysqlParameters.Add(_parameter);
            return _parameter;
        }

        private bool validateSQLSatement(string _sqlStatement)
        {
            if (_sqlStatement.Length < 10)
            {
                Exception ex = new Exception("The SQL Statement must be provided and at least 10 characters long");

                return false;

            }
            return true;
        }

        private bool validateSP(string _storedProcedure)
        {
            if (_storedProcedure.Length < 2)
            {
                return false;
                throw new Exception("The Stored Procedure must be provided and at least 2 characters long");
            }
            return true;
        }
    }
}
