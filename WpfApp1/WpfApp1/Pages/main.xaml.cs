using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using workingBD;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для main.xaml
    /// </summary>
    public partial class main : Page
    {
        public MainWindow mainWindow;
        public main(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            Loader();
        }

        List<field> fields = new List<field>();

        public void Loader()
        {
            parrent.Children.Clear();

            for (int i = 0; i < mainWindow.tests.Count; i++)
            {
                var bc = new BrushConverter();
                Grid global = new Grid();

                DockPanel dockPanel = new DockPanel();

                global.Margin = new Thickness(0, 5, 0, 0);


                Border myBorder = new Border();
                myBorder.Background = Brushes.Transparent;
                myBorder.BorderBrush = (Brush)bc.ConvertFrom("#FEB05E");
                myBorder.BorderThickness = new Thickness(5);
                myBorder.CornerRadius = new CornerRadius(25);
                myBorder.Padding = new Thickness(10, 10, 10, 10);
                myBorder.Width = 250;


                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Vertical;


                string[] _value = new string[] { };
                _value = mainWindow.tests[i].values.ToString().Replace("{", "").Replace("}", "").Replace("\"", "").Split(new string[] { "," }, StringSplitOptions.None);
                for (int j = 0; j < _value.Length; j++)
                {
                    string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                    TextBlock ValueText = new TextBlock();

                    ValueText.Text = strings[0].ToString().Trim();

                    TextBox text = new TextBox();
                    text.Margin = new Thickness(25, 0, 0, 0);
                    text.TextWrapping = TextWrapping.Wrap;
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    if (strings.Length > 1)
                    {
                        string t = strings[0].ToString().Replace(" ", "");
                        string t2 = strings[1].ToString().Replace("|", ",");
                        t = t.Trim();
                        t2 = t2.Trim();
                        text.Text = t2;

                    }
                    else
                    {
                        string t = strings[0].ToString().Replace(" ", "");
                        t = t.Trim();
                        text.Text = " ";
                    }
                    stack.Children.Add(ValueText);
                    stack.Children.Add(text);
                }

                Button delte = new Button();
                delte.Content = "Удалить";
                delte.HorizontalAlignment = HorizontalAlignment.Right;
                delte.Margin = new Thickness(0, 15, 0, 15);
                delte.Tag = i;
                delte.Click += async delegate
                {
                    string _id = mainWindow.values[Convert.ToInt32(delte.Tag)].id;
                    workingBD.workingDB.Deleter(_id);
                    mainWindow.values.Clear();
                    workingDB.loader(mainWindow.values);
                    Loader();
                };
                stack.Children.Add(delte);

                Button edit = new Button();
                edit.Content = "Изменить";
                edit.HorizontalAlignment = HorizontalAlignment.Right;
                edit.Margin = new Thickness(5, 0, 0, 0);
                edit.Tag = i;
                edit.Click += async delegate
                {
                    string _id = mainWindow.values[Convert.ToInt32(edit.Tag)].id;
                    Editer(_id);
                };
                stack.Children.Add(edit);



                myBorder.Child = stack;

                dockPanel.Children.Add(myBorder);

                parrent.Children.Add(dockPanel);
            }
        }

        public async void Editer(string _id)
        {
            mainWindow.frame.Navigate(new Pages.editing.edit(mainWindow, _id));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.OpenPages(MainWindow.pages.import);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            //mainWindow.OpenPages(MainWindow.pages.add);
            mainWindow.frame.Navigate(new Pages.editing.add(mainWindow));
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            filter.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            filter.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            int id = 0;
            for (int i = 0; i < fields.Count; i++)
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

        public void Create()
        {
            search.Children.Clear();
            for (int i = 0; i < fields.Count; i++)
            {
                var bc = new BrushConverter();
                Grid global = new Grid();
                global.Margin = new Thickness(0, 5, 0, 0);

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                TextBox title = new TextBox();
                title.Text = fields[i].title;
                title.HorizontalAlignment = HorizontalAlignment.Left;
                title.Tag = i;
                title.TextChanged += delegate
                {
                    string t = title.Text;
                    fields[Convert.ToInt32(title.Tag)].title = t;

                };

                stack.Children.Add(title);

                TextBox value = new TextBox();
                value.Text = fields[i].values;
                value.HorizontalAlignment = HorizontalAlignment.Right;
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
                search.Children.Add(global);
            }
        }


        List<workingBD.Values> noDupes = new List<workingBD.Values>();

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            List<workingBD.Values> values = new List<workingBD.Values>();
            values.Clear();

            for (int i = 0; i < mainWindow.values.Count; i++)
            {
                string value = string.Empty;
                string title = mainWindow.values[i].values;
                for (int j = 0; j < fields.Count; j++)
                {
                    title = title.ToString().Trim();
                    if (fields[j].values != "" || fields[j].values != " ")
                    {

                        value += fields[j].title + " : " + fields[j].values;
                        if (title.Contains(value.Trim()))
                        {
                            values.Add(new Values(
                            mainWindow.values[i].id,
                            mainWindow.values[i].values
                        ));
                        }
                    }
                    else
                    {
                        if (title.Contains(fields[j].title))
                        {
                            values.Add(new Values(
                           mainWindow.values[i].id,
                           mainWindow.values[i].values
                           ));
                        }
                    }
                }
            }

            noDupes = values.GroupBy(x => x.id).Select(x => x.First()).ToList();



            searchlist(noDupes);

        }


        public void searchlist(List<workingBD.Values> values)
        {
            parrent.Children.Clear();
            for (int i = 0; i < values.Count; i++)
            {
                var bc = new BrushConverter();
                Grid global = new Grid();
                global.Margin = new Thickness(0, 5, 0, 0);

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                Button delte = new Button();
                delte.Content = "Удалить";
                delte.HorizontalAlignment = HorizontalAlignment.Right;
                delte.Margin = new Thickness(0, 0, 0, 0);
                delte.Tag = i;
                delte.Click += async delegate
                {
                    string _id = values[Convert.ToInt32(delte.Tag)].id;
                    workingBD.workingDB.Deleter(_id);
                    mainWindow.values.Clear();
                    workingDB.loader(mainWindow.values);
                    Loader();
                };

                stack.Children.Add(delte);

                Button edit = new Button();
                edit.Content = "Изменить";
                edit.HorizontalAlignment = HorizontalAlignment.Right;
                edit.Margin = new Thickness(5, 0, 0, 0);
                edit.Tag = values[i].id;
                edit.Click += async delegate
                {
                    string _id = edit.Tag.ToString();
                    Editer(_id);
                };

                stack.Children.Add(edit);

                string value = values[i].values;

                string[] _value = new string[] { };

                _value = value.Split(new string[] { "," }, StringSplitOptions.None);

                for (int j = 0; j < _value.Length; j++)
                {
                    Grid Value = new Grid();
                    TextBlock ValueText = new TextBlock();

                    ValueText.Text = _value[j].ToString().Replace("|", ",");
                    ValueText.Margin = new Thickness(50, 5, 0, 0);
                    Value.Children.Add(ValueText);
                    stack.Children.Add(Value);
                }

                global.Children.Add(stack);
                parrent.Children.Add(global);
            }
        }

        public class vales
        {
            public string id { get; set; }
            public vales(string id)
            {
                this.id = id;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

            Loader();
        }

        List<vales> list = new List<vales>();
        public void DisplayInExcel(IEnumerable<vales> accounts)
        {
            var excelApp = new Excel.Application();
            excelApp.Workbooks.Add();
            excelApp.Visible = true;



            List<vales> _list = accounts.ToList();

            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < _list.Count; i++)
            {
                excelApp.Range[$"{letters[i]}1"].Value = _list[i].id;
            }

            Excel._Worksheet worksheet = (Excel.Worksheet)excelApp.ActiveSheet;

            if (noDupes.Count > 0)
            {
                for (int i = 0; i < noDupes.Count; i++)
                {
                    string value = noDupes[i].values;

                    string[] _value = value.Split(new string[] { ", " }, StringSplitOptions.None);
                    for (int j = 0; j < _value.Length; j++)
                    {

                        int row = i + 2;
                        string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                        string t = "";
                        try
                        {
                            t = strings[1].ToString().Replace("|", ",");
                        }
                        catch
                        {

                        }

                        worksheet.Cells[row, $"{letters[j]}"] = t;
                    }
                }
            }
            else
            {
                for (int i = 0; i < mainWindow.values.Count; i++)
                {
                    string value = mainWindow.values[i].values;

                    string[] _value = value.Split(new string[] { ", " }, StringSplitOptions.None);
                    for (int j = 0; j < _value.Length; j++)
                    {

                        int row = i + 2;
                        string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                        string t = "";
                        try
                        {
                            t = strings[1].ToString().Replace("|", ",");
                        }
                        catch
                        {

                        }

                        worksheet.Cells[row, $"{letters[j]}"] = t;
                    }
                }
            }


            for (int i = 1; i < _list.Count; i++)
            {
                worksheet.Columns[i].AutoFit();
            }


        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (noDupes.Count > 0)
            {
                for (int i = 0; i < noDupes.Count; i++)
                {

                    string value = noDupes[0].values;

                    string[] _value = value.Split(new string[] { ", " }, StringSplitOptions.None);

                    for (int j = 0; j < _value.Length; j++)
                    {
                        string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                        string t = strings[0].ToString();

                        list.Add(new vales(
                        t
                        ));
                    }
                }
            }
            else
            {
                for (int i = 0; i < mainWindow.values.Count; i++)
                {

                    string value = mainWindow.values[0].values;

                    string[] _value = value.Split(new string[] { ", " }, StringSplitOptions.None);

                    for (int j = 0; j < _value.Length; j++)
                    {
                        string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                        string t = strings[0].ToString();

                        list.Add(new vales(
                        t
                        ));
                    }
                }
            }

            List<vales> _noDupes = list.GroupBy(x => x.id).Select(x => x.First()).ToList();

            DisplayInExcel(_noDupes);
        }
    }
}