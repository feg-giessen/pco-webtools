using System.Collections.Generic;

namespace PcoBase
{
    public class Arrangement
    {
        public int id { get; set; }

        public string name { get; set; }

        public object bpm { get; set; }

        public bool has_chords { get; set; }

        public bool has_chord_chart { get; set; }

        public int length { get; set; }

        public string sequence_to_s { get; set; }

        public string meter { get; set; }

        public int song_id { get; set; }

        public int update_by_id { get; set; }

        public int created_by_id { get; set; }

        public string updated_at { get; set; }

        public string created_at { get; set; }

        public string notes { get; set; }

        public int minutes { get; set; }

        public int seconds { get; set; }

        public string formatted_length { get; set; }

        public List<object> custom_zooms { get; set; }

        public string chord_chart { get; set; }

        public List<Attachment> attachments { get; set; }

        public string chord_chart_key { get; set; }

        public string chord_chart_font { get; set; }

        public int chord_chart_columns { get; set; }

        public int chord_chart_font_size { get; set; }

        public List<Property> properties { get; set; }
    }
}