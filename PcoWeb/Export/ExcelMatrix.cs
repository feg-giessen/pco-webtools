using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PcoBase;
using PcoWeb.Models;

namespace PcoWeb.Export
{
    public class ExcelMatrix
    {
        public static byte[] Generate(Organization org, DateTime start, DateTime end, IEnumerable<MatrixPlan> plans)
        {
            if (org == null)
                throw new ArgumentNullException("org");

            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("Dienstplan");

            var title = sheet.Cells[1, 1, 1, 19];
            title.Merge = true;
            title.Value = string.Format("{2} – Gesamtdienstplan {0:dd.MM.}–{1:dd.MM.yyyy}", start, end, org.Name);
            title.Style.Font.Bold = true;
            title.Style.Font.Size = 16;
            title.Style.Font.Name = "Arial";

            int row = 2;
            sheet.Cells[row, 1].Value = "Tag";
            sheet.Column(1).Width = 4;
            sheet.Cells[row, 2].Value = "Datum";
            sheet.Column(2).Width = 10;
            sheet.Cells[row, 3].Value = "Uhrzeit";
            sheet.Column(3).Width = 7;
            sheet.Cells[row, 4].Value = "Anlass";
            sheet.Column(4).Width = 18;
            sheet.Cells[row, 5].Value = "Planung";
            sheet.Column(5).Width = 13;
            sheet.Cells[row, 6].Value = "Moderation";
            sheet.Column(6).Width = 13;
            sheet.Cells[row, 7].Value = "Ltg. Abendmahl";
            sheet.Column(7).Width = 13;
            sheet.Cells[row, 8].Value = "Verkündigung";
            sheet.Column(8).Width = 13;
            sheet.Cells[row, 9].Value = "Thema/Text";
            sheet.Column(9).Width = 17;
            sheet.Cells[row, 10].Value = "Ltg. Musik";
            sheet.Column(10).Width = 13;
            sheet.Cells[row, 11].Value = "Ton";
            sheet.Column(11).Width = 13;
            sheet.Cells[row, 12].Value = "Präsentation";
            sheet.Column(12).Width = 13;
            sheet.Cells[row, 13].Value = "Licht";
            sheet.Column(13).Width = 13;
            sheet.Cells[row, 14].Value = "Bes. Elemente";
            sheet.Column(14).Width = 8.7;
            sheet.Cells[row, 15].Value = "Bemerkungen";
            sheet.Column(15).Width = 8.7;
            sheet.Cells[row, 16].Value = "Kollekte";
            sheet.Column(16).Width = 7.5;
            sheet.Cells[row, 17].Value = "CD/WWW";
            sheet.Column(17).Width = 7.5;
            sheet.Cells[row, 18].Value = "Bistro";
            sheet.Column(18).Width = 7;
            sheet.Cells[row, 19].Value = "Deko";
            sheet.Column(19).Width = 13;

            row++;

            foreach (var plan in plans)
            {
                sheet.Cells[row, 1].Formula = "B" + row;
                sheet.Cells[row, 1].Style.Numberformat.Format = "ddd";
                sheet.Cells[row, 1].Style.Locked = true;
                sheet.Cells[row, 2].Value = plan.Date;
                sheet.Cells[row, 2].Style.Numberformat.Format = "dd.mm.yyyy";
                sheet.Cells[row, 3].Formula = "B" + row;
                sheet.Cells[row, 3].Style.Numberformat.Format = "hh:MM";
                sheet.Cells[row, 3].Style.Locked = true;
                sheet.Cells[row, 4].Value = plan.Anlass;
                sheet.Cells[row, 5].Value = plan.Gottesdienstplanung;
                sheet.Cells[row, 6].Value = plan.Hauptmoderation;
                sheet.Cells[row, 7].Value = plan.Abendmahl;
                sheet.Cells[row, 8].Value = plan.Verkuendigung;
                sheet.Cells[row, 9].Value = plan.Thema;
                sheet.Cells[row, 10].Value = plan.Musik;
                sheet.Cells[row, 11].Value = plan.Ton;
                sheet.Cells[row, 12].Value = plan.Praesentation;
                sheet.Cells[row, 13].Value = plan.Licht;
                sheet.Cells[row, 14].Value = plan.BesElemente;
                sheet.Cells[row, 15].Value = plan.Bemerkung;
                sheet.Cells[row, 16].Value = plan.Kollekte;
                sheet.Cells[row, 17].Value = plan.Aufnahme;
                sheet.Cells[row, 18].Value = plan.Bistro;
                sheet.Cells[row, 19].Value = plan.Deko;

                switch (plan.Item.ServiceTypeId)
                {
                    case 200602:
                        sheet.Cells[row, 1, row, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, 1, row, 19].Style.Fill.BackgroundColor.SetColor(ViewHelpers.ColorAbend);
                        break;
                    case 308904:
                        sheet.Cells[row, 1, row, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, 1, row, 19].Style.Fill.BackgroundColor.SetColor(ViewHelpers.ColorMorgen);
                        break;
                    case 312434:
                        sheet.Cells[row, 1, row, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[row, 1, row, 19].Style.Fill.BackgroundColor.SetColor(ViewHelpers.ColorBesondere);
                        break;
                }

                row++;
            }

            var cellRange = sheet.Cells[2, 1, row - 1, 19];

            cellRange.Style.SetBorder(ExcelBorderStyle.Thin, Color.Black);
            cellRange.Style.Font.Name = "Arial";
            cellRange.Style.Font.Size = 10;
            cellRange.Style.WrapText = true;

            var table = sheet.Tables.Add(sheet.Cells[2, 1, row - 1, 19], "DienstTable");
            table.StyleName = "None";
            
            sheet.Cells[2, 1, 2, 19].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[2, 1, 2, 19].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            sheet.Cells[2, 1, 2, 19].Style.Font.Bold = true;

            var stream = new MemoryStream();
            package.SaveAs(stream);

            return stream.ToArray();
        }
    }

    internal static class Extensions
    {
        public static void SetBorder(this ExcelStyle style, ExcelBorderStyle border, Color color)
        {
            style.Border.Left.Style = border;
            style.Border.Left.Color.SetColor(color);
            style.Border.Top.Style = border;
            style.Border.Top.Color.SetColor(color);
            style.Border.Right.Style = border;
            style.Border.Right.Color.SetColor(color);
            style.Border.Bottom.Style = border;
            style.Border.Bottom.Color.SetColor(color);
        }
    }
}