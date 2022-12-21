using CarService.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace CarService.Reports
{
    internal class ClientReport
    {
        public static void GetClient()
        {
            SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection);

            SqlCommand command = new SqlCommand("sp_SelectFromClientT", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            DataTable table = new DataTable();
            sqlConnection.Open();
            table.Load(command.ExecuteReader());
            sqlConnection.Close();

            //Обьявим приложение 
            Excel.Application ex = new Excel.Application();
            //отобразим excel
            ex.Visible = true;
            //добавит 2 листа в рабочию книгу
            ex.SheetsInNewWorkbook = 2;
            //добавим рабочую книгу 
            Excel.Workbook workbook = ex.Workbooks.Add(Type.Missing);
            //Отключим окна с сообщениями
            ex.DisplayAlerts = false;
            //Получаем первый лист документ (счёт начинается с 1 )
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);

            ex.Range["A2:G2"].Merge();
            ex.Range["A2:G2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ex.Range["A2:G2"].Value = $"Список клиентов";

            ex.Range["A4"].Value = $"№";
            ex.Range["A4"].ColumnWidth = 4;
            ex.Range["B4"].Value = $"ID Клиента";
            ex.Range["B4"].ColumnWidth = 10;
            ex.Range["C4"].Value = $"ФИО";
            ex.Range["C4"].ColumnWidth = 30;
            ex.Range["D4:E4"].Merge();
            ex.Range["D4:E4"].Value = $"Номер телефона";
            ex.Range["D4"].ColumnWidth = 20;
            ex.Range["F4:G4"].Merge();
            ex.Range["F4:G4"].Value = $"Адрес";
            ex.Range["F4"].ColumnWidth = 40;


            ex.Range["A4:G4"].Font.Name = "Arial";
            ex.Range["A4:G4"].Font.Size = "10";
            ex.Range["A4:G4"].Font.Bold = true;


            int i = 5;
            foreach (DataRow dr in table.Rows)
            {
                ex.Range[$"A{i}"].Value = i - 4;
                ex.Range[$"B{i}"].Value = dr["Id client"].ToString();
                ex.Range[$"C{i}"].Value = dr["Name"].ToString();
                ex.Range[$"D{i}:E{i}"].Value = dr["phone"].ToString();
                ex.Range[$"F{i}:G{i}"].Value = dr["adress"].ToString();

                ex.Range[$"D{i}:E{i}"].Merge();
                ex.Range[$"F{i}:G{i}"].Merge();

                i++;
            }
            ex.Range[$"A4:G{i - 1}"].Borders.LineStyle = 1;
            ex.Range[$"A4:G{i - 1}"].Borders.Weight = 2;



        }
    }
}
