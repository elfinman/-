using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace 超高频会议签到.Business
{
    class ExcelEditHelper
    {

        OleDbConnection conn = new OleDbConnection();
        public bool isOpen = false;
        public bool CreatAndOpen(string fileName)
        {
            string excelPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + fileName + ".xlsx";              //提取文件
            if (!System.IO.File.Exists(excelPath))
            {
                //不存在先创建
            /*   excel._Application excel_app = new excel.Application();           //创建Excel 程序
                excel_app.Visible = true;
                try
                {
                    excel.Workbook book1 = excel_app.Workbooks.Add(Type.Missing);//使用 app1生成一个book对象  
                    excel.Worksheet sheet1 = (excel.Worksheet)book1.Sheets[1];//从book1对象中生成Sheet对象并赋值  
                    excel.Range rng1 = sheet1.get_Range("A1", Type.Missing); //设置操作区域  
                    rng1.Value2 = "Hello World!"; //对操作区域赋值  
                }
                catch
                {
                    excel_app.Quit();//结束进程  
                }*/
            }
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;Persist Security Info=False;" + "data source=" + @excelPath + ";Extended Properties='Excel 12.0; HDR=yes; IMEX=10'";          
            conn.ConnectionString = strConn;
            isOpen = true;
            return true;
        }

        public void Insert(string data)
        {
            OleDbCommand cmd = new OleDbCommand(data, conn);//(A,B,C,D,E,F) 
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void Close()
        {
            isOpen = false;
        }
    }
}
