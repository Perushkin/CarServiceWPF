using CarService.Code;
using CarService.Context;
using System;
using System.Collections.Generic;
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

namespace CarService.Forms
{
    /// <summary>
    /// Логика взаимодействия для ResgistrationForm.xaml
    /// </summary>
    public partial class ResgistrationForm : Window
    {
        SqlConnection con = new SqlConnection(DbContext.stringConnection);

        public ResgistrationForm()
        {
            InitializeComponent();
        }

        private void Button_Click_Registration(object sender, RoutedEventArgs e)
        {
            var userLogin = txtLogin.Text;
            var userPassword = md5.HashPassword(txtPassword.Text);

            string quarystring = $"insert into tbl_users (login, password) values ('{userLogin}', '{userPassword}')";

            SqlCommand command = new SqlCommand(quarystring, con);
            con.Open();

            if (command.ExecuteNonQuery() != 1)
            {
                MessageBox.Show("Аккаунт не был зарегистрирован");
            }

            else
            {
                MessageBox.Show("Аккаунт зарегистрирован");
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }

            con.Close();          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();

        }
    }
}
