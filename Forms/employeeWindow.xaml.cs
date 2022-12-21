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
    /// Логика взаимодействия для employeeWindow.xaml
    /// </summary>
    public partial class employeeWindow : Window
    {
        SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection);
        public employeeWindow()
        {
            InitializeComponent();
            Codes.UpdateGrid("employeeT", gridView);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_InsertEmployee", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.Add(new SqlParameter("@employeeId", employeeIdTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@fullName", fullNameTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@position", positionTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@experience", experienceTb.Text));



            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            Codes.UpdateGrid("employeeT", gridView);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            editBtn.IsEnabled = true;
            saveBtn.IsEnabled = false;
            SqlCommand sqlCommand = new SqlCommand("sp_updateEmployee", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            sqlCommand.Parameters.Add(new SqlParameter("@employeeId", employeeIdTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@fullName", fullNameTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@position", positionTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@experience", experienceTb.Text));

            sqlConnection.Open();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            employeeIdTb.Text = "";


            Codes.UpdateGrid("employeeT", gridView);
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = gridView.SelectedItem as DataRowView;

            if (item != null)
            {
                editBtn.IsEnabled = false;
                employeeIdTb.Text = item.DataView[gridView.SelectedIndex][0].ToString().Trim();
                fullNameTb.Text = item.DataView[gridView.SelectedIndex][1].ToString().Trim();
                positionTb.Text = item.DataView[gridView.SelectedIndex][2].ToString().Trim();
                experienceTb.Text = item.DataView[gridView.SelectedIndex][3].ToString().Trim();               
            }
            else
                MessageBox.Show("Не указана запись для редактирования", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView item = gridView.SelectedItem as DataRowView;

                if (item != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_DeleteFromEmployeeT", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.Add(new SqlParameter("@employeeId", item.DataView[gridView.SelectedIndex][0].ToString().Trim()));
                    sqlConnection.Open();
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        Codes.UpdateGrid("employeeT", gridView);
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
