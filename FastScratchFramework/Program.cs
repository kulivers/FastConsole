using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Aspose.Cells;

namespace FastScratchMVC
{
    internal class Program
    {
        public static MemoryStream GetExcelStream(string file)
        {
            var workbook = new Workbook(File.OpenRead(file));
            var ms = new MemoryStream();
            var txtSaveOptions = new TxtSaveOptions(SaveFormat.Pdf) { Encoding = Encoding.Unicode };
            workbook.Save(ms, txtSaveOptions);
            ms.Position = 0L;
            return ms;
        }

        public static void Main(string[] args)
        {
            var fileVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            var version = new Version(fileVersion);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var a = Encoding.GetEncoding("windows-1251");
            var bytes = a.GetBytes("Вес");
            var dir = @"C:\Users\ekul\Desktop\";
            foreach (var encoding in Encoding.GetEncodings())
            {
                using (var fs = File.Create(dir + $"{encoding.Name}.pdf"))
                using (var book = new Workbook(fs))
                {
                    var sheet = book.Worksheets.First();
                    sheet.Cells["A1"].Value = encoding.GetEncoding().GetString(bytes);//try only  windows
                    var txtSaveOptions = new TxtSaveOptions(SaveFormat.Pdf) { Encoding = encoding.GetEncoding(), };
                    {
                        book.Save(fs, txtSaveOptions);
                    }
                }
            }

            // var byteFile = @"C:\Users\ekul\Desktop\str3";
            // var pdfFile = @"C:\Users\ekul\Desktop\test.pdf";
            // using (var excelStream = GetExcelStream(byteFile))
            // using (var pdfFileStream = File.Create(pdfFile))
            // {
            //     excelStream.WriteTo(pdfFileStream);
            // }
        }
    }
}