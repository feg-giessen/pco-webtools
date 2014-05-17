using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PcoBase;

namespace PcoWeb.Models
{
    public class LivePlanModel
    {
        public LivePlanModel(Plan plan, Func<int, Arrangement> arrangements)
        {
            this.Item = plan;
            this.Items = plan.items.Select(i => new LivePlanItemModel(i, this.Time)).ToList();

            this.People = plan.plan_people;

            ServiceTime time = this.Time;
            if (time != null)
            {
                this.StartTime = DateTime.Parse(time.starts_at).ToUniversalTime(); //.Replace(" +0000", string.Empty));
                this.EndTime = DateTime.Parse(time.ends_at).ToUniversalTime();

                int sum = 0;
                foreach (var item in this.Items.Where(i => i.Item.is_preservice).OrderByDescending(s => s.Item.sequence))
                {
                    if (item.Time != null && !item.Time.exclude && item.Time.time_id == time.id)
                    {
                        item.TimePoint = this.StartTime.Value.AddSeconds(-1 * (sum + item.Item.length));
                        sum += item.Item.length;
                    }
                }

                bool HasPlanItemTimes = this.Items.Any(i => !i.Item.is_preservice && !i.Item.is_postservice && i.Time != null);

                sum = 0;
                foreach (var item in this.Items.Where(i => !i.Item.is_preservice && !i.Item.is_postservice).OrderBy(s => s.Item.sequence))
                {
                    if (!HasPlanItemTimes 
                        || (item.Time != null && !item.Time.exclude && item.Time.time_id == time.id))
                    {
                        item.TimePoint = this.StartTime.Value.AddSeconds(sum);
                        sum += item.Item.length;

                        this.EndTime = item.TimePoint;
                    }
                }
            }

            foreach (var item in this.Items.Where(i => i.ItemType == PlanItemType.Song && i.Item.arrangement_id.HasValue))
            {
                var arr = arrangements(item.Item.arrangement_id.Value);

                if (arr != null)
                {
                    item.Arrangement = new ArrangementModel(arr, item.SongSequence.Select(s => s.Title));
                }
            }
        }

        public Plan Item { get; private set; }

        public ServiceTime Time
        {
            get { return this.Item.Try(i => i.service_times.FirstOrDefault(t => t.time_type == "Service")); }
        }

        public DateTime? StartTime
        {
            get;
            private set;
        }

        public DateTime? StartTimeLocal
        {
            get
            {
                if (!this.StartTime.HasValue)
                    return null;

                return ViewHelpers.ConvertToTimeZone(this.StartTime.Value);
            }
        }

        public DateTime? EndTime
        {
            get;
            private set;
        }

        public IEnumerable<LivePlanItemModel> Items
        {
            get;
            private set;
        }

        public IEnumerable<PlanPeople> People { get; private set; }

        public IEnumerable<SingerModel> Singers
        {
            get
            {
                return this.People
                    .Where(p => p.position != null && p.status != "D" && p.position.IndexOf("Gesang", StringComparison.OrdinalIgnoreCase) > -1)
                    .OrderBy(p => p.person_name)
                    .Select(p => new SingerModel(p));
            }
        }
    }

    public class SingerModel
    {
        private readonly PlanPeople person;

        public SingerModel(PlanPeople person)
        {
            this.person = person;
        }

        public int Id
        {
            get { return this.person.person_id; }
        }

        public string Name
        {
            get { return this.person.person_name; }
        }

        public string Shortcut
        {
            get 
            { 
                string[] nameParts = (this.person.person_name ?? string.Empty).Split(' ');

                if (nameParts.Length > 1)
                {
                    return nameParts.First().Substring(0, 1).ToUpper()
                        + nameParts.Last().Substring(0, 1).ToUpper();
                }

                if (nameParts[0].Length > 1)
                    return nameParts[0].Substring(0, 2);

                return nameParts[0];
            }
        }
    }

    public enum PlanItemType
    {
        Header,
        Song,
        Normal
    }

    public class LivePlanItemModel
    {
        public LivePlanItemModel(Item item, ServiceTime time)
        {
            this.Item = item;
            this.Time = item.plan_item_times.FirstOrDefault(t => t.time_id == time.id);
        }

        public Item Item { get; private set; }

