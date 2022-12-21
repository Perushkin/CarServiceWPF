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
    internal class OrderReport
    {
        public static void GetOrders()
        {
            SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection);

            SqlCommand command = new SqlCommand("sp_SelectFromOrderT", sqlConnection)
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
            ex.Range["A2:G2"].Value = $"Список заказов";

            ex.Range["A4"].Value = $"№";
            ex.Range["A4"].ColumnWidth = 4;
            ex.Range["B4"].Value = $"ID Заказа";
            ex.Range["B4"].ColumnWidth = 10;
            ex.Range["C4"].Value = $"ID Клиента";
            ex.Range["C4"].ColumnWidth = 10;
            ex.Range["D4"].Merge();
            ex.Range["D4"].Value = $"Автомобиль";
            ex.Range["D4"].ColumnWidth = 40;
            ex.Range["E4"].Merge();
            ex.Range["E4"].Value = $"Тип работы";
            ex.Range["E4"].ColumnWidth = 20;
            ex.Range["F4"].Merge();
            ex.Range["F4"].Value = $"Цена";
            ex.Range["F4"].ColumnWidth = 10;
            ex.Range["G4"].Merge();
            ex.Range["G4"].Value = $"Сотрудник";
            ex.Range["G4"].ColumnWidth = 30;
            ex.Range["H4"].Merge();
            ex.Range["H4"].Value = $"Состояние заказа";
            ex.Range["H4"].ColumnWidth = 15;


            ex.Range["A4:G4"].Font.Name = "Arial";
            ex.Range["A4:G4"].Font.Size = "10";
            ex.Range["A4:G4"].Font.Bold = true;


            int i = 5;
            foreach (DataRow dr in table.Rows)
            {
                ex.Range[$"A{i}"].Value = i - 4;
                ex.Range[$"B{i}"].Value = dr["IdOrder"].ToString();
                ex.Range[$"C{i}"].Value = dr["IdClient"].ToString();
                ex.Range[$"D{i}"].Value = dr["Car model"].ToString();
                ex.Range[$"E{i}"].Value = dr["work type"].ToString();
                ex.Range[$"F{i}"].Value = dr["price"].ToString();
                ex.Range[$"G{i}"].Value = dr["Name employee"].ToString();
                ex.Range[$"H{i}"].Value = dr["completed"].ToString();


                ex.Range[$"D{i}"].Merge();
                ex.Range[$"F{i}"].Merge();

                i++;
            }
            ex.Range[$"A4:H{i - 1}"].Borders.LineStyle = 1;
            ex.Range[$"A4:H{i - 1}"].Borders.Weight = 2;



        }
    }
}
