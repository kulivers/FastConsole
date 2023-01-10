using System;
using System.IO;
using System.Linq;
using Aspose.Cells;

namespace excel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, "1.xlsx");
            var path2 = Path.Combine(dir, "2.xlsx");
            if (File.Exists(path2)) File.Delete(path2);
            File.Copy(path, path2);
            var workbook = new Workbook(path2);
            var ws = workbook.Worksheets.First();
            ws.Cells.DeleteRange(18, 0, 18, 100, ShiftType.Up);
            
            workbook.Save(path2, new PdfSaveOptions(){});
            
            if (File.Exists(path2)) File.Delete(path2);
        }
    }
}