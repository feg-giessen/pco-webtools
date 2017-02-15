using System.Collections.Generic;
using Newtonsoft.Json;

namespace PcoBase
{
    public class Arrangement
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bpm")]
        public object Bpm { get; set; }

        [JsonProperty("has_chords")]
        public bool HasChords { get; set; }

        [JsonProperty("has_chord_chart")]
        public bool HasChordChart { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("sequence_to_s")]
        public string SequenceToS { get; set; }

        [JsonProperty("meter")]
        public string Meter { get; set; }

        [JsonProperty("song_id")]
        public int SongId { get; set; }

        [JsonProperty("update_by_id")]
        public int UpdateById { get; set; }

        [JsonProperty("created_by_id")]
        public int CreatedById { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("minutes")]
        public int Minutes { get; set; }

        [JsonProperty("seconds")]
        public int Seconds { get; set; }

        [JsonProperty("formatted_length")]
        public string FormattedLength { get; set; }

        public List<object> CustomZooms { get; set; }

        [JsonProperty("chord_chart")]
        public string ChordChart { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("chord_chart_key")]
        public string ChordChartKey { get; set; }

        [JsonProperty("chord_chart_font")]
        public string ChordChartFont { get; set; }

        [JsonProperty("chord_chart_columns")]
        public int? ChordChartColumns { get; set; }

        [JsonProperty("chord_chart_font_size")]
        public int? ChordChartFontSize { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }
    }
}