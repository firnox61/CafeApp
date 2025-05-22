using ClosedXML.Excel;
using Entities.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilites
{
    public static class ExcelExportHelper
    {
        public static byte[] ExportIngredientUsageToExcel(List<IngredientUsageReportDto> data)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("En Çok Kullanılan Malzemeler");

            // Başlıklar
            worksheet.Cell(1, 1).Value = "Malzeme Adı";
            worksheet.Cell(1, 2).Value = "Toplam Kullanım Miktarı";

            // Veri
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = data[i].IngredientName;
                worksheet.Cell(i + 2, 2).Value = data[i].TotalUsedAmount;
            }

            // Otomatik sütun genişliği
            worksheet.Columns().AdjustToContents();

            // Excel dosyasını byte dizisine çevir
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
