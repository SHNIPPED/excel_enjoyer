using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Data;
using ExcelDataReader;
using System.IO;
using workingBD;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<workingBD.Values> values = new List<workingBD.Values>();
        public List<workingBD.test> tests = new List<workingBD.test>();

        public MainWindow()
        {
            InitializeComponent();
            workingBD.workingDB.loader(values);
            workingBD.workingDB.loaderTest(tests);
            OpenPages(pages.login);
        }
        public enum pages
        {
            main,
            import,
            add,
            login
        }
        public void OpenPages(pages _pages)
        {
            if (_pages == pages.main)
                frame.Navigate(new Pages.main(this));
            else if(_pages == pages.import)
                frame.Navigate(new Pages.import(this));
            else if (_pages == pages.import)
                frame.Navigate(new Pages.editing.add(this));
            else if (_pages == pages.login)
                frame.Navigate(new Pages.login(this));
        }

    }
}