        public ItemTime Time { get; private set; }

        public DateTime? TimePoint { get; set; }

        public DateTime? TimePointLocal
        {
            get
            {
                if (!this.TimePoint.HasValue)
                    return null;

                return ViewHelpers.ConvertToTimeZone(this.TimePoint.Value);
            }
        }

        public PlanItemType ItemType
        {
            get
            {
                if (this.Item.type == "PlanHeader")
                    return PlanItemType.Header;

                if (this.Item.song != null)
                    return PlanItemType.Song;

                return PlanItemType.Normal;
            }
        }
        
        public string Akteur
        {
            get { return this.GetPlanNote("Akteur") ?? string.Empty; }
        }

        public IHtmlString Description
        {
            get { return Helper.Nl2br(this.Item.description); }
        }

        public IHtmlString Detail
        {
            get { return MvcHtmlString.Create(this.Item.detail); }
        }

        public IEnumerable<Sequence> SongSequence
        {
            get
            {
                if (string.IsNullOrEmpty(this.Item.arrangement_sequence_to_s))
                    return Enumerable.Empty<Sequence>();

                return this.Item.arrangement_sequence_to_s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => new Sequence { Title = s.Trim() });
            }
        }

        public ArrangementModel Arrangement { get; set; }

        private string GetPlanNote(string name)
        {
            return this.Item.plan_item_notes.FirstOrDefault(n => n.category_name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) > -1).Try(n => n.note);
        }

        public class Sequence 
        {
            public string Title { get; set; }

            public string Class 
            {
                get
                {
                    if (this.Title == null)
                        return string.Empty;

                    if (this.Title.IndexOf("Zwischenspiel", StringComparison.OrdinalIgnoreCase) > -1)
                        return "inst";

                    if (this.Title.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                        return this.Title.ToLower();

                    return this.Title.ToLower();
                }
            }
        }
    }

    public class ArrangementModel
    {
        private static readonly string[] delimiterString = new[] { "\r\n\r\n", "\n\n" };

        public ArrangementModel(Arrangement arrangement, IEnumerable<string> sequence)
        {
            if (arrangement == null)
                throw new ArgumentNullException("arrangement");

            this.Parts = new List<SongPart>();
            this.Create(arrangement.chord_chart);
            this.TranslateClasses(sequence);
        }

        public IList<SongPart> Parts { get; private set; }

        private void Create(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = Helper.TranslateLabels(text);

                text = Regex.Replace(text, @"(?!\r)\n", "\r\n");
                text = text.Replace("\r\r\n", "\r\n");

                string[] strArray = delimiterString[0] != "\r\n"
                    ? text.Split(delimiterString, StringSplitOptions.RemoveEmptyEntries)
                    : text.Split(delimiterString, StringSplitOptions.None);

                var stringBuilder = new StringBuilder();
                int num = 0;

                SongPart partModel = null;

                int index = -1;
                foreach (string part in strArray)
                {
                    index++;

                    if (!string.IsNullOrEmpty(part))
                    {
                        string trimmed = part.TrimStart(new char[0]);

                        if (index < (strArray.Length - 1) && Helper.SkipToNextLine(trimmed, strArray[index + 1]))
                            continue;

                        if (Helper.IsValidLabel(trimmed))
                        {
                            string slideText = stringBuilder.ToString();
                            stringBuilder.Clear();

                            if (!string.IsNullOrEmpty(slideText) && partModel != null)
                                partModel.AddText(slideText);

                            partModel = new SongPart();

                            if (trimmed.IndexOf("\r\n") < 0)
                            {
                                partModel.Label = trimmed;
                                num = 0;
                            }
                            else
                            {
                                partModel.Label = trimmed.Substring(0, trimmed.IndexOf("\r\n"));

                                stringBuilder.AppendLine(trimmed.Substring(trimmed.IndexOf("\r\n") + 2));
                                num = 1;
                            }

                            this.Parts.Add(partModel);
                        }
                        else
                        {
                            stringBuilder.Append(part);
                            ++num;

                            if (num == -1) //this.PresentationParams.DelemetersPerSlide)
                            {
                                string slideText = stringBuilder.ToString();
                                stringBuilder.Clear();
                                num = 0;

                                partModel.AddText(slideText);
                            }
                            else
                            {
                                stringBuilder.AppendLine();
                            }
                        }
                    }
                }

                partModel.AddText(stringBuilder.ToString());
            }
        }

        private void TranslateClasses(IEnumerable<string> sequence)
        {
            if (sequence == null)
                return;

            foreach (string seq in sequence.Select(s => s.ToLower()))
            {
                SongPart selected = null;

                if (seq.First() == 'v')
                {
                    if (seq.Length > 1)
                        selected = this.Parts.FirstOrDefault(g => g.Label.Equals("verse " + seq.Substring(1), StringComparison.OrdinalIgnoreCase));

                    if (selected == null)
                        selected = this.Parts.FirstOrDefault(g => g.Label.StartsWith("verse", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(g.Class));
                }
                else if (seq.First() == 'c')
                {
                    if (seq.Length > 1)
                        selected = this.Parts.FirstOrDefault(g => g.Label.Equals("chorus " + seq.Substring(1), StringComparison.OrdinalIgnoreCase));

                    if (selected == null)
                        selected = this.Parts.FirstOrDefault(g => g.Label.StartsWith("chorus", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(g.Class));
                }
                else if (seq.First() == 'b')
                {
                    if (seq.Length > 1)
                        selected = this.Parts.FirstOrDefault(g => g.Label.Equals("bridge " + seq.Substring(1), StringComparison.OrdinalIgnoreCase));

                    if (selected == null)
                        selected = this.Parts.FirstOrDefault(g => g.Label.StartsWith("bridge", StringComparison.OrdinalIgnoreCase));
                }
                else if (seq.StartsWith("pc"))
                {
                    if (seq.Length > 2)
                        selected = this.Parts.FirstOrDefault(g => g.Label.Equals("pre-chorus " + seq.Substring(2), StringComparison.OrdinalIgnoreCase));

                    if (selected == null)
                        selected = this.Parts.FirstOrDefault(g => g.Label.StartsWith("pre-chorus", StringComparison.OrdinalIgnoreCase));
                }
                else if (seq.StartsWith("misc"))
                {
                    if (seq.Length > 4)
                        selected = this.Parts.FirstOrDefault(g => g.Label.Equals("misc " + seq.Substring(4), StringComparison.OrdinalIgnoreCase));

                    if (selected == null)
                        selected = this.Parts.FirstOrDefault(g => g.Label.StartsWith("misc", StringComparison.OrdinalIgnoreCase));
                }

                if (selected != null)
                    selected.Class = seq;
            }
        }
    }

    internal static class Helper
    {
        private static readonly string[] labels = new[]
            {
              "verse",
              "chorus",
              "pre-chorus",
              "bridge",
              "misc"
            };

        public static string TranslateLabels(string text)
        {
            text = text
                .Replace("strophe", "verse")
                .Replace("Strophe", "verse")
                .Replace("refrain", "chorus")
                .Replace("Refrain", "chorus");

            text = Regex.Replace(text, @"(\d)+. verse", @"verse $1");
            text = Regex.Replace(text, @"\((pre-chorus|bridge)\)", "$1", RegexOptions.IgnoreCase);

            return text;
        }

        public static bool SkipToNextLine(string suggestedLabel, string nextLine)
        {
            return suggestedLabel.StartsWith("misc", StringComparison.InvariantCultureIgnoreCase) 
                && IsValidLabel(nextLine);
        }

        public static bool IsValidLabel(string suggestedLabel)
        {
            return Helper.labels.Any(label => suggestedLabel.StartsWith(label, StringComparison.InvariantCultureIgnoreCase));
        }

        public static IHtmlString Nl2br(string value)
        {
            if (string.IsNullOrEmpty(value))
                return MvcHtmlString.Empty;

            return MvcHtmlString.Create(string.Join("<br/>", Regex.Split(value, "\r\n|\r|\n").Select(HttpUtility.HtmlEncode)));
        }
    }

    public class SongPart
    {
        public SongPart()
        {
            this.Text = string.Empty;
        }

        public string Text { get; set; }

        public IHtmlString Html
        {
            get { return Helper.Nl2br(this.Text); }
        }

        public string Label { get; set; }

        public string Class { get; set; }

        public void AddText(string text)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Text += Environment.NewLine;
            }

            this.Text += text;
        }
    }
}