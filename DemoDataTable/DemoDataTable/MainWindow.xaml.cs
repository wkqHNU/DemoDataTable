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
            DataRow[] dr;
            DataTable dt = new DataTable();


            DateTime dt1 = DateTime.Now;
            // 1.读取csv
            dt = Csv2DataTable(filepath);
            DateTime dt2 = DateTime.Now;
            // 2.用dt.Select();查询
            dr = dt.Select("Phi=1 and Theta=1");
            DateTime dt3 = DateTime.Now;
            // 3.用dt.Rows.Find();查询
            // 3.1.联合两个字段为一个主键
            DataColumn[] cols = new DataColumn[] { dt.Columns["Phi"], dt.Columns["Theta"] };
            dt.PrimaryKey = cols;
            // 3.2.查询
            object[] objs = new object[] { 1, 1 };
            DataRow dr2 = dt.Rows.Find(objs);
            DateTime dt4 = DateTime.Now;

            double data = double.Parse(dr[0]["dB(GainTotal)"].ToString());
            double data2 = double.Parse(dr2["dB(GainTotal)"].ToString());
            Console.WriteLine("data:\t{0:#.00}", data);
            Console.WriteLine("data2:\t{0:#.00}", data2);

            double secInterval21 = dt2.Subtract(dt1).TotalMilliseconds;
            double secInterval32 = dt3.Subtract(dt2).TotalMilliseconds;
            double secInterval43 = dt4.Subtract(dt3).TotalMilliseconds;
            Console.WriteLine("Csv2DataTable():\t{0:#.0}ms", secInterval21);
            Console.WriteLine("dt.Select():\t\t{0:#.0}ms", secInterval32);
            Console.WriteLine("dt.Rows.Find():\t\t{0:#.0}ms", secInterval43);
            //Csv2DataTable():  213.4315 ms
            //dt.Select():      614.3553 ms
            //dt.Rows.Find():   7.9794 ms
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
