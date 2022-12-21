using CarService.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CarService.Code
{
    internal class Codes
    {
        public static void UpdateGrid(string table, DataGrid dataGrid)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection))
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"select * from {table}", sqlConnection);

                sqlConnection.Open();
                sqlDataAdapter.Fill(dataTable);
                dataGrid.DataContext = dataTable.DefaultView;
            }

            //using (SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection))
            //{
            //    DataTable dataTable = new DataTable();
            //    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"select * from orderT", sqlConnection);

            //    sqlConnection.Open();
            //    sqlDataAdapter.Fill(dataTable);
            //    dataGrid.DataContext = dataTable.DefaultView;
            //}
        }
    }
}
