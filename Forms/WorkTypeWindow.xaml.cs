using CarService.Code;
using CarService.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarService
{
    /// <summary>
    /// Логика взаимодействия для WorkTypeWindow.xaml
    /// </summary>
    public partial class WorkTypeWindow : Window
    {
        SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection);
        public WorkTypeWindow()
        {
            InitializeComponent();
            Codes.UpdateGrid("workTypeT", gridViewWork);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_InsertWorkType", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.Add(new SqlParameter("@workTypeId", workTypeIdTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@workName", workNameTb.Text));

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            Codes.UpdateGrid("workTypeT", gridViewWork);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView item = gridViewWork.SelectedItem as DataRowView;

                if (item != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_DeleteFromWorkTypeT", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.Add(new SqlParameter("@workTypeId", item.DataView[gridViewWork.SelectedIndex][0].ToString().Trim()));
                    sqlConnection.Open();
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        Codes.UpdateGrid("workTypeT", gridViewWork);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }
    }
}
