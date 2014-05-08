using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PcoWeb.Models;

namespace PcoWeb.Export
{
    public class ExcelTopics
    {
        public static byte[] Generate(IEnumerable<MatrixPlan> plans)
        {
            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("Veranstaltungen");
            
            int row = 1;
            sheet.Cells[row, 1].Value = "Tag";
            sheet.Column(1).Width = 5;
            sheet.Cells[row, 2].Value = "Datum";
            sheet.Column(2).Width = 8;
            sheet.Cells[row, 3].Value = "Uhrzeit";
            sheet.Column(3).Width = 8;
            sheet.Cells[row, 4].Value = "Veranstaltung";
            sheet.Column(4).Width = 40;

            row++;

            var datePlane = plans.GroupBy(p => p.Date.Date);

            foreach (var dateGroup in datePlane)
            {
                sheet.Cells[row, 1].Value = string.Format("{0:ddd}", dateGroup.Key);
                sheet.Cells[row, 2].Value = string.Format("{0:dd.MM.}", dateGroup.Key);

                foreach (var plan in dateGroup)
                {
                    sheet.Cells[row, 3].Value = string.Format("{0:HH:mm}", plan.Date);
                    
                    switch (plan.Item.service_type_id)
                    {
                        case 200602:
                            // Abend
                            sheet.Cells[row, 4].Value = plan.Verkuendigung;
                            break;
                        case 308904:
                            // Morgen
                        case 312434:
                        default:
                            // Sonstige
                            sheet.Cells[row, 4].Value = string.Format("{0} / {1}", plan.Gottesdienstplanung, plan.Verkuendigung);
                            break;
                    }

                    if (plan.Anlass != "Gottesdienst" && !string.IsNullOrWhiteSpace(plan.Anlass))
                    {
                        string anlass = plan.Anlass.Replace("Gottesdienst - ", string.Empty);

                        if (anlass == "Gemeindestunde")
                        {
                            sheet.Cells[row, 4].Value = anlass + " " + sheet.Cells[row, 4].Value;
                        }
                        else
                        {
                            sheet.Cells[row, 4].Value += " (" + anlass + ")";
                        }
                    }

                    row++;
                }

                // insert empty row
                row++;
            }

            var cellRange = sheet.Cells[1, 1, row - 1, 4];

            cellRange.Style.SetBorder(ExcelBorderStyle.Thin, Color.Black);
            cellRange.Style.Font.Name = "Arial";
            cellRange.Style.Font.Size = 10;
            cellRange.Style.WrapText = true;

            sheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            sheet.Cells[1, 1, 1, 4].Style.Font.Bold = true;

            var stream = new MemoryStream();
            package.SaveAs(stream);

            return stream.ToArray();
        }
    }
}