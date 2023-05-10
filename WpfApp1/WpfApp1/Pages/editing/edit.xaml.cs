using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using workingBD;

namespace WpfApp1.Pages.editing
{
    /// <summary>
    /// Логика взаимодействия для edit.xaml
    /// </summary>
    public partial class edit : Page
    {
        MainWindow mainWindow;
        string _id;
        public edit(MainWindow mainWindow, string id)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            _id = id;
            Loader(_id);
        }

        
        public class vales
        {
        
            public string title { get ; set; }
            public string value { get; set; }

            public vales(string title, string value)
            {
                this.title = title;
                this.value = value;
            }
        }

        List<vales> _vales = new List<vales>();
        public void Loader(string _id)
        {
            var bc = new BrushConverter();
            Grid global = new Grid();
            global.Margin = new Thickness(0, 5, 0, 0);

            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Vertical;
            string value = string.Empty;
            for (int i =0; i < mainWindow.values.Count; i++)
            {
                if(_id == mainWindow.values[i].id)
                {
                     value = mainWindow.values[i].values;
                }
            }
            
            string[] _value = value.Split(new string[] { ", " }, StringSplitOptions.None);

            

            for (int j = 0; j < _value.Length; j++)
            {

                StackPanel Value = new StackPanel();
                stack.Orientation = Orientation.Vertical;
                TextBlock ValueText = new TextBlock();
                string[] strings = _value[j].Split(new string[] { ": " }, StringSplitOptions.None);
                ValueText.Text = strings[0].ToString();
                ValueText.Margin = new Thickness(25, 0, 0, 0);

                Value.Children.Add(ValueText);

                TextBox text = new TextBox();
                text.Margin = new Thickness(25 , 0 , 0 , 25);
                text.Tag = j;
                text.TextWrapping= TextWrapping.Wrap;
                text.HorizontalAlignment= HorizontalAlignment.Left;
                if (strings.Length > 1)
                {
                    string t = strings[0].ToString().Replace(" ","");
                    string t2 = strings[1].ToString().Replace("|",",");
                    t = t.Trim();
                    t2 = t2.Trim();
                    text.Text = t2;
                    _vales.Add(new vales(
                        t,
                        t2
                       ));
                }
                else
                {
                    string t = strings[0].ToString().Replace(" ", "");
                    t = t.Trim();
                    text.Text = " ";
                    _vales.Add(new vales(
                       t,
                       " "
                      ));
                }
                text.TextChanged += delegate
                {
                    string t = text.Text;
                    _vales[Convert.ToInt32(text.Tag)].value = t;
         

                };
                Value.Children.Add(text);

                stack.Children.Add(Value);
            }
            global.Children.Add(stack);



            Button accept = new Button();
            accept.Content = "Поддтвердить";
            accept.HorizontalAlignment = HorizontalAlignment.Left;
            accept.VerticalAlignment = VerticalAlignment.Bottom;
            accept.Margin = new Thickness(25, 0, 0, 25);
            accept.Tag = _id;
            accept.Click += async delegate
            {
                string Id = accept.Tag.ToString();
                string result = string.Empty;
                for (int j = 0; j < _value.Length; j++)
                {
                    string _drv = String.Empty;

                    try
                    {
                        result += "\"" + _vales[j].title.ToString().Trim() + "\":\"" + _vales[j].value.ToString().Replace(",","|").Trim() + "\", ";
                    }
                    catch
                    {

                    }
                }
                result = result.Remove(result.Length - 2);
                workingBD.workingDB.Edit(Id,result);

                mainWindow.values.Clear();

                workingDB.loaderTest(mainWindow.tests);

                mainWindow.OpenPages(MainWindow.pages.main);
            };
            stack.Children.Add(accept);



            Button cancel = new Button();
            cancel.Content = "Отменить";
            cancel.HorizontalAlignment = HorizontalAlignment.Left;
            cancel.VerticalAlignment = VerticalAlignment.Bottom;
            cancel.Margin = new Thickness(25, 0, 0, 25);
            cancel.Tag = _id;
            cancel.Click += async delegate
            {
                mainWindow.OpenPages(MainWindow.pages.main);
            };

            stack.Children.Add(cancel);

            parrent.Children.Add(global);
        }
    }
}
