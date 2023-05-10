using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using workingBD;
using static MongoDB.Driver.WriteConcern;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.Pages.editing
{
    /// <summary>
    /// Логика взаимодействия для add.xaml
    /// </summary>
    public partial class add : Page
    {
        public MainWindow mainWindow;
        public add(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }



        List<field> fields = new List<field>();
        public void Create()
        {
            parrent.Children.Clear();
            for (int i = 0; i < fields.Count; i++)
            {
                var bc = new BrushConverter();
                Grid global = new Grid();
                global.Margin = new Thickness(0, 5, 0, 0);

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                TextBox title = new TextBox();
                title.Text = fields[i].title;
                title.MinWidth = 150;
                title.Margin = new Thickness(5, 5, 0, 0);
                title.Tag = i;
                title.TextChanged += delegate
                {
                    string t = title.Text;
                    fields[Convert.ToInt32(title.Tag)].title = t;

                };

                stack.Children.Add(title);

                TextBox value = new TextBox();
                value.MinWidth = 150;
                value.Text = fields[i].values;
                value.Margin = new Thickness(30, 5, 0, 0);
                value.Tag = i;
                value.TextChanged += delegate
                {
                    string t = value.Text;
                    fields[Convert.ToInt32(value.Tag)].values = t;

                };
                stack.Children.Add(value);

                Button cancel = new Button();
                cancel.Content = "-";
                cancel.HorizontalAlignment = HorizontalAlignment.Left;
                cancel.VerticalAlignment = VerticalAlignment.Bottom;
                cancel.Height = 25;
                cancel.Width = 25;
                cancel.Margin = new Thickness(25, 5, 0, 0);
                cancel.Tag = fields[i].id;
                cancel.Click += async delegate
                {
                    var itemRemove = fields.Single(r => r.id == Convert.ToInt32(cancel.Tag)); 
                    fields.Remove(itemRemove);
                    Create();
                };
                stack.Children.Add(cancel);

                global.Children.Add(stack);
                parrent.Children.Add(global);
            }
        }

        private void add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string result = string.Empty;
            for (int j = 0; j < fields.Count; j++)
            {
                string _drv = String.Empty;

                try
                {
                    result += "\"" + fields[j].title.ToString().Trim() + "\":\"" + fields[j].values.ToString().Replace(",", "|").Trim() + "\", ";
                }
                catch
                {

                }
            }
            result = result.Remove(result.Length - 2);
            workingBD.workingDB.Improt(result);

            mainWindow.tests.Clear();

            workingDB.loaderTest(mainWindow.tests);

            mainWindow.OpenPages(MainWindow.pages.main);
        }

        private void addPanel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int id  = 0;
            for(int i =0;i < fields.Count; i++)
            {
                if (fields[i].id >= id)
                {
                    id = fields[i].id + 1;
                }
            }
            fields.Add(new field(
                    id,
                    $"Name {id}",
                    " "
                    ));

            Create();
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPages(MainWindow.pages.main);
        }
    }
}
