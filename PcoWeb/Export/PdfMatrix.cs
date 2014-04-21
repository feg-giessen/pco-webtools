using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.hyphenation;
using PcoBase;
using PcoWeb.Models;
using Color = System.Drawing.Color;

namespace PcoWeb.Export
{
    public class PdfMatrix
    {
        static PdfMatrix()
        {
            iTextSharp.text.io.StreamUtil.AddToResourceSearch(Assembly.Load("itext-hyph-xml"));
        }

        public static byte[] Generate(Organization org, DateTime start, DateTime end, IEnumerable<MatrixPlan> plans, bool a3)
        {
            if (org == null)
                throw new ArgumentNullException("org");

            var pdfdoc = new Document();
            pdfdoc.SetPageSize(a3 ? PageSize.A3.Rotate() : PageSize.A4.Rotate());
            pdfdoc.SetMargins(15, 15, 10, 15);

            var culture = CultureInfo.GetCultureInfo("de-DE");

            var hyphenation = new HyphenationAuto("de", "DE", 2, 2);
            
            var stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
            pdfdoc.Open();

            float baseFontSize = a3 ? 9 : 7;
            
            Font title = FontFactory.GetFont("Arial", 1.6F * baseFontSize, Font.BOLD);
            Font standard = FontFactory.GetFont("Arial", baseFontSize);
            Font header = FontFactory.GetFont("Arial", baseFontSize, Font.BOLD);

            var titleContent = new Paragraph(new Chunk(
                string.Format(culture, "{2} – Gesamtdienstplan {0:dd.MM.}–{1:dd.MM.yyyy}", start, end, org.name), 
                title))
            {
                SpacingAfter = 10,
            };

            pdfdoc.Add(titleContent);

            var table = new PdfPTable(19)
            {
                WidthPercentage = 100,
                HeaderRows = 1
            };

            Func<string, Font, Phrase> phraseFactory = (string s, Font f) =>
            {
                var p = new Phrase(s, f);
                foreach (Chunk c in p.Chunks)
                {
                    c.SetHyphenation(hyphenation);
                }

                return p;
            };

            int row = 2;
            table.AddCell(phraseFactory("Tag", header));
            ////sheet.Column(1).Width = 8.7;
            table.AddCell(phraseFactory("Datum", header));
            ////sheet.Column(2).Width = 10;
            table.AddCell(phraseFactory("Uhrzeit", header));
            ////sheet.Column(3).Width = 7;
            table.AddCell(phraseFactory("Anlass", header));
            ////sheet.Column(4).Width = 18;
            table.AddCell(phraseFactory("Planung", header));
            ////sheet.Column(5).Width = 13;
            table.AddCell(phraseFactory("Moderation", header));
            ////sheet.Column(6).Width = 13;
            table.AddCell(phraseFactory("Ltg. Abendmahl", header));
            ////sheet.Column(7).Width = 13;
            table.AddCell(phraseFactory("Verkündigung", header));
            ////sheet.Column(8).Width = 13;
            table.AddCell(phraseFactory("Thema/Text", header));
            ////sheet.Column(9).Width = 17;
            table.AddCell(phraseFactory("Ltg. Musik", header));
            ////sheet.Column(10).Width = 13;
            table.AddCell(phraseFactory("Ton", header));
            ////sheet.Column(12).Width = 13;
            table.AddCell(phraseFactory("Präsentation", header));
            ////sheet.Column(13).Width = 13;
            table.AddCell(phraseFactory("Licht", header));
            ////sheet.Column(14).Width = 13;
            table.AddCell(phraseFactory("Bes. Elemente", header));
            table.AddCell(phraseFactory("Bemerkungen", header));
            ////sheet.Column(15).Width = 8.7;
            table.AddCell(phraseFactory("Kollekte", header));
            ////sheet.Column(16).Width = 7;
            table.AddCell(phraseFactory("CD/WWW", header));
            ////sheet.Column(16).Width = 7;
            table.AddCell(phraseFactory("Bistro", header));
            ////sheet.Column(17).Width = 7;
            table.AddCell(phraseFactory("Deko", header));
            ////sheet.Column(18).Width = 7;

            float personWidth = 6.423F;

            table.SetTotalWidth(new[] { 
                2.5F,        // Tag
                4.0F,        // datum
                3.458F,      // uhrzeit
                8.893F,      // anlass
                personWidth, // planung
                personWidth, // moderation
                personWidth, // abendmahl
                personWidth, // verkündigung
                8.399F,      // thema/text
                personWidth, // Musik VA
                personWidth, // Ton
                personWidth, // Präs
                personWidth, // Licht
                6.298F,      // bes. element
                6.298F,      // bemerkungen
                4.5F,        // kollekte
                4.5F,        // cd/www
                3.458F,      // bistro
                3.458F       // deko
            });

            foreach (var cell in table.GetRow(0).GetCells())
            {
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            }

            table.CompleteRow();

            row = 1;

            foreach (var plan in plans)
            {
                Font cellFont;
                
                switch (plan.Item.service_type_id)
                {
                    //case 200602:
                    //    cellFont = FontFactory.GetFont("Arial", baseFontSize, BaseColor.RED);
                    //    break;
                    //case 312434:
                    default:
                        cellFont = FontFactory.GetFont("Arial", baseFontSize, BaseColor.BLACK);
                        break;
                }

                table.AddCell(phraseFactory(string.Format(culture, "{0:ddd}", plan.Date), cellFont));
                table.AddCell(phraseFactory(string.Format(culture, "{0:dd.MM.}", plan.Date), cellFont));
                table.AddCell(phraseFactory(string.Format(culture, "{0:HH:mm}", plan.Date), cellFont));
                table.AddCell(phraseFactory(plan.Anlass, cellFont));
                table.AddCell(phraseFactory(People(plan.Gottesdienstplanung), cellFont));
                table.AddCell(phraseFactory(People(plan.Hauptmoderation), cellFont));
                table.AddCell(phraseFactory(People(plan.Abendmahl), cellFont));
                table.AddCell(phraseFactory(plan.Verkuendigung, cellFont));
                table.AddCell(phraseFactory(plan.Thema, cellFont));
                table.AddCell(phraseFactory(People(plan.Musik), cellFont));
                table.AddCell(phraseFactory(People(plan.Ton), cellFont));
                table.AddCell(phraseFactory(People(plan.Praesentation), cellFont));
                table.AddCell(phraseFactory(People(plan.Licht), cellFont));
                table.AddCell(phraseFactory(plan.BesElemente, cellFont));
                table.AddCell(phraseFactory(plan.Bemerkung, cellFont));
                table.AddCell(phraseFactory(plan.Kollekte, cellFont));
                table.AddCell(phraseFactory(plan.Aufnahme, cellFont));
                table.AddCell(phraseFactory(plan.Bistro, cellFont));
                table.AddCell(phraseFactory(plan.Deko, cellFont));
                                
                switch (plan.Item.service_type_id)
                {
                    case 200602:
                        foreach (var cell in table.GetRow(row).GetCells())
                        {
                            cell.BackgroundColor = new BaseColor(ViewHelpers.ColorAbend);
                        }
                        break;
                    case 308904:
                        foreach (var cell in table.GetRow(row).GetCells())
                        {
                            cell.BackgroundColor = new BaseColor(ViewHelpers.ColorMorgen);
                        }
                        break;
                    case 312434:
                        foreach (var cell in table.GetRow(row).GetCells())
                        {
                            cell.BackgroundColor = new BaseColor(ViewHelpers.ColorBesondere);
                        }
                        break;
                }

                row++;
                table.CompleteRow();
            }

            pdfdoc.Add(table);

            pdfdoc.Close();
            writer.Flush();

            byte[] result = stream.ToArray();
            stream.Dispose();

            return result;
        }

        private static string People(string names)
        {
            const char NBSP = '\u00a0';

            return string.Join(", ", names.Split(new[] { ", " }, StringSplitOptions.None).Select(n => n.Replace(' ', NBSP)));
        }
    }
}