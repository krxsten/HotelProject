using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject
{
    public class LogForExcel : ILog
    {
        private List<string> text;
        public LogForExcel()
        {
            text = new List<string>();
        }
        public void Log(string excelText)
        {
            text.Add(excelText);
        }
        public static void ReadExcelFile(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheet("Sheet1");
                for (int i = 0; i < sheet.LastRowNum; i++)
                {
                    IRow currentRow = sheet.GetRow(i);
                    if (currentRow != null)
                    {
                        for (int j = 0; j < currentRow.LastCellNum; j++)
                        {
                            Console.WriteLine(currentRow.GetCell(j) + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
        public void PrintLogger(string filePath)
        {
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheets.Worksheet("Worksheet");

                foreach (var row in worksheet.RowsUsed())
                {
                    Console.WriteLine(row.Cell(1).Value);
                }
            }
        }

        public void SaveToWorkbook(string savedMessage)
        {
            this.text.Add(savedMessage);
        }

        public void WriteToWorkbook(string filehpat)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("WorkSheet");

                for (int i = 0; i < text.Count; i++)
                {
                    worksheet.Cell(i + 1, 1).Value = text[i];
                    worksheet.Cell(i + 1, 1).Style.Font.SetBold(true);
                }

                workbook.SaveAs(filehpat);
            }
        }
    }
}
