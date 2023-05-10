using ExcelDataReader;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using workingBD;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для import.xaml
    /// </summary>
    public partial class import : Page
    {
        MainWindow mainWindow;
        public import(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var path = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "xls files (*.xls)|*.xls";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;

            }

            openFileDialog.Reset();




            if (path != string.Empty)
            {
                DataTableCollection tableCollection = null;

                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                DataSet db = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });


                tableCollection = db.Tables;

                int count = tableCollection.Count;

                DataTable table = tableCollection[0];


                info.DataContext = table;
                info.ItemsSource = table.AsDataView();

                reader.Close();
                path = string.Empty;
            }
           
        }

        private async void button_Click1(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            BsonDocument tom = null;
            var result = string.Empty;
            int start = 0;
            DataRowView drv = null;
            DataRowView Zagolovok = null;
            for (int i = 0; i < info.Items.Count - 1; i++)
            {
                DataRowView timles = (DataRowView)info.Items[i];

                string value = timles[0].ToString();

                if (value == "№№ пп")
                {
                    start = i;
                    Zagolovok = (DataRowView)info.Items[i];
                    break;
                }
                else
                {
                    flag = false;
                    MessageBox.Show("Файл должен содержать поле \"№№ пп\" ");
                    break;
                    
                }
            }

            if(flag == true)
            {
                for (int i = start + 1; i < info.Items.Count - 1; i++)
                {
                    result = "";
                    drv = (DataRowView)info.Items[i];

                    for (int j = 1; j < info.Columns.Count; j++)
                    {
                        string _drv = String.Empty;

                        _drv += (drv[j]).ToString();

                        try
                        {
                            _drv = _drv.Replace("\"", "\'");
                            _drv = _drv.Replace(",", "|");
                            result += " \"" + Zagolovok[j].ToString() + "\": \"" + _drv + "\" , ";
                        }
                        catch
                        {

                        }
                    }

                    result = result.Remove(result.Length - 2);

                    workingBD.workingDB.Improt(result);

                }
            }
        }

        private void button_Click2(object sender, RoutedEventArgs e)
        {
            mainWindow.values.Clear();
            workingBD.workingDB.loader(mainWindow.values);
            mainWindow.OpenPages(MainWindow.pages.main);
        }
    }
}
