using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using CarService.Context;
using CarService.Code;
using CarService.Forms;

namespace CarService
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            
        }

        SqlConnection con = new SqlConnection(DbContext.stringConnection);
        SqlCommand cmd = new SqlCommand();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            var userLogin = txtLogin.Text;
            var userPassword = md5.HashPassword(txtPassword.Password);
            string quarystring = $"SELECT * FROM tbl_users WHERE login = '{userLogin}'and password='{userPassword}'";
            cmd = new SqlCommand(quarystring, con);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read() == true)
            {
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль, Пожалуйста попробуйте еще раз", "Логин неправильный", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLogin.Text = "";
                txtPassword.Password = "";
                txtPasswordShow.Text = "";
                txtLogin.Focus();
                con.Close();
            }
        }

        private void CheckBoxPas_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            if (CheckBoxPas.IsChecked.Value)
            {
                txtPasswordShow.Text = txtPassword.Password;
                txtPasswordShow.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Hidden;
            }
            else
            {
                txtPassword.Password = txtPasswordShow.Text;
                txtPasswordShow.Visibility = Visibility.Hidden;
                txtPassword.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Forms.ResgistrationForm resgistrationForm = new ResgistrationForm();
            resgistrationForm.Show();
            this.Close();
        }

    }
}
