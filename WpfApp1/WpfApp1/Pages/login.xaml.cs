using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Page
    {
        MainWindow mainWindow;
        public login(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = pas.Password;
            if(log.Text == "1")
            {
                if(password == "1")
                {
                    mainWindow.OpenPages(MainWindow.pages.main);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}
