using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace DemoDataTable
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //string filepath = @"D:\00_data\01_WorkSpace\VisualStudio\WPF\TestDataTable\TestDataTable\Dipole_dB(GainTotal).csv";
            string filepath = @"..\..\..\Dipole_dB(GainTotal).csv";
            DataRow[] dr1;
            DataRow dr2;
            DataRow[] dr3;
            DataRow dr4;
            DataTable dt = new DataTable();
            DateTime dt0;
            DateTime dt1;
            DateTime dt2;
            DateTime dt3;
            DateTime dt4;
            DateTime dt5;
            object[] objs;
            object[] objs2;
            DataColumn[] cols1;
            DataColumn[] cols2;

            // 0.记录开始时刻
            dt0 = DateTime.Now;
            // 1.读取csv
            dt = Csv2DataTable(filepath);
            dt1 = DateTime.Now;
            // 2.dt.Select()查询单个字段
            dr1 = dt.Select("Id=1");
            dt2 = DateTime.Now;
            // 3.dt.Rows.Find()查询单个字段
            cols1 = new DataColumn[] { dt.Columns["Id"] };
            dt.PrimaryKey = cols1;
            objs = new object[] { 1 };
            dr2 = dt.Rows.Find(objs);
            dt3 = DateTime.Now;
            // 4.dt.Select()查询多个字段
            dr3 = dt.Select("Phi=1 and Theta=1");
            dt4 = DateTime.Now;
            // 5.dt.Rows.Find()查询多个字段
            // 5.1.联合两个字段为一个主键
            cols2 = new DataColumn[] { dt.Columns["Phi"], dt.Columns["Theta"] };
            dt.PrimaryKey = cols2;
            // 5.2.查询
            objs2 = new object[] { 1, 1 };
            dr4 = dt.Rows.Find(objs2);
            dt5 = DateTime.Now;

            double data1 = double.Parse(dr1[0]["dB(GainTotal)"].ToString());
            double data2 = double.Parse(dr2["dB(GainTotal)"].ToString());
            double data3 = double.Parse(dr3[0]["dB(GainTotal)"].ToString());
            double data4 = double.Parse(dr4["dB(GainTotal)"].ToString());
            Console.WriteLine("data:\t{0:#.00}", data1);
            Console.WriteLine("data2:\t{0:#.00}", data2);
            Console.WriteLine("data:\t{0:#.00}", data3);
            Console.WriteLine("data2:\t{0:#.00}", data4);

            Console.WriteLine("csv文件读取到dataTable耗时");
            Console.WriteLine("Csv2DataTable():\t{0:#.0}ms", dt1.Subtract(dt0).TotalMilliseconds);
            Console.WriteLine("查询单个字段耗时");
            Console.WriteLine("dt.Select():\t\t{0:#.0}ms", dt2.Subtract(dt1).TotalMilliseconds);
            Console.WriteLine("dt.Rows.Find():\t\t{0:#.0}ms", dt3.Subtract(dt2).TotalMilliseconds);
            Console.WriteLine("查询多个字段耗时");
            Console.WriteLine("dt.Select():\t\t{0:#.0}ms", dt4.Subtract(dt3).TotalMilliseconds);
            Console.WriteLine("dt.Rows.Find():\t\t{0:#.0}ms", dt5.Subtract(dt4).TotalMilliseconds);

            //data: -64.59
            //data2: -64.59
            //data: -34.89
            //data2: -34.89
            //csv文件读取到dataTable耗时
            //Csv2DataTable():	252.5ms
            //查询单个字段耗时
            //dt.Select():		527.2ms
            //dt.Rows.Find():	4.5ms
            //查询多个字段耗时
            //dt.Select():		530.2ms
            //dt.Rows.Find():	9.4ms

            // 删除某一列
            dt.Columns.Remove("Phi");
            dt.Columns.RemoveAt(1);
        }
        //导入CSV文件
        public static DataTable Csv2DataTable(string fileName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;

            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列,并命名字段名称
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
