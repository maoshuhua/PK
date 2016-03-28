using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace NPOIWeb
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string file = "E:\\template.xlsx";
            ExcelRead(file);
        }

        private void ExcelRead(string file)
        {
            try { 
               using(ExcelHelper excelHelper = new ExcelHelper(file))
               {
                   DataTable dt = excelHelper.ExcelToDataTable("Sheet1",true);
                   PrintData(dt);
               }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void PrintData(DataTable data)
        {
            string str = string.Empty;
            if (data == null) return;
            for (int i = 0; i < data.Rows.Count; ++i)
            {
                for (int j = 0; j < data.Columns.Count; ++j)
                    str += data.Rows[i][j];
                str += "\n";
            }

            Label1.Text = str;
        }
    }
}