using CarService.Code;
using CarService.Context;
using CarService.Reports;
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
    /// Логика взаимодействия для MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection);
        public MainForm()
        {
            InitializeComponent();
            Codes.UpdateGrid("orderT", DataGrid);
            FillCbWorkType();
            FillCbEmployee();
        }

        public void FillCbWorkType()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from workTypeT", sqlConnection);
                sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                workTypeTb.ItemsSource = dt.DefaultView;
                workTypeTb.DisplayMemberPath = "workName";
                workTypeTb.SelectedValuePath = "workTypeId";
                cmd.Dispose();
                sqlConnection.Close();
            }
            catch (Exception)
            {
            }
        }
        public void FillCbEmployee()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from employeeT", sqlConnection);
                sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                employeeNameTb.ItemsSource = dt.DefaultView;
                employeeNameTb.DisplayMemberPath = "fullName";
                employeeNameTb.SelectedValuePath = "employeeId";
                cmd.Dispose();
                sqlConnection.Close();
            }
            catch (Exception)
            {
            }
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand("sp_InsertOrder", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            


            sqlCommand.Parameters.Add(new SqlParameter("@orderId", orderIdTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@clientId", clientIdTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@carModel", carModelTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@workType", workTypeTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@price", priceTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@employeeName", employeeNameTb.Text));
            sqlCommand.Parameters.Add(new SqlParameter("@completed", completedTb.IsChecked));


            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            
            sqlConnection.Close();

            Codes.UpdateGrid("orderT", DataGrid);
        }


        private void clientAdd_Click(object sender, RoutedEventArgs e)
        {
            clientWindow clientWindow = new clientWindow();
            clientWindow.Show();

        }

        private void workTypeAdd_Click(object sender, RoutedEventArgs e)
        {
            WorkTypeWindow workTypeWindow = new WorkTypeWindow();
            workTypeWindow.Show();
        }

        private void employeeAdd_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow employeeWindow = new employeeWindow();
            employeeWindow.Show();
        }

        private void workTypeTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                        
        }

        private void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            Codes.UpdateGrid("orderT", DataGrid);
            FillCbWorkType();
            FillCbEmployee();
        }

        private void spisClient_Click(object sender, RoutedEventArgs e)
        {
            ClientReport.GetClient();
        }

        private void spisOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderReport.GetOrders();
        }

        public void UpdateGridAll()
        {
            string quary = $"select * from orderT";
            using (SqlConnection sqlConnection = new SqlConnection(DbServiceContext.stringConnection))
            {

                sqlConnection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(quary, sqlConnection);
                SqlCommandBuilder sqlbuilder = new SqlCommandBuilder(adapter);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "orderT");

                DataGrid.ItemsSource = ds.Tables["orderT"].DefaultView;
                sqlConnection.Close();
            }
        }
        private void all_Checked(object sender, RoutedEventArgs e)
        {
            UpdateGridAll();
        }

        private void id_Checked(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("sp_FindIdOrder", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@orderID", findTb.Text));
            DataTable DT = new DataTable();

            sqlConnection.Open();
            DT.Load(command.ExecuteReader());
            sqlConnection.Close();
            DataGrid.ItemsSource = DT.DefaultView;
        }

        private void ClientId_Checked(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("sp_FindIdClient", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@clientId", findTb.Text));
            DataTable DT = new DataTable();

            sqlConnection.Open();
            DT.Load(command.ExecuteReader());
            sqlConnection.Close();
            DataGrid.ItemsSource = DT.DefaultView;
        }

        private void BDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DataRowView item = DataGrid.SelectedItem as DataRowView;

                if (item != null)
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_DeleteFromOrderT", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.Add(new SqlParameter("@orderId", item.DataView[DataGrid.SelectedIndex][0].ToString().Trim()));
                    sqlConnection.Open();
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        Codes.UpdateGrid("orderT", DataGrid);
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

        private void BClientT_Click(object sender, RoutedEventArgs e)
        {
            clientWindow clientWindow = new clientWindow();
            clientWindow.Show();
        }

        private void name_Checked(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("sp_FindCarModel", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@carModel", findTb.Text));
            DataTable DT = new DataTable();

            sqlConnection.Open();
            DT.Load(command.ExecuteReader());
            sqlConnection.Close();
            DataGrid.ItemsSource = DT.DefaultView;
        }

        private void servicetype_Checked(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand("sp_FindWorkType", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@workType", findTb.Text));
            DataTable DT = new DataTable();

            sqlConnection.Open();
            DT.Load(command.ExecuteReader());
            sqlConnection.Close();
            DataGrid.ItemsSource = DT.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clientWindow clientWindow = new clientWindow();
            clientWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            employeeWindow employeeWindow = new employeeWindow();
            employeeWindow.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lMenu.Visibility = Visibility.Hidden;
            bAddClient.Visibility = Visibility.Hidden;
            bAddEmployee.Visibility = Visibility.Hidden;
            bOrderMain.Visibility = Visibility.Hidden;

            lCarModel.Visibility = Visibility.Visible;
            lEmployee.Visibility = Visibility.Visible;
            lIdClien.Visibility = Visibility.Visible;
            lIdOrder.Visibility = Visibility.Visible;
            lPrice.Visibility = Visibility.Visible;
            lWorkType.Visibility = Visibility.Visible;

            BAdd.Visibility = Visibility.Visible;
            BUpdate.Visibility = Visibility.Visible;
            BDelete.Visibility = Visibility.Visible;
            clientIdTb.Visibility = Visibility.Visible;
            orderIdTb.Visibility = Visibility.Visible;
            carModelTb.Visibility = Visibility.Visible;
            priceTb.Visibility = Visibility.Visible;
            workTypeTb.Visibility = Visibility.Visible;
            employeeNameTb.Visibility= Visibility.Visible;
            grB1.Visibility = Visibility.Visible;
        }
    }
}
