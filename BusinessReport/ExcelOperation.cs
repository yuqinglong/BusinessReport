using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessReport
{
    public class ExcelOperation
    {
        public static void SaveToExcel(System.Data.DataTable dt, string filePath,string sheetName)
        {

            
            DateTime startTime = DateTime.Now;
            Excel.Application xlsApp = new Excel.ApplicationClass();
            DateTime endTime = DateTime.Now;
            try
            {
                if (xlsApp == null)
                {
                    throw new Exception("xlsApp == null");
                }
                Excel.Workbook xlsBook = xlsApp.Workbooks.Add(true);
                Excel.Worksheet xlsSheet = (Excel.Worksheet)xlsBook.Worksheets[1];

                xlsSheet.Name = sheetName;
                xlsApp.Cells.NumberFormatLocal = "@";   //设置为文本格式

                int rowIndex = 1;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    xlsSheet.Cells[rowIndex, i + 1] = dt.Columns[i].ColumnName;
                    Excel.Range range = xlsSheet.get_Range(xlsSheet.Cells[rowIndex, i + 1], xlsSheet.Cells[rowIndex, i + 1]);
                    range.EntireColumn.AutoFit();//列宽自适应
                }

                rowIndex++;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        xlsSheet.Cells[rowIndex, j + 1] = Convert.ToString(dt.Rows[i][j].ToString());
                    }

                    rowIndex++;
                }
//save the excel

                xlsBook.Saved = true;
                xlsBook.SaveCopyAs(filePath);
                xlsApp.Quit();
                GC.Collect();
            }
            finally
            {
                Process proc = new Process();
                foreach (Process p in Process.GetProcessesByName("excel"))
                {
                    if (p.StartTime >= startTime && p.StartTime <= endTime)
                    {
                        p.Kill();
                    }
                }
            }
        }
    }
}
