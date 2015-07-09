using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PcoBase;
using PcoWeb.Models;

namespace PcoWeb.Export
{
    public class PdfMatrix
    {
        static PdfMatrix()
        {
            // Load DLL with german hyphenation configuration.
            iTextSharp.text.io.StreamUtil.AddToResourceSearch(Assembly.Load("itext-hyph-xml"));
        }

        public static byte[] Generate(Organization org, DateTime start, DateTime end, IEnumerable<MatrixPlan> plans, bool a3)
        {
            if (org == null)
                throw new ArgumentNullException("org");

            var pdfdoc = new Document();
            pdfdoc.SetPageSize(a3 ? PageSize.A3.Rotate() : PageSize.A4.Rotate());   // Landscape format.
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
                string.Format(culture, "{2} – Gesamtdienstplan {0:dd.MM.}–{1:dd.MM.yyyy}", start, end, org.Name), 
                title))
            {
                SpacingAfter = 10,
            };

            pdfdoc.Add(titleContent);

            var table = new PdfPTable(20)
            {
                WidthPercentage = 100,
                HeaderRows = 1
            };

            // Factory for applying hyphenation.
            Func<string, Font, Phrase> phraseFactory = (string s, Font f) =>
            {
                var p = new Phrase(s, f);
                foreach (Chunk c in p.Chunks)
                {
                    c.SetHyphenation(hyphenation);
                }

                return p;
            };

            table.AddCell(phraseFactory("Tag", header));
            table.AddCell(phraseFactory("Datum", header));
            table.AddCell(phraseFactory("Uhrzeit", header));
            table.AddCell(phraseFactory("Anlass", header));
            table.AddCell(phraseFactory("Planung", header));
            table.AddCell(phraseFactory("Leitung", header));
            table.AddCell(phraseFactory("Ltg. Abendmahl", header));
            table.AddCell(phraseFactory("Verkündigung", header));
            table.AddCell(phraseFactory("Thema/Text", header));
            table.AddCell(phraseFactory("Ltg. Musik", header));
            table.AddCell(phraseFactory("Ton", header));
            table.AddCell(phraseFactory("Präsentation", header));
            table.AddCell(phraseFactory("Licht", header));
            table.AddCell(phraseFactory("Bes. Elemente", header));
            table.AddCell(phraseFactory("Bemerkungen", header));
            table.AddCell(phraseFactory("Kollekte", header));
            table.AddCell(phraseFactory("CD/WWW", header));
            table.AddCell(phraseFactory("Bistro", header));
            table.AddCell(phraseFactory("Deko", header));
            table.AddCell(phraseFactory("Foyer", header));

            const float PersonWidth = 6.423F;

            table.SetTotalWidth(new[] { 
                2.5F,        // Tag
                4.0F,        // datum
                3.458F,      // uhrzeit
                8.893F,      // anlass
                PersonWidth, // planung
                PersonWidth, // moderation
                PersonWidth, // abendmahl
                PersonWidth, // verkündigung
                8.399F,      // thema/text
                PersonWidth, // Musik VA
                PersonWidth, // Ton
                PersonWidth, // Präs
                PersonWidth, // Licht
                6.298F,      // bes. element
                6.298F,      // bemerkungen
                4.5F,        // kollekte
                4.5F,        // cd/www
                3.458F,      // bistro
                PersonWidth,  // deko
                3.458F,      // foyer
            });

            foreach (var cell in table.GetRow(0).GetCells())
            {
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            }

            table.CompleteRow();

            int row = 1;

            Font cellFont = FontFactory.GetFont("Arial", baseFontSize, BaseColor.BLACK);

            foreach (var plan in plans)
            {
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
                table.AddCell(phraseFactory(plan.Foyerdienst, cellFont));
                                
                switch (plan.Item.ServiceTypeId)
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