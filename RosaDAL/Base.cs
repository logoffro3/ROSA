﻿using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace RosaDAL
{
    public abstract class Base
    {
        private SqlDataAdapter adapter;
        protected SqlConnection conn;
        public Base()
        {

            string connstring = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            conn = new SqlConnection(connstring);
            adapter = new SqlDataAdapter();
            OpenConnection();
        }

        protected SqlConnection OpenConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }

            }
            catch
            {
                string exp = "RosaAPP couldn't open the connection";


            }

            return conn;

        }

        private void CloseConnection()
        {
            try
            {
                conn.Close();
            }
            catch
            {
                string exp = "RosaAPP couldn't close the connection";

            }

        }

        /* For Insert/Update/Delete Queries with transaction */
        protected void ExecuteEditTranQuery(String query, SqlParameter[] sqlParameters, SqlTransaction sqlTransaction)
        {
            SqlCommand command = new SqlCommand(query, conn, sqlTransaction);
            try
            {
                command.Parameters.AddRange(sqlParameters);
                adapter.InsertCommand = command;
                command.ExecuteNonQuery();
            }
            catch
            {
                string exp = "RosaAPP couldn't execute query";

                throw;
            }
        }

        /* For Insert/Update/Delete Queries */
        protected void ExecuteEditQuery(String query, SqlParameter[] sqlParameters)
        {
            SqlCommand command = new SqlCommand();

            try
            {
                command.Connection = OpenConnection();
                command.CommandText = query;
                command.Parameters.AddRange(sqlParameters);
                adapter.InsertCommand = command;
                command.ExecuteNonQuery();
            }
            catch
            {
                string exp = "RosaAPP couldn't execute the edit query";

                throw;
            }
            finally
            {
                CloseConnection();
            }
        }



        /* For Select Queries */
        protected DataTable ExecuteSelectQuery(String query, params SqlParameter[] sqlParameters)
        {
            SqlCommand command = new SqlCommand();
            DataTable dataTable;
            DataSet dataSet = new DataSet();

            try
            {
                command.Connection = OpenConnection();
                command.CommandText = query;
                command.Parameters.AddRange(sqlParameters);
                command.ExecuteNonQuery();
                adapter.SelectCommand = command;
                adapter.Fill(dataSet);
                dataTable = dataSet.Tables[0];
            }
            catch
            {
                string exp = "RosaAPP couldn't execute the select query";

                return null;

            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }
    }
}